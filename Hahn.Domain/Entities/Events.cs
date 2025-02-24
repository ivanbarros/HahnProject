using Hahn.Domain.Entities.BaseEntity;

namespace Hahn.Domain.Entities;

public class Events : CommonEntity
{
    public string Location { get; set; }
    public DateTime DateEvent { get; set; }
    public string Title { get; set; }
    public int QntPeople { get; set; }
    public string ImagemUrl { get; set; }
}
