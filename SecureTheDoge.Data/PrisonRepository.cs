using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SecureTheDoge.Data.Entities;

namespace SecureTheDoge.Data
{
  public class PrisonRepository : IPrisonRepository
  {
    private PrisonContext _context;

    public PrisonRepository(PrisonContext context)
    {
      _context = context;
    }

    public void Add<T>(T entity) where T : class
    {
      _context.Add(entity);
    }

    public void Delete<T>(T entity) where T : class
    {
      _context.Remove(entity);
    }

    public IEnumerable<Prison> GetAllPrisons()
    {
      return _context.Prisons
                .Include(c => c.Location)
                .OrderBy(c => c.EventDate)
                .ToList();
    }

    public Prison GetPrison(int id)
    {
      return _context.Prisons
        .Include(c => c.Location)
        .Where(c => c.Id == id)
        .FirstOrDefault();
    }

    public Prison GetPrisonWithPrisoners(int id)
    {
      return _context.Prisons
        .Include(c => c.Location)
        .Include(c => c.Prisoners)
        .ThenInclude(s => s.Crimes)
        .Where(c => c.Id == id)
        .FirstOrDefault();
    }

    public Prison GetPrisonByMoniker(string moniker)
    {
      return _context.Prisons
        .Include(c => c.Location)
        .Where(c => c.Moniker.Equals(moniker, StringComparison.CurrentCultureIgnoreCase))
        .FirstOrDefault();
    }

    public Prison GetPrisonByMonikerWithPrisoners(string moniker)
    {
      return _context.Prisons
        .Include(c => c.Location)
        .Include(c => c.Prisoners)
        .ThenInclude(s => s.Crimes)
        .Where(c => c.Moniker.Equals(moniker, StringComparison.CurrentCultureIgnoreCase))
        .FirstOrDefault();
    }

    public Prisoner GetPrisoner(int prisonerId)
    {
      return _context.Prisoners
        .Include(s => s.Prison)
        .Where(s => s.Id == prisonerId)
        .FirstOrDefault();
    }

    public IEnumerable<Prisoner> GetPrisoners(int id)
    {
      return _context.Prisoners
        .Include(s => s.Prison)
        .Where(s => s.Prison.Id == id)
        .OrderBy(s => s.Name)
        .ToList();
    }

    public IEnumerable<Prisoner> GetPrisonersWithCrimes(int id)
    {
      return _context.Prisoners
        .Include(s => s.Prison)
        .Include(s => s.Crimes)
        .Where(s => s.Prison.Id == id)
        .OrderBy(s => s.Name)
        .ToList();
    }

    public IEnumerable<Prisoner> GetPrisonersByMoniker(string moniker)
    {
      return _context.Prisoners
        .Include(s => s.Prison)
        .Where(s => s.Prison.Moniker.Equals(moniker, StringComparison.CurrentCultureIgnoreCase))
        .OrderBy(s => s.Name)
        .ToList();
    }

    public IEnumerable<Prisoner> GetPrisonersByMonikerWithCrimes(string moniker)
    {
      return _context.Prisoners
        .Include(s => s.Prison)
        .Include(s => s.Crimes)
        .Where(s => s.Prison.Moniker.Equals(moniker, StringComparison.CurrentCultureIgnoreCase))
        .OrderBy(s => s.Name)
        .ToList();
    }

    public Prisoner GetPrisonerWithCrimes(int prisonerId)
    {
      return _context.Prisoners
        .Include(s => s.Prison)
        .Include(s => s.Crimes)
        .Where(s => s.Id == prisonerId)
        .FirstOrDefault();
    }

    public Crime GetCrime(int crimeId)
    {
      return _context.Crimes
        .Include(t => t.Prisoner)
        .ThenInclude(s => s.Prison)
        .Where(t => t.Id == crimeId)
        .OrderBy(t => t.Title)
        .FirstOrDefault();
    }

    public IEnumerable<Crime> GetCrimes(int prisonerId)
    {
      return _context.Crimes
        .Include(t => t.Prisoner)
        .ThenInclude(s => s.Prison)
        .Where(t => t.Prisoner.Id == prisonerId)
        .OrderBy(t => t.Title)
        .ToList();
    }

    public PrisonUser GetUser(string userName)
    {
      return _context.Users
        .Where(u => u.UserName == userName)
        .Cast<PrisonUser>()
        .FirstOrDefault();
    }

    public async Task<bool> SaveAllAsync()
    {
      return (await _context.SaveChangesAsync()) > 0;
    }
  }
}
