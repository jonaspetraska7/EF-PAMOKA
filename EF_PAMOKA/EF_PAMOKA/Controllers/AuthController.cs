using EF_PAMOKA.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace EF_PAMOKA.Controllers
{
    [ApiController]
    [Route("/auth")]
    public class AuthController : ControllerBase
    {
        private IConfiguration _config;
        private readonly PavyzdinisDbContext _dbContext;

        public AuthController(IConfiguration config, PavyzdinisDbContext dbContext)
        {
            _config = config;
            _dbContext = dbContext;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("/login")]
        public IActionResult Login([FromBody] Vartotojas login)
        {
            IActionResult response = Unauthorized();
            var user = AuthenticateUser(login);

            if (user != null)
            {
                var tokenString = GenerateJSONWebToken(user);
                response = Ok(new { token = tokenString });
            }

            return response;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("/register")]
        public IActionResult Register([FromBody] Vartotojas login)
        {
            IActionResult response = Unauthorized();
            var userExist = _dbContext.Vartotojai.Any(x => x.Vardas == login.Vardas);

            if (!userExist)
            {
                var user = new Vartotojas
                {
                    Vardas = login.Vardas,
                    Slaptazodis = BCrypt.Net.BCrypt.HashPassword(login.Slaptazodis),
                    Pastas = ""
                };
                _dbContext.Vartotojai.Add(user);
                _dbContext.SaveChanges();
                var tokenString = GenerateJSONWebToken(user);
                response = Ok(new { token = tokenString });
            }

            return response;
        }

        private string GenerateJSONWebToken(Vartotojas userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(null,
              null,
              null,
              expires: DateTime.Now.AddMinutes(120),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private Vartotojas AuthenticateUser(Vartotojas login)
        {
            var user = _dbContext.Vartotojai.Where(x => x.Vardas == login.Vardas).FirstOrDefault();

            if (user != null && BCrypt.Net.BCrypt.Verify(login.Slaptazodis, user.Slaptazodis))
            {
                user = new Vartotojas { Vardas = user.Vardas, Pastas = user.Pastas };
                return user;
            }
            else
            {
                return null;
            }

        }





    }
}


