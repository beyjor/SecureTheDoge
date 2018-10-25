using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SecureTheDoge.Controllers;
using SecureTheDoge.Data.Entities;

namespace SecureTheDoge.Models
{
    public class PrisonUrlResolver : IValueResolver<Prison, PrisonModel, string>
    {
        private IHttpContextAccessor _httpContextAccessor;

        public PrisonUrlResolver(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string Resolve(Prison source, PrisonModel destination, 
            string destMember, ResolutionContext conext)
        {
            var url = (IUrlHelper)_httpContextAccessor.HttpContext.Items[BaseController.URLHELPER];
            return url.Link("PrisonGet", new { moniker = source.Moniker });
        }
    }
}
