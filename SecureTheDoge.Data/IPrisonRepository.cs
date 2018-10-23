using System.Collections.Generic;
using System.Threading.Tasks;
using MyCodeCamp.Data.Entities;

namespace MyCodeCamp.Data
{
  public interface IPrisonRepository
  {
    // Basic DB Operations
    void Add<T>(T entity) where T : class;
    void Delete<T>(T entity) where T : class;
    Task<bool> SaveAllAsync();

    // Camps
    IEnumerable<Prison> GetAllCamps();
    Prison GetCamp(int id);
    Prison GetCampWithSpeakers(int id);
    Prison GetCampByMoniker(string moniker);
    Prison GetCampByMonikerWithSpeakers(string moniker);

    // Speakers
    IEnumerable<Prisoner> GetSpeakers(int id);
    IEnumerable<Prisoner> GetSpeakersWithTalks(int id);
    IEnumerable<Prisoner> GetSpeakersByMoniker(string moniker);
    IEnumerable<Prisoner> GetSpeakersByMonikerWithTalks(string moniker);
    Prisoner GetSpeaker(int speakerId);
    Prisoner GetSpeakerWithTalks(int speakerId);

    // Talks
    IEnumerable<Crime> GetTalks(int speakerId);
    Crime GetTalk(int talkId);

    // CampUser
    PrisonUser GetUser(string userName);
  }
}