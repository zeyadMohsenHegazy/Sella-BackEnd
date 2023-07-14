using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using API_Sella.Helpers;
using API_Sella.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API_Sella.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthMangmentController : Controller
    {


        private readonly ILogger<AuthMangmentController> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly JWTCONFIG _jwtConfig;

        public AuthMangmentController(ILogger<AuthMangmentController> logger,
            UserManager<IdentityUser> userManager,
            IOptionsMonitor<JWTCONFIG> optionsMonitor)
        {
            _logger = logger;
            _userManager = userManager;
            _jwtConfig = optionsMonitor.CurrentValue;
        }



        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] UserRegestareRequest request)
        {
            if (ModelState.IsValid)
            {
                //check if email exist
                var emailExist = await _userManager.FindByEmailAsync(request.Email);
                if (emailExist != null)
                    return BadRequest("Email already Exist");

                var newUser = new IdentityUser()
                {
                    Email = request.Email,
                    UserName = request.Email
                };

                var isCreated = await _userManager.CreateAsync(newUser, request.Password);

                if (isCreated.Succeeded)
                {
                    var token = GenerateJwtToken(newUser);
                    //Generate a token
                    return Ok(new RegistrationRequestResult()
                    {
                        Result = true,
                        Token = token
                    });
                }
                return BadRequest("error creating the user, please try again later");

            }
            return BadRequest("Invalid request payload");
        }


        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest userLogin)
        {
            if (ModelState.IsValid)
            {
                var ExistUser = await _userManager.FindByEmailAsync(userLogin.Email);

                if (ExistUser == null)
                    return BadRequest("Invalid Authentication");

                var isPasswordValid = await _userManager.CheckPasswordAsync(ExistUser, userLogin.Password);


                if (isPasswordValid)
                {
                    var token = GenerateJwtToken(ExistUser);

                    return Ok(new LoginRequestResult()
                    {
                        Token = token,
                        Result = true
                    });
                }
                return BadRequest("Invalid Authentication");
            }
            return BadRequest("Invalid Payload");
        }



        private string GenerateJwtToken(IdentityUser user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtConfig.Key);
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("Id", user.Id),
                    new Claim(JwtRegisteredClaimNames.Sub, value: user.Email),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(25),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256)
            };
            var Token = jwtTokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = jwtTokenHandler.WriteToken(Token);
            return jwtToken;
        }

    }
}
