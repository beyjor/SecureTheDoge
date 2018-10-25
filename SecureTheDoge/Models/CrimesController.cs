using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SecureTheDoge.Controllers;
using SecureTheDoge.Data;
using SecureTheDoge.Data.Entities;
using SecureTheDoge.Filters;
using SecureTheDoge.Models;

namespace SecureTheDoge.Controllers
{
  [Route("api/prisons/{moniker}/prisoners/{prisonerId}/crimes")]
  [ValidateModel]
  public class CrimesController : BaseController
  {
    private ILogger<CrimesController> _logger;
    private IMapper _mapper;
    private IPrisonRepository _repo;

    public CrimesController(IPrisonRepository repo, ILogger<CrimesController> logger, IMapper mapper)
    {
      _repo = repo;
      _logger = logger;
      _mapper = mapper;
    }

    [HttpGet]
    public IActionResult Get(string moniker, int prisonerId)
    {
      var crimes = _repo.GetCrimes(prisonerId);

      if (crimes.Any(t => t.Prisoner.Prison.Moniker != moniker)) return BadRequest("Invalid crimes for the prisoner selected");

      return Ok(_mapper.Map<IEnumerable<CrimeModel>>(crimes));
    }

    [HttpGet("{id}", Name = "CrimeGet")]
    public IActionResult Get(string moniker, int prisonerId, int id)
    {
      var crime = _repo.GetCrime(id);

      if (crime.Prisoner.Id != prisonerId || crime.Prisoner.Prison.Moniker != moniker) return BadRequest("Invalid crime for the prisoner selected");

      return Ok(_mapper.Map<CrimeModel>(crime));
    }

    [HttpPost()]
    public async Task<IActionResult> Post(string moniker, int prisonerId, [FromBody] CrimeModel model)
    {
      try
      {
        var prisoner = _repo.GetPrisoner(prisonerId);
        if (prisoner != null)
        {
          var crime = _mapper.Map<Crime>(model);

          crime.Prisoner = prisoner;
          _repo.Add(crime);

          if (await _repo.SaveAllAsync())
          {
            return Created(Url.Link("GetCrime", new { moniker = moniker, prisonerId = prisonerId, id = crime.Id }), _mapper.Map<CrimeModel>(crime));
          }
        }

      }
      catch (Exception ex)
      {

        _logger.LogError($"Failed to save new crime: {ex}");
      }

      return BadRequest("Failed to save new crime");
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(string moniker, int prisonerId, int id, [FromBody] CrimeModel model)
    {
      try
      {
        var crime = _repo.GetCrime(id);
        if (crime == null) return NotFound();

        _mapper.Map(model, crime);

        if (await _repo.SaveAllAsync())
        {
          return Ok(_mapper.Map<CrimeModel>(crime));
        }

      }
      catch (Exception ex)
      {

        _logger.LogError($"Failed to update crime: {ex}");
      }

      return BadRequest("Failed to update crime");
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string moniker, int prisonerId, int id)
    {
      try
      {
        var crime = _repo.GetCrime(id);
        if (crime == null) return NotFound();

        _repo.Delete(crime);

        if (await _repo.SaveAllAsync())
        {
          return Ok();
        }

      }
      catch (Exception ex)
      {

        _logger.LogError($"Failed to delete crime: {ex}");
      }

      return BadRequest("Failed to delete crime");
    }

  }
}
