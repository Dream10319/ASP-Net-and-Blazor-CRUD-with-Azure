using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NuGet.Common;
using Simple_BackEndAPI.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Simple_BackEndAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private UserContext _userContext;
        private readonly IConfiguration _config;

        public AuthController(UserContext userContext, IConfiguration config)
        {
            _userContext = userContext;
            _config = config;
        }

        //generate token with username and password
        [AllowAnonymous]
        [HttpPost]
        public Task<IActionResult> Auth([FromBody] UserAuth auth)
        {
            var user = Authenticate(auth);
            if (user != null)
            {
                var token = GenerateToken(user);
                return Task.FromResult<IActionResult>(Ok(new
                {
                    User_Id = user.User_Id,
                    Part_Id = user.Part_Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Utype_Id = user.Utype_Id,
                    Enabled = user.Enabled,
                    AccessToken = token
                }));
            }

            return Task.FromResult<IActionResult>(NotFound(new
            {
                Error = "User not found",
            }));
        }

        private string GenerateToken(User user)
        {
#pragma warning disable CS8604 // Possible null reference argument.
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:SecretKey"]));
#pragma warning restore CS8604 // Possible null reference argument.
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,user.FirstName),
                new Claim(ClaimTypes.NameIdentifier,user.LastName)
            };
            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);


            return new JwtSecurityTokenHandler().WriteToken(token);

        }

        //To authenticate user
        private User? Authenticate(UserAuth auth)
        {
#pragma warning disable CS8604 // Possible null reference argument.
            string enc_pass = Utils.Utils.Encrypt(auth.Password, _config["Jwt:SecretKey"]);
#pragma warning restore CS8604 // Possible null reference argument.
            var currentUser = _userContext.Users.FirstOrDefault(x => x.Email == auth.Email && x.Password == enc_pass);
            if (currentUser != null)
            {
                return currentUser;
            }
            return null;
        }
    }
}
