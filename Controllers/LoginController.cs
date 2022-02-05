using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using BCrypt.Net;
using libry.DataAccess;
using libry.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace libry.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private IConfiguration _config;
        private BooksContext _booksContext;

        public LoginController(IConfiguration config, BooksContext booksContext)
        {
            _config = config;
            _booksContext = booksContext;
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login([FromBody] UserLogin userLogin)
        {
            var newuser = Authenticate(userLogin);

            if (newuser != null)
            {
                var token = Generate(newuser);

                return Ok(token);
            }

            return NotFound("User not found");
        }

        private string Generate(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
           {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.LastName),
                new Claim(ClaimTypes.GivenName, user.FirstName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(ClaimTypes.Hash, user.Password)
            };
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddHours(1),
                SigningCredentials = credentials,
                Issuer = _config["Jwt:Issuer"],
                Audience = _config["Jwt:Audience"]
            };
            
            var tokenHandler = new JwtSecurityTokenHandler(
                
            );

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private User Authenticate(UserLogin userLogin)
        {
            var currentUser = _booksContext.Users.FirstOrDefault(o => o.Email.ToLower() == userLogin.Email.ToLower());

            try
            {
                 if (currentUser != null && BCrypt.Net.BCrypt.Verify(userLogin.Password, currentUser.Password))
                 {
                    return currentUser;
                }
            }
            catch (System.Exception ex)
            {

                Console.WriteLine(ex.Message);
            }

            return null;
        }
    }
}