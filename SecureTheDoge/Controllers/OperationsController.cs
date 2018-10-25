using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecureTheDoge.Controllers
{
    [Route("api/[controller]")]
    public class OperationsController : Controller
    {
        private IConfigurationRoot _configRoot;
        private ILogger<OperationsController> _logger;

        public OperationsController(ILogger<OperationsController> logger, IConfigurationRoot configuration)
        {
            _configRoot = configuration;
            _logger = logger;
        }

        [HttpOptions("reloadConfig")]
        public IActionResult ReloadConfiguration()
        {
            try
            {
                _configRoot.Reload();

                return Ok("Configuration Reloaded");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception thrown while reloading configuration: {ex}");
            }

            return BadRequest("Could not reload configuration.");
        }
    }
}
