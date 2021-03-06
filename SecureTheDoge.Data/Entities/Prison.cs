﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureTheDoge.Data.Entities
{
  public class Prison
  {
    public int Id { get; set; }
    public string Moniker { get; set; }
    public string Name { get; set; }
    public DateTime EventDate { get; set; } = DateTime.MinValue;
    public int Length { get; set; }
    public string Description { get; set; }
    public Location Location { get; set; }

    public ICollection<Prisoner> Prisoners {get;set;}

    public byte[] RowVersion { get;set;}
  }
}
