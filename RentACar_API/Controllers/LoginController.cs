using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using RentACar_API.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RentACar_API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<LoginController> _logger;

        public LoginController(IConfiguration configuration, ILogger<LoginController> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        /// <summary>
        /// Login using the user name and password stored in appsettings.json and return JWT token.
        /// </summary>
        [HttpPost]
        [AllowAnonymous]
        public IActionResult Login(LoginModel loginModel)
        {
            try
            {
                var userName = _configuration["Login:UserName"];
                var password = _configuration["Login:Password"];

                if (loginModel.UserName != userName || loginModel.Password != password)
                    return Unauthorized();

                var key = _configuration["Jwt:Key"] ?? string.Empty;
                var issuer = _configuration["Jwt:Issuer"];
                var audience = _configuration["Jwt:Audience"];
                var expireMinutes = Convert.ToDouble(_configuration["Jwt:ExpireMinutes"] ?? "60");

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, loginModel.UserName)
                };

                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: issuer,
                    audience: audience,
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(expireMinutes),
                    signingCredentials: credentials);

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token)
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, "Something went wrong, please try again later.");
            }
        }
    }
}
