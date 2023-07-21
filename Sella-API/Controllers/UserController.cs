using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sella_API.Helpers;
using Sella_API.Model;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Text.RegularExpressions;
using System;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
using System.Security.Cryptography;
using Sella_API.DTO;
using NETCore.MailKit.Core;

namespace API_Sella.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly SellaDb DbContext; 
        private readonly IConfiguration Config;
        //private readonly IEmailService emailService;

        public UserController(SellaDb _dbContext, IConfiguration config) //IEmailService _emailService
        {
            DbContext = _dbContext;
            Config = config;
            //emailService = _emailService;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] User UserObj)
        {
                if (UserObj == null)
                    return BadRequest();

                var User = await DbContext.Users.FirstOrDefaultAsync(ww => ww.Email == UserObj.Email);
                if(User == null)
                {
                    return NotFound(new { Message = "User Not Found" });
                }

                if (!PasswordHasher.VarifyPassword(UserObj.Password , User.Password))
                {
                    return BadRequest(new {Message = "Password is Incorrect"});
                }

                User.Token = CreateJwtToken(User);

                return Ok(new { 
                    Token = User.Token,
                    User = (User.UserId).ToString(),
                    Message = "Success!" 
                });
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] User UserObj)
            {
                if (UserObj == null)
                {
                    return BadRequest();
                }

            
                //check Email Existance 
                if(await CheckEmailExistance(UserObj.Email))
                    return BadRequest(new { Message = "Email Already Exists!" });
            
                //check Password strength
                var Pass = CheckPasswordStrength(UserObj.Password);
                if (!string.IsNullOrEmpty(Pass))
                    return BadRequest(new { Message = Pass });

                UserObj.Password = PasswordHasher.HashPassword(UserObj.Password);
                UserObj.Role = "User";
                UserObj.Token = "";
                UserObj.Address = "";
                UserObj.Phone = "";

                await DbContext.Users.AddAsync(UserObj);
                await DbContext.SaveChangesAsync();
            

                //on success 
                return Ok(new { Message = "User Registered" });

            }

        //just for developing reasons 
        [Authorize]
        [HttpGet("GetAllUsers")]
        public IActionResult GetAllUsers()
            {
                var Users = DbContext.Users.ToList();
                return Ok(Users);
            }

        [HttpGet("api/User/{id:int}")]
        public IActionResult GetUserbyID(int id)
            {
                var User = DbContext.Users.Find(id);
                return Ok(User);
            }

        [HttpPut("api/User/{id:int}")]
        public IActionResult PutUser(int id,[FromBody] User user)
            {
                var User = DbContext.Users.Find(id);
                User.FirstName = user.FirstName;
                User.LastName = user.LastName;
                User.Email = user.Email;
        
                User.Address = user.Address;
                User.Phone = user.Phone;
            
                DbContext.SaveChanges();
                return Ok("Edit Successfully !");
            }

        private bool UserExists(int id)
            {
                return DbContext.Users.Any(e => e.UserId == id);
            }
    


        #region Backend Validatoin on Registeration
    //method to check Email Existance 
    private async Task<bool> CheckEmailExistance(string _email)
            {
                return await DbContext.Users.AnyAsync(z => z.Email == _email);
            }

            //method to check password strength
            private string CheckPasswordStrength(string _password)
            {
                StringBuilder sb = new StringBuilder();

                if (_password.Length < 8)
                    sb.Append("Minimum Password length should be 8" + Environment.NewLine);
                if (!(Regex.IsMatch(_password, "[a-z]")
                    && Regex.IsMatch(_password, "[A-Z]")
                    && Regex.IsMatch(_password, "[0-9]")))
                {
                    sb.Append("Minimum Password should be Alphnumeric" + Environment.NewLine);
                }
                if (!Regex.IsMatch(_password, "[!,@,#,$,%,^,&,*,(,),~,`,/,\\,?,>,<,=,-,+]"))
                {
                    sb.Append("Minimum Password should contain special character" + Environment.NewLine);
                }
                return sb.ToString();
            } 
            #endregion

        private string CreateJwtToken(User _user)
            {
                var JwtTokenHandler = new JwtSecurityTokenHandler();
                var Key = Encoding.ASCII.GetBytes("InTheNameOfAllah...");

                var Identity = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Role, _user.Role),
                    new Claim(ClaimTypes.Name, $"{_user.FirstName} {_user.LastName}")
                });

                var Credentials = new SigningCredentials(new SymmetricSecurityKey(Key), 
                    SecurityAlgorithms.HmacSha256);

                var TokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = Identity,
                    Expires = DateTime.Now.AddDays(1),
                    SigningCredentials = Credentials,

                };

                var Token = JwtTokenHandler.CreateToken(TokenDescriptor);

                return JwtTokenHandler.WriteToken(Token);
            }



        [HttpPost("send-reset-email/{email}")]
        public async Task<IActionResult> SendEmail(string email)
        {
            var user = await DbContext.Users.FirstOrDefaultAsync(z => z.Email == email);
            if (user is null)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = "email doesn't exist"
                });
            }
            var tokenBytes = RandomNumberGenerator.GetBytes(64);
            var emailToken = Convert.ToBase64String(tokenBytes);
            user.ResetPasswordToken = emailToken;
            user.ResetPasswordExpire = DateTime.Now.AddMinutes(15);
            string From = Config["EmailSettings:From"];
            var emailModel = new EmailModel(email, "Reset Password", EmailBody.EmailStringBody(email, emailToken) );
            Sella_API.Helpers.EmailService emailService = new Sella_API.Helpers.EmailService(Config);
            emailService.SendEmail(emailModel);
            DbContext.Entry(user).State = EntityState.Modified;
            await DbContext.SaveChangesAsync();
            return Ok(new
            {
                StatusCode = 200,
                Message= "Email Sent!"
            });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody]ResetPasswordDTO resetPasswordDTO)
        {
            //var newToken = resetPasswordDTO.EmailToken.Replace(" ", "+");
            var user = await DbContext.Users.AsNoTracking().FirstOrDefaultAsync(z => z.Email == resetPasswordDTO.Email);
            if (user is null)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = "user doesn't exist"
                });
            }

            var token = user.ResetPasswordToken;
            DateTime emailTokenExpire = user.ResetPasswordExpire;
            if (token != resetPasswordDTO.EmailToken && emailTokenExpire < DateTime.Now)
            {
                return BadRequest(new
                {
                    StatusCode= 400, 
                    Message= "Invalide Reset Link"
                });
            }

            user.Password = PasswordHasher.HashPassword(resetPasswordDTO.NewPassword);
            DbContext.Entry(user).State = EntityState.Modified;
            await DbContext.SaveChangesAsync();
            return Ok(new
            {
                SatutsCode = 200,
                Message = "Password Reset Successfully"
            });

        }

    }
}
