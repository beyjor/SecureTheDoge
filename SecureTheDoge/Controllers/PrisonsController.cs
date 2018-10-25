using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SecureTheDoge.Data;
using SecureTheDoge.Data.Entities;
using SecureTheDoge.Filters;
using SecureTheDoge.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecureTheDoge.Controllers
{
    [Route("api/[controller]")]
    [ValidateModel]
    public class PrisonsController : BaseController
    {
        private IPrisonRepository _repo;
        private ILogger<PrisonsController> _logger;
        private IMapper _mapper;

        public PrisonsController(IPrisonRepository repo, ILogger<PrisonsController> logger,
            IMapper mapper)
        {
            _repo = repo;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet("")]
        public IActionResult Get()
        {
            var prisons = _repo.GetAllPrisons();

            return Ok(_mapper.Map<IEnumerable<PrisonModel>>(prisons));
        }

        [HttpGet("{moniker}", Name = "PrisonGet")]
        public IActionResult Get(string moniker, bool includePrisoners = false)
        {
            try
            {
                Prison prison = null;

                if (includePrisoners) prison = _repo.GetPrisonByMonikerWithPrisoners(moniker);
                else prison = _repo.GetPrisonByMoniker(moniker);

                if (prison == null) return NotFound($"Prison {moniker} was not found");

                return Ok(_mapper.Map<PrisonModel>(prison, opt => opt.Items["UrlHelper"] = this.Url));
            }
            catch (Exception)
            {
            }

            return BadRequest();
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]PrisonModel model)
        {
            try
            {
                // Use a 'filter' instead! 
                //if (!ModelState.IsValid)
                //{
                //    _logger.LogInformation("Bad post request for a prison.");
                //    return BadRequest(ModelState);
                //}

                _logger.LogInformation("Posting a prison.");

                var prison = _mapper.Map<Prison>(model);

                _repo.Add(prison);
                if (await _repo.SaveAllAsync())
                {
                    var newUri = Url.Link("PrisonGet", new { moniker = model.Moniker });
                    return Created(newUri, _mapper.Map<PrisonModel>(prison));
                }
                else
                {
                    _logger.LogWarning("Could not post prison to DB.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Threw exception where saving prison: {ex}");
            }
            return BadRequest();
        }

        [HttpPatch("{moniker}")]
        [HttpPut("{moniker}")]
        public async Task<IActionResult> Put(string moniker, [FromBody] Prison model)
        {
            try
            {
                // Use a 'filter' instead! 
                //if (!ModelState.IsValid)
                //{
                //    _logger.LogInformation("Bad put request for a prison.");
                //    return BadRequest(ModelState);
                //}

                var oldPrison = _repo.GetPrisonByMoniker(moniker);
                if (oldPrison == null) return NotFound($"Could not find a prison with an ID of {moniker}");

                // Map model to the oldPrison
                oldPrison.Name = model.Name ?? oldPrison.Name;
                oldPrison.Description = model.Description ?? oldPrison.Description;
                oldPrison.Location = model.Location ?? oldPrison.Location;
                oldPrison.Length = model.Length > 0 ? model.Length : oldPrison.Length;
                oldPrison.EventDate = model.EventDate != DateTime.MinValue ? model.EventDate : oldPrison.EventDate;
                //_mapper.Map(model, oldPrison);


                if (await _repo.SaveAllAsync())
                {
                    return Ok(_mapper.Map<PrisonModel>(oldPrison));
                }
            }
            catch (Exception ex)
            {

            }

            return BadRequest("Couldn't update prison.");
        }

        [HttpDelete("{moniker}")]
        public async Task<IActionResult> Delete(string moniker)
        {
            try
            {

                var oldPrison = _repo.GetPrisonByMoniker(moniker);
                if (oldPrison == null) return NotFound($"Could not find a prison with an ID of {moniker}");

                _repo.Delete(oldPrison);
                if (await _repo.SaveAllAsync())
                {
                    return Ok();
                }
            }
            catch (Exception ex)
            {

            }

            return BadRequest("Couldn't delete prison.");
        }
    }
}
