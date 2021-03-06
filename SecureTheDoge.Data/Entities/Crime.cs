﻿using System;

namespace SecureTheDoge.Data.Entities
{
  public class Crime
  {
    public int Id { get; set; }
    public string Title { get; set; }
    public string Abstract { get; set; }
    public string Category { get; set; }
    public string Level { get; set; }
    public string PriorOffences { get; set; }
    public DateTime StartingTime { get; set; } = DateTime.Now;
    public string Room { get; set; }

    public Prisoner Prisoner { get; set; }

    public byte[] RowVersion { get; set; }

  }
}