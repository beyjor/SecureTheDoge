using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SecureTheDoge.Controllers;
using SecureTheDoge.Data.Entities;

namespace SecureTheDoge.Models
{
    public class PrisonerUrlResolver : IValueResolver<Prisoner, PrisonerModel, string>
    {
        private IHttpContextAccessor _httpContextAccessor;

        public PrisonerUrlResolver(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string Resolve(Prisoner source, PrisonerModel destination, 
            string destMember, ResolutionContext conext)
        {
            var url = (IUrlHelper)_httpContextAccessor.HttpContext.Items[BaseController.URLHELPER];
            return url.Link("PrisonerGet", new { moniker = source.Prison.Moniker, id = source.Id });
        }
    }
}
