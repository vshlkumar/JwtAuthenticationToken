using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BasicCoreApplication.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace BasicCoreApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public AccountController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        public IActionResult Login([FromBody]LoginViewModel loginDetails)
        {
            bool result = ValidateUser(loginDetails);
            if (result)
            {
                var tokenString = GenerateJWT();
                return Ok(new TokenResponseViewModel{ Token = tokenString,UserName=loginDetails.UserName });
            }
            else
            {
                return Unauthorized();
            }
        }

        private bool ValidateUser(LoginViewModel loginDetails)
        {
            if (loginDetails.UserName == "uncle.bob@gmail.com" &&
        loginDetails.Password == "Systems@123")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private string GenerateJWT()
        {
            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];
            var expiry = DateTime.Now.AddMinutes(5);
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                        issuer: issuer,
                        audience: audience,
                        expires: expiry,
                        signingCredentials: credentials
                        );
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenString = tokenHandler.WriteToken(token);
            return tokenString;
        }
    }
}