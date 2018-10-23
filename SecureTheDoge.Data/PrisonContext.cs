using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SecureTheDoge.Data.Entities;

namespace SecureTheDoge.Data
{
  public class PrisonContext : IdentityDbContext
  {
    private IConfiguration _config;

    public PrisonContext(DbContextOptions options, IConfiguration config)
      : base(options)
    {
      _config = config;
    }

    public DbSet<Prison> Prisons { get; set; }
    public DbSet<Prisoner> Prisoners { get; set; }
    public DbSet<Crime> Crimes { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
      base.OnModelCreating(builder);

      builder.Entity<Prison>()
        .Property(c => c.Moniker)
        .IsRequired();
      builder.Entity<Prison>()
        .Property(c => c.RowVersion)
        .ValueGeneratedOnAddOrUpdate()
        .IsConcurrencyToken();
      builder.Entity<Prisoner>()
        .Property(c => c.RowVersion)
        .ValueGeneratedOnAddOrUpdate()
        .IsConcurrencyToken();
      builder.Entity<Crime>()
        .Property(c => c.RowVersion)
        .ValueGeneratedOnAddOrUpdate()
        .IsConcurrencyToken();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      base.OnConfiguring(optionsBuilder);

      optionsBuilder.UseSqlServer(_config["Data:ConnectionString"]);
    }
  }
}
