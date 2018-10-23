using System.Collections.Generic;

namespace MyCodeCamp.Data.Entities
{
  public class Prisoner
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public string CompanyName { get; set; }
    public string PhoneNumber { get; set; }
    public string WebsiteUrl { get; set; }
    public string TwitterName { get; set; }
    public string GitHubName { get; set; }
    public string Bio { get; set; }
    public string HeadShotUrl { get; set; }
    public PrisonUser User { get; set; }

    public ICollection<Crime> Talks { get; set; }
    public Prison Prison { get; set; }

    public byte[] RowVersion { get; set; }

  }
}