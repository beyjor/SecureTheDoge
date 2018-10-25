using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SecureTheDoge.Controllers;
using SecureTheDoge.Data.Entities;

namespace SecureTheDoge.Models
{
  public class CrimeUrlResolver : IValueResolver<Crime, CrimeModel, string>
  {
    private IHttpContextAccessor _httpContextAccessor;

    public CrimeUrlResolver(IHttpContextAccessor httpContextAccessor) 
    {
      _httpContextAccessor = httpContextAccessor;
    }

    public string Resolve(Crime source, CrimeModel destination, string destMember, ResolutionContext context)
    {
      var helper = (IUrlHelper)_httpContextAccessor.HttpContext.Items[BaseController.URLHELPER];
      return helper.Link("CrimeGet", new { moniker = source.Prisoner.Prison.Moniker, prisonerId = source.Prisoner.Id, id = source.Id });
    }
  }
}
