using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyCodeCamp.Data.Entities;

namespace MyCodeCamp.Data
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

    public IEnumerable<Prison> GetAllCamps()
    {
      return _context.Prisons
                .Include(c => c.Location)
                .OrderBy(c => c.EventDate)
                .ToList();
    }

    public Prison GetCamp(int id)
    {
      return _context.Prisons
        .Include(c => c.Location)
        .Where(c => c.Id == id)
        .FirstOrDefault();
    }

    public Prison GetCampWithSpeakers(int id)
    {
      return _context.Prisons
        .Include(c => c.Location)
        .Include(c => c.Prisoners)
        .ThenInclude(s => s.Talks)
        .Where(c => c.Id == id)
        .FirstOrDefault();
    }

    public Prison GetCampByMoniker(string moniker)
    {
      return _context.Prisons
        .Include(c => c.Location)
        .Where(c => c.Moniker.Equals(moniker, StringComparison.CurrentCultureIgnoreCase))
        .FirstOrDefault();
    }

    public Prison GetCampByMonikerWithSpeakers(string moniker)
    {
      return _context.Prisons
        .Include(c => c.Location)
        .Include(c => c.Prisoners)
        .ThenInclude(s => s.Talks)
        .Where(c => c.Moniker.Equals(moniker, StringComparison.CurrentCultureIgnoreCase))
        .FirstOrDefault();
    }

    public Prisoner GetSpeaker(int speakerId)
    {
      return _context.Prisoners
        .Include(s => s.Prison)
        .Where(s => s.Id == speakerId)
        .FirstOrDefault();
    }

    public IEnumerable<Prisoner> GetSpeakers(int id)
    {
      return _context.Prisoners
        .Include(s => s.Prison)
        .Where(s => s.Prison.Id == id)
        .OrderBy(s => s.Name)
        .ToList();
    }

    public IEnumerable<Prisoner> GetSpeakersWithTalks(int id)
    {
      return _context.Prisoners
        .Include(s => s.Prison)
        .Include(s => s.Talks)
        .Where(s => s.Prison.Id == id)
        .OrderBy(s => s.Name)
        .ToList();
    }

    public IEnumerable<Prisoner> GetSpeakersByMoniker(string moniker)
    {
      return _context.Prisoners
        .Include(s => s.Prison)
        .Where(s => s.Prison.Moniker.Equals(moniker, StringComparison.CurrentCultureIgnoreCase))
        .OrderBy(s => s.Name)
        .ToList();
    }

    public IEnumerable<Prisoner> GetSpeakersByMonikerWithTalks(string moniker)
    {
      return _context.Prisoners
        .Include(s => s.Prison)
        .Include(s => s.Talks)
        .Where(s => s.Prison.Moniker.Equals(moniker, StringComparison.CurrentCultureIgnoreCase))
        .OrderBy(s => s.Name)
        .ToList();
    }

    public Prisoner GetSpeakerWithTalks(int speakerId)
    {
      return _context.Prisoners
        .Include(s => s.Prison)
        .Include(s => s.Talks)
        .Where(s => s.Id == speakerId)
        .FirstOrDefault();
    }

    public Crime GetTalk(int talkId)
    {
      return _context.Crimes
        .Include(t => t.Prisoner)
        .ThenInclude(s => s.Prison)
        .Where(t => t.Id == talkId)
        .OrderBy(t => t.Title)
        .FirstOrDefault();
    }

    public IEnumerable<Crime> GetTalks(int speakerId)
    {
      return _context.Crimes
        .Include(t => t.Prisoner)
        .ThenInclude(s => s.Prison)
        .Where(t => t.Prisoner.Id == speakerId)
        .OrderBy(t => t.Title)
        .ToList();
    }

    public PrisonUser GetUser(string userName)
    {
      return _context.Users
        .Include(u => u.Claims)
        .Include(u => u.Roles)
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
