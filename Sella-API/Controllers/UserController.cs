using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sella_API.Model;

namespace API_Sella.Controllers
{
    public class UserController : Controller
    {
        private readonly SellaDb _dbContext;

        public UserController(SellaDb sellaDBContext)
        {
            _dbContext = sellaDBContext;
        }

        public SellaDb DbContext => _dbContext;

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

            await DbContext.Users.AddAsync(UserObj);
            await DbContext.SaveChangesAsync();
            return Ok(new { Message = "User Registered" });
        } 



    }
}
