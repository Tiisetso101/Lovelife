using System.Threading.Tasks;
using LoveLife.API.Data;
using LoveLife.API.Models;
using Microsoft.AspNetCore.Mvc;
using LoveLife.API.Data.Dtos;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System;
using System.IdentityModel.Tokens.Jwt;

namespace LoveLife.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public IAuthRepository _Repo { get; }
        public IConfiguration _Iconfig { get; }
        public AuthController(IAuthRepository repo, IConfiguration iconfig)
        {
            _Iconfig = iconfig;
            _Repo = repo;

        }
          [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto userForResisterDto)
        {
            //validate request

            userForResisterDto.username = userForResisterDto.username.ToLower();

            if (await _Repo.UserExists(userForResisterDto.username))
                return BadRequest("User name already exists!");

            var userTocreate = new User()
            {
                UserName = userForResisterDto.username
            };

            var createdUser = await _Repo.Register(userTocreate, userForResisterDto.password);


            return StatusCode(201);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto userForLoginDto)
        {
            
            var userFromRepo = await _Repo.Login(userForLoginDto.username.ToLower(), userForLoginDto.password);

            if (userFromRepo == null)
                return Unauthorized();

            var claims = new[]
            {
               new Claim(ClaimTypes.NameIdentifier, userFromRepo.Id.ToString()),
               new Claim(ClaimTypes.Name, userFromRepo.UserName)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8
            .GetBytes(_Iconfig.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDesriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDesriptor);


            return Ok(new{
                token = tokenHandler.WriteToken(token)
            });
        }
    }
}