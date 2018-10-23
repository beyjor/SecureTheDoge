using System.Collections.Generic;
using System.Threading.Tasks;
using SecureTheDoge.Data.Entities;

namespace SecureTheDoge.Data
{
  public interface IPrisonRepository
  {
    // Basic DB Operations
    void Add<T>(T entity) where T : class;
    void Delete<T>(T entity) where T : class;
    Task<bool> SaveAllAsync();

    // Prisons
    IEnumerable<Prison> GetAllPrisons();
    Prison GetPrison(int id);
    Prison GetPrisonWithPrisoners(int id);
    Prison GetPrisonByMoniker(string moniker);
    Prison GetPrisonByMonikerWithPrisoners(string moniker);

    // Prisoners
    IEnumerable<Prisoner> GetPrisoners(int id);
    IEnumerable<Prisoner> GetPrisonersWithCrimes(int id);
    IEnumerable<Prisoner> GetPrisonersByMoniker(string moniker);
    IEnumerable<Prisoner> GetPrisonersByMonikerWithCrimes(string moniker);
    Prisoner GetPrisoner(int prisonerId);
    Prisoner GetPrisonerWithCrimes(int prisonerId);

    // Crimes
    IEnumerable<Crime> GetCrimes(int prisonerId);
    Crime GetCrime(int crimeId);

    // PrisonUser
    PrisonUser GetUser(string userName);
  }
}