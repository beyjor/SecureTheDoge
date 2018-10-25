using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SecureTheDoge.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecureTheDoge.Models
{
    public class PrisonMappingProfile : Profile
    {
        public PrisonMappingProfile()
        {
            CreateMap<Prison, PrisonModel>()

                .ForMember(c => c.SentenceStart,
                opt => opt.MapFrom(prison => prison.EventDate))

                .ForMember(c => c.EligibleForParole,
                opt => opt.ResolveUsing(prison => prison.EventDate.AddDays(prison.Length - 1)))

                .ForMember(c => c.Url,
                opt => opt.ResolveUsing<PrisonUrlResolver>())

                .ReverseMap()

                .ForMember(m => m.EventDate,
                opt => opt.MapFrom(model => model.SentenceStart))

                .ForMember(m => m.Length,
                opt => opt.ResolveUsing(model => (model.EligibleForParole - model.SentenceStart).Days + 1))

                .ForMember(m => m.Length,
                opt => opt.ResolveUsing(c => new Location()
                {
                    Address1 = c.LocationAddress1,
                    Address2 = c.LocationAddress2,
                    Address3 = c.LocationAddress3,
                    CityTown = c.LocationCityTown,
                    StateProvince = c.LocationStateProvince,
                    PostalCode = c.LocationPostalCode,
                    Country = c.LocationCountry
                }));


            CreateMap<Prisoner, PrisonerModel>()

                .ForMember(c => c.Url,
                opt => opt.ResolveUsing<PrisonerUrlResolver>())

                .ReverseMap();


            CreateMap<Crime, CrimeModel>()

                .ForMember(c => c.Url,
                opt => opt.ResolveUsing<CrimeUrlResolver>())

                .ReverseMap();
        }
    }
}
