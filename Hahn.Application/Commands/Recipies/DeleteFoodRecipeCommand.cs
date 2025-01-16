using MediatR;

namespace Hahn.Application.Commands.Recipies;

public class DeleteFoodRecipeCommand : IRequest<Unit>
{
    public Guid Id { get; }

    public DeleteFoodRecipeCommand(Guid id)
    {
        Id = id;
    }
}
