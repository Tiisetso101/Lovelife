using System;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using LoveLife.API.Controllers.Helpers;
using LoveLife.API.Data;
using LoveLife.API.Dtos;
using LoveLife.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LoveLife.API.Controllers
{

    [ServiceFilter(typeof(LogUserAcivity))]
    [Authorize]
    [Route("api/users/{userId}/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly IDatingRepository _repo;
        private readonly IMapper _mapper;
        public MessagesController(IDatingRepository repo, IMapper mapper)
        {
            this._mapper = mapper;
            this._repo = repo;

        }
        [HttpGet("{id}", Name = "GetMessage")]
        public async Task<IActionResult>GetMessage(int userId, int id)
        { 
             if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value) )
                return Unauthorized();

            var messageFromRepo = await _repo.GetMessage(id);

            if(messageFromRepo == null)
            return NotFound();

            return Ok(messageFromRepo);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMessage(int userId, MessageForCreationDto messageForCreationDto)
        {
              if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

                messageForCreationDto.senderId = userId;

                var recipient = await _repo.GetUser(messageForCreationDto.recipientId);

                if(recipient == null)
                    return BadRequest("Could not find user");

                var message = _mapper.Map<Message>(messageForCreationDto);
                _repo.add(message);

                var messageToReturn = _mapper.Map<MessageForCreationDto>(message);

                if(await _repo.SaveAll())
                    return CreatedAtRoute("GetMessage", new {id= message.Id}, messageToReturn);

                throw new Exception("Creating the message failed on saved");

              //  return Ok();
        }
    }
}