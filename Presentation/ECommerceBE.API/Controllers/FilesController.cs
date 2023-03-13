﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceBE.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        readonly IConfiguration _configuration;

        public FilesController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("[action]")]
        public IActionResult GetBaseStorageUrl()
        {
            string baseUrl = _configuration["BaseStorageUrl"];

            return Ok(new
            {
                url = baseUrl,
            });
        }
    }
}
