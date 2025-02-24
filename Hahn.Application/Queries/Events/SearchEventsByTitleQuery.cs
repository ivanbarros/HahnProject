using Hahn.Data.Dtos.Events;
using Hahn.Data.Dtos.Recipies;
using MediatR;

namespace Hahn.Application.Queries.Events
{
    public class SearchEventsByTitleQuery : IRequest<IEnumerable<EventsDto>>
    {
        public string Title { get; }
        public SearchEventsByTitleQuery(string title)
        {
            Title = title;
        }
    }
}
