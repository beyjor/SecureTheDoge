﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using SecureTheDoge.Data.Entities;

namespace SecureTheDoge.Data
{
  public class PrisonDbInitializer
  {
    private PrisonContext _ctx;

    public PrisonDbInitializer(PrisonContext ctx)
    {
      _ctx = ctx;
    }

    public async Task Seed()
    {
      if (!_ctx.Prisons.Any())
      {
        // Add Data
        _ctx.AddRange(_sample);
        await _ctx.SaveChangesAsync();
      }
    }

    List<Prison> _sample = new List<Prison>
    {
      new Prison()
      {
        Name = "Your First Code Prison",
        Moniker = "ATL2016",
        EventDate = DateTime.Today.AddDays(45),
        Length = 1,
        Description = "This is the first code camp",
        Location = new Location()
        {
          Address1 = "123 Main Street",
          CityTown = "Atlanta",
          StateProvince = "GA",
          PostalCode = "30303",
          Country = "USA"
        },
        Prisoners = new List<Prisoner>
        {
          new Prisoner()
          {
            Name = "Shawn Wildermuth",
            Bio = "I'm a speaker",
            CompanyName = "Wilder Minds LLC",
            GitHubName = "shawnwildermuth",
            TwitterName = "shawnwildermuth",
            PhoneNumber = "555-1212",
            HeadShotUrl = "http://wilderminds.com/images/minds/shawnwildermuth.jpg",
            WebsiteUrl = "http://wildermuth.com",
            Crimes = new List<Crime>()
            {
              new Crime()
              {
                Title =  "How to do ASP.NET Core",
                Abstract = "How to do ASP.NET Core",
                Category = "Web Development",
                Level = "100",
                PriorOffences = "C# Experience",
                Room = "245",
                StartingTime = DateTime.Parse("14:30")
              },
              new Crime()
              {
                Title =  "How to do Bootstrap 4",
                Abstract = "How to do Bootstrap 4",
                Category = "Web Development",
                Level = "100",
                PriorOffences = "CSS Experience",
                Room = "246",
                StartingTime = DateTime.Parse("13:00")
              },
            }
          },
          new Prisoner()
          {
            Name = "Resa Wildermuth",
            Bio = "I'm a speaker",
            CompanyName = "Wilder Minds LLC",
            GitHubName = "resawildermuth",
            TwitterName = "resawildermuth",
            PhoneNumber = "555-1212",
            HeadShotUrl = "http://wilderminds.com/images/minds/resawildermuth.jpg",
            WebsiteUrl = "http://wildermuth.com",
            Crimes = new List<Crime>()
            {
              new Crime()
              {
                Title =  "Managing a Consulting Business",
                Abstract = "Managing a Consulting Business",
                Category = "Soft Skills",
                Level = "100",
                Room = "230",
                StartingTime = DateTime.Parse("10:30")
              }
            }
          }
        }
      }
    };

  }
}
