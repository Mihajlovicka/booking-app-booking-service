using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace BookingService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        //test method
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Hello World");
        }
    }
}
