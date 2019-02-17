using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using LoveLife.API.Data;
using LoveLife.API.Data.Dtos;
using Microsoft.AspNetCore.Authorization;
using LoveLife.API.Controllers.Helpers;
using Microsoft.AspNetCore.Mvc;
using LoveLife.API.Models;

namespace LoveLife.API.Controllers
{
    [ServiceFilter(typeof(LogUserAcivity))]
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IDatingRepository _repo;
        private readonly IMapper _mapper;
        public UsersController(IDatingRepository repo, IMapper mapper)
        {
            _mapper = mapper;
            _repo = repo;

        }
        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery]UserParams userParams)
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var userFromRepo = await _repo.GetUser(currentUserId);
            var users = await _repo.GetUsers(userParams);
            var usersToReturn = _mapper.Map<IEnumerable<UserForListDto>>(users);
            userParams.UserId = currentUserId;
            if(string.IsNullOrEmpty(userParams.Gender))
            {
                userParams.Gender = userFromRepo.Gender == "male" ? "female":"male";
            }
            return Ok(usersToReturn);
        }

        [HttpGet("{id}", Name="GetUser")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _repo.GetUser(id);
            var userToReturn  = _mapper.Map<UserForDetailedDto>(user);
          //  userParams.UserId = 

           // Response.AddPagination(users.CurrentPage, users.PageSize,
             //User, User.TotalPages);

             Response.AddPadinationHeader(user.CurrentPage,user.ItemsPerPage,user.TotalItems , user.TotalPages);

            return Ok(userToReturn);
        }
        [HttpPut("{id}")]
        public async Task <IActionResult> UpdateUser(int id, UserForUpdateDto userForUpdateDto)
        {
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value) )
                return Unauthorized();

            var userFromRepo = await _repo.GetUser(id);
            _mapper.Map(userForUpdateDto, userFromRepo);

            if(await _repo.SaveAll())
                return NoContent();

            throw new Exception("$Updating user {id} failed on save"); 
        }
        [HttpPost("{id}/like/{recipientId}")]
        public async Task <IActionResult> LikeUser(int id, int recipientId)
        {
            if(id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var like = await _repo.GetLike(id, recipientId);

            if(like != null)
             return BadRequest("You like this user already");
             
            if(await _repo.GetUser(recipientId) == null)
             return NotFound();

            like = new Like
            {
                LikerId = id,
                LikeeId = recipientId
            };

           _repo.add<Like>(like);
            if(await _repo.SaveAll())
                return Ok();

            return BadRequest("Could not like user");
        }

    }
}