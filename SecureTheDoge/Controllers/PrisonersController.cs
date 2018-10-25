using AutoMapper;
using Microsoft.AspNetCore.Cors;
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
    //[EnableCors("AnyGet")]
    [Route("api/prisons/{moniker}/prisoners")]
    [ValidateModel]
    public class PrisonersController : BaseController
    {
        private IPrisonRepository _repo;
        private ILogger<PrisonersController> _logger;
        private IMapper _mapper;

        public PrisonersController(IPrisonRepository repo, ILogger<PrisonersController> logger,
            IMapper mapper)
        {
            _repo = repo;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Get(string moniker, bool includeCrimes = false)
        {
            var prisoners = includeCrimes ? _repo.GetPrisonersByMonikerWithCrimes(moniker) : _repo.GetPrisonersByMoniker(moniker);

            return Ok(_mapper.Map<IEnumerable<PrisonerModel>>(prisoners));
        }

        [HttpGet("{id}", Name="PrisonerGet")]
        public IActionResult Get(string moniker, int id, bool includeCrimes = false)
        {
            var prisoner = includeCrimes ? _repo.GetPrisonerWithCrimes(id) : _repo.GetPrisoner(id);

            if (prisoner == null) return NotFound();

            if (prisoner.Prison.Moniker != moniker) return BadRequest("Prisoner is not encarcerated in that prison.");

            return Ok(_mapper.Map<PrisonerModel>(prisoner));
        }

        [HttpPost]
        public async Task<IActionResult> Post(string moniker, [FromBody] PrisonerModel model)
        {
            try
            {
                //if (!ModelState.IsValid) return BadRequest(ModelState);
                // Use a 'filter' instead! 

                var prison = _repo.GetPrisonByMoniker(moniker);

                if (prison == null) return BadRequest("No such prison exists.");

                var prisoner = _mapper.Map<Prisoner>(model);
                prisoner.Prison = prison;

                _repo.Add(prisoner);

                if (await _repo.SaveAllAsync())
                {
                    var url = Url.Link("PrisonerGet", new { moniker = prison.Moniker, id = prisoner.Id });
                    return Created(url, _mapper.Map<PrisonerModel>(prisoner));
                }
            }
            catch(Exception ex)
            {
                _logger.LogError($"Exception thrown while adding prisoner: {ex}");
            }

            return BadRequest("Could not add new prisoner");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string moniker, int id, [FromBody] PrisonerModel model)
        {
            try
            {
                //if (!ModelState.IsValid) return BadRequest(ModelState);
                // Use a 'filter' instead! 
                var prison = _repo.GetPrisonByMoniker(moniker);
                if (prison == null) return BadRequest("No such prison exists.");

                var prisoner = _repo.GetPrisoner(id);
                if (prisoner == null) return NotFound();
                if (prisoner.Prison.Moniker != moniker) return BadRequest("Prisoner is not encarcerated in that prison.");

                _mapper.Map(model, prisoner);

                if (await _repo.SaveAllAsync())
                {
                    return Ok(_mapper.Map<PrisonerModel>(prisoner));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception thrown while updating prisoner: {ex}");
            }

            return BadRequest("Could not update prisoner");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string moniker, int id)
        {
            try
            {
                var prisoner = _repo.GetPrisoner(id);
                if (prisoner == null) return NotFound();
                if (prisoner.Prison.Moniker != moniker) return BadRequest("Prisoner is not encarcerated in that prison.");

                _repo.Delete(prisoner);

                if (await _repo.SaveAllAsync())
                {
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception thrown while deleting prisoner: {ex}");
            }

            return BadRequest("Could not delete prisoner");
        }
    }
}
