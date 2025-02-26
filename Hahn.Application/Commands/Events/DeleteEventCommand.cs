using MediatR;

namespace Hahn.Application.Commands.Events
{
    public class DeleteEventCommand : IRequest<bool>
    {
        public DeleteEventCommand(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}
