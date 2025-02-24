using Hahn.Domain.Entities.BaseEntity;

namespace Hahn.Domain.Entities;

public class Events : CommonEntity
{
    public Events()
    {
    }

    public Events(string location, DateTime dateEvent, string title, int qntPeople, string imagemUrl)
    {
        Location = location;
        DateEvent = dateEvent;
        Title = title;
        QntPeople = qntPeople;
        ImagemUrl = imagemUrl;
    }

    public string Location { get; set; }
    public DateTime DateEvent { get; set; }
    public string Title { get; set; }
    public int QntPeople { get; set; }
    public string ImagemUrl { get; set; }
}
