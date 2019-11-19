using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using WebApi.Domain.Entities;
using WebApi.Domain.Services.Interfaces;

namespace WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AttendedsController : ControllerBase
    {
        private readonly IAttendedService _service;
        public AttendedsController(IAttendedService service)
        {
            this._service = service;
        }

        // GET api/attendeds
        [HttpGet()]
        public ActionResult<IEnumerable<string>> GetAll()
        {
            return Ok(this._service.GetAll());
        }

        // GET api/attendeds
        [HttpGet("search")]
        public ActionResult<IEnumerable<string>> GetByName(string search)
        {
            return Ok(this._service.GetByName(search));
        }

        // GET api/attendeds/5
        [HttpGet("{id}")]
        public ActionResult<string> GetById(Guid id)
        {
            return Ok(_service.GetById(id));
        }

        // POST api/attendeds
        [HttpPost("")]
        public ActionResult Post(Attended attended)
        {
            try
            {
                var result = this._service.Insert(attended);
                if (result)
                    return Ok();
                else
                    return BadRequest();
            }
            catch (Exception ex)
            {
                int number = ((MySqlException) ex.InnerException).Number;
                if (number == 1062)
                    return StatusCode(409, "Registration Number: DUPLICATED KEY");
                else        
                    return StatusCode(400, "Bad Request");
            }
        }

        // PUT api/attendeds/5
        [HttpPut("{id}")]
        public void Put(Guid id, [FromBody] Attended attended)
        {
            var result = this._service.Update(id, attended);
            if (result)
                Ok();
            else
                BadRequest();
        }

        // DELETE api/attendeds/5
        [HttpDelete("{id}")]
        public void Delete(Guid id)
        {
            this._service.Delete(id);
        }
    }
}