using Hahn.Data.Dtos.Events;
using MediatR;

namespace Hahn.Application.Queries.Events
{
    public class GetAllEventsQuery : IRequest<IEnumerable<EventsDto>>
    {
    }
}
