using Hahn.Domain.Entities.BaseEntity;
using System.ComponentModel.DataAnnotations;

namespace Hahn.Data.Dtos.Events;

public class EventsDto : CommonEntity
{
    [Required]
    public string Location { get; set; }
    [Required]
    public DateTime EventDate { get; set; }
    
    [Required]
    public string Title { get; set; }
    public int QntPeople { get; set; }
    public string ImagemUrl { get; set; }
}
