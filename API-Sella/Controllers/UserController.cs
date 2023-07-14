using API_Sella.Context;
using API_Sella.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_Sella.Controllers
{
    public class UserController : Controller
    {
        private readonly SellaDBContext _dbContext;

        public UserController(SellaDBContext sellaDBContext)
        {
            _dbContext = sellaDBContext;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] User UserObj)
        {
            if (UserObj == null)
                return BadRequest();

            var User = await _dbContext.Users.FirstOrDefaultAsync(ww => ww.Email == UserObj.Email && ww.Password == UserObj.Password);
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

            await _dbContext.Users.AddAsync(UserObj);
            await _dbContext.SaveChangesAsync();
            return Ok(new { Message = "User REgistered" });
        } 



    }
}
