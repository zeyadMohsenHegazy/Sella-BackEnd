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

    namespace API_Sella.Controllers
    {
        public class UserController : Controller
        {
            private readonly SellaDb DbContext;

            public UserController(SellaDb _dbContext)
            {
               DbContext = _dbContext ;
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
        }
    }
