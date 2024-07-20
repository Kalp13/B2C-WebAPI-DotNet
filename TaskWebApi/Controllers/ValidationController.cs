﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TaskWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValidationController : ControllerBase
    {
        [HttpGet("ValidateActive")]
        public IActionResult ValidateActive()
        {
            return Ok("API Active");
        }

        [HttpGet("ValidateAccessCode")]
        public async Task<IActionResult> ValidateAccessCode([FromQuery] string accessCode)
        {
            // Validate the access code
            if (accessCode == "54321")
            {
                return Ok("Success");
            }
            else
            {
                return Conflict();
            }
        }
    }
}
