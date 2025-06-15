using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OrderManagementSystem.API.Controllers
{
    [Route("api/authentication")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AuthenticationController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        [Route("login")]
        public IActionResult Login(string username, string password)
        {
            //for demonstration
            username = _configuration.GetSection("Credentials")["Username"];
            password = _configuration.GetSection("Credentials")["Password"];

            //claims
            var claims = new[]
           {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var key = new SymmetricSecurityKey
                (Encoding.UTF8.GetBytes(_configuration.GetSection("JwtSettings")["SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            //create token
            var token = new JwtSecurityToken(issuer: _configuration.GetSection("JWTSettings")["Issuer"],
                audience: _configuration.GetSection("JWTSettings")["Audience"],
                claims: claims, expires: DateTime.Now.AddHours(1),
                signingCredentials: creds);

            return Ok(new { Token = new JwtSecurityTokenHandler().WriteToken(token) });
        }
    }
}