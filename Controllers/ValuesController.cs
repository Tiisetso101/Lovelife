using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LoveLife.API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LoveLife.API.Models;
using Microsoft.AspNetCore.Authorization;

namespace loveLife.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly DataContext _context;

        public ValuesController(DataContext context)
        {
            _context = context;
        }
        // GET api/values
        [HttpGet]
        [AllowAnonymous]
        public async Task <IActionResult> GetValues()
        {
            var values = await  _context.Values.ToListAsync();
            return Ok(values);
           
          //  return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [AllowAnonymous]
        [HttpGet("{id}")]
        public IActionResult GetValue(int id)
        {
            var val =  _context.Values.FirstOrDefault(x => x.Id ==id);
            return Ok(val);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
