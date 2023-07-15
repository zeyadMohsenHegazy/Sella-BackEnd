using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sella_API.Helpers;
using Sella_API.Model;
using System.Text;
using System.Text.RegularExpressions;

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

            var User = await DbContext.Users.FirstOrDefaultAsync(ww => ww.Email == UserObj.Email && ww.Password == UserObj.Password);
            if(User == null)
            {
                return NotFound(new { Message = "User Not Found" });
            }

            return Ok(new { Message = "Success!" });
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] User UserObj)
        {
            if (UserObj == null)
            {
                return BadRequest();
            }

            //check user Name existance 
            if (await CheckUserNameExistance(UserObj.UserName))
                return BadRequest(new { Message = "User Name Already Exists!" });
            
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

            await DbContext.Users.AddAsync(UserObj);
            await DbContext.SaveChangesAsync();
            return Ok(new { Message = "User Registered" });
        }


        //just for developing reasons 
        [HttpGet("GetAllUsers")]
        public IActionResult GetAllUsers()
        {
            var Users = DbContext.Users.ToList();
            return Ok(Users);
        }

        #region Backend Validatoin on Registeration

        //method to check username existance
        private async Task<bool> CheckUserNameExistance(string _userName)
        {
            return await DbContext.Users.AnyAsync(z => z.UserName == _userName);
        }

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
    }
}
