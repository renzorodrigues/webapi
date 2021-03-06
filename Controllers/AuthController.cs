using System;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using WebApi.Domain.Entities;
using WebApi.Domain.Services.Interfaces;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            this._authService = authService;
        }

        // POST api/auth
        [HttpPost]
        public IActionResult Authenticate(User credentials)
        {
            if (string.IsNullOrEmpty(credentials.Email) || string.IsNullOrEmpty(credentials.Password))
                return StatusCode(400, "Bad Request: one of the credentials was not sent");

            var token = this._authService.Authenticate(credentials);

            if (!string.IsNullOrEmpty(token))
                return Ok(token);
            else
                return StatusCode(401, "401 Unauthorized");
        }

        // POST api/register
        [HttpPost("register")]
        public IActionResult Register(User credentials)
        {      
            try
            {
                if (this._authService.Register(credentials))
                    return StatusCode(201, "User created");
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
    }
}