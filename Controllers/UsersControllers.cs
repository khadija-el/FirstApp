using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FistApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
// using Helpers;

namespace FistApi.Controllers
{
    //[Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UsersController : SuperController<User>
    {
        // private readonly AppSettings _appSettings;
        public UsersController(FistApiContext context) : base(context)
        {
            // _appSettings = appSettings.Value;
        }

        [HttpGet("{startIndex}/{pageSize}")]
        public async Task<IActionResult> EL(int startIndex, int pageSize)
        {

            var q = _context.Users
                ;


            int count = await q.CountAsync();

            var list = await q.Skip(startIndex)
                   .Take(pageSize)
                //    .Include(e => e.Organisme)
                   .Include(e => e.Profil)
                   .ToListAsync()
               ;

            return Ok(new { list = list, count = count });
        }

        [HttpGet("{startIndex}/{pageSize}/{sortBy}/{sortDir}/{nom}/{prenom}/{organisme}")]
        public async Task<IActionResult> GetAll2(int startIndex, int pageSize, string sortBy, string sortDir, string nom, string prenom, int organisme)
        {

            var q = _context.Users.Where(u => nom == "*" ? true : u.Nom.Contains(nom))
                .Where(u => prenom == "*" ? true : u.Prenom.Contains(prenom))
                ;


            int count = await q.CountAsync();

            var list = await q.Skip(startIndex)
                   .Take(pageSize)
                //    .Include(e => e.Organisme)
                   .Include(e => e.Profil)
                   .ToListAsync()
               ;

            return Ok(new { list = list, count = count });
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<User>> LogIn(UserDTO model)
        {
            if (string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
                return BadRequest("Email ou mot pass sont vide");

            var user = await _context.Users
                // .Include(u => u.Organisme)
                .SingleOrDefaultAsync(x => x.Email == model.Email)
                ;

            Profil role = null;
            // check if username exists
            if (user == null)
                return BadRequest("Email est pas correct");

            if (user.Password == model.Password)
            {
                role = await _context.Profils.FirstOrDefaultAsync(e => e.Id == user.IdProfil);
                // authentication successful so generate jwt token
                var tokenHandler = new JwtSecurityTokenHandler();
                // var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                // var key = Encoding.ASCII.GetBytes("this is the secret phrase");
                // var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                var claims = new Claim[]
                    {
                        new Claim(ClaimTypes.Name, user.Id.ToString()),
                        new Claim(ClaimTypes.Email, user.Email),
                        new Claim(ClaimTypes.Role, role.Id.ToString())
                    };

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.UtcNow.AddDays(7),
                    // SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var createToken = tokenHandler.CreateToken(tokenDescriptor);
                var token = tokenHandler.WriteToken(createToken);
                // var token = tokenHandler.ReadJwtToken(createToken);

                // remove password before returning
                user.Password = "";
                user.Profil = role;
                return Ok(new { user, token, idRole = role.Id });
            }

            return BadRequest("Mot pass est pas correct");
        }

    }

    public class UserDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}