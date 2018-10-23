using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecureTheDoge.Controllers
{
    [Route("api/[controller]")]
    public class PrisonsController : Controller
    {
        [HttpGet("")]
        public IActionResult Get()
        {
            return Ok(new { Name = "Shown", FavoriteColor = "Blue" });
        }
    }
}
