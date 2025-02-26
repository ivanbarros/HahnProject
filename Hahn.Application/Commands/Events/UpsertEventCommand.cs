using Hahn.Data.Dtos.Events;
using MediatR;

namespace Hahn.Application.Commands.Events;

public class UpsertEventCommand : IRequest<EventsDto>
{
    public UpsertEventCommand(UpsertEventDto dto)
    {
        Dto = dto;
    }

    public UpsertEventDto Dto { get; set; }
}
