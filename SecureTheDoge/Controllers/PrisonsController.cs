using Microsoft.AspNetCore.Mvc;
using SecureTheDoge.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecureTheDoge.Controllers
{
    [Route("api/[controller]")]
    public class PrisonsController : Controller
    {
        private IPrisonRepository _repo;

        public PrisonsController(IPrisonRepository repo)
        {
            _repo = repo;
        }

        [HttpGet("")]
        public IActionResult Get()
        {
            var prisons = _repo.GetAllPrisons();

            return Ok(prisons);
        }
    }
}
