using Hahn.Data.Dtos.Recipies;
using MediatR;

namespace Hahn.Application.Queries.Recipies;

public class GetRecipeByIdQuery : IRequest<FoodRecipeDto>
{
    public Guid Id { get; }

    public GetRecipeByIdQuery(Guid id)
    {
        Id = id;
    }
}
