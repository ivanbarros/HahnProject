using Hahn.Domain.Entities.BaseEntity;

namespace Hahn.Data.Dtos.Events;

public class EventsDto : CommonEntity
{
    public string Location { get; set; }
    public DateTime EventDate { get; set; }
    public string Title { get; set; }
    public int QntPeople { get; set; }
    public string ImagemUrl { get; set; }
}
