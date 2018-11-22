using System.Threading.Tasks;
using LoveLife.API.Data;
using LoveLife.API.Models;
using Microsoft.AspNetCore.Mvc;
using LoveLife.API.Data.Dtos;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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
            var userFromRepo = await _Repo.Login(userForLoginDto.username, userForLoginDto.password);

            if (userFromRepo == null)
                return Unauthorized();

            var claims = new[]
            {
               new Claim(ClaimTypes.NameIdentifier, userFromRepo.Id.ToString()),
               new Claim(ClaimTypes.Name, userFromRepo.UserName)
            };

            //var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes() )
            return StatusCode(201);
        }
    }
}