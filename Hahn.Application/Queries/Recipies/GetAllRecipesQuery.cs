using Hahn.Data.Dtos.Recipies;
using MediatR;

namespace Hahn.Application.Queries.Recipies;

public class GetAllRecipesQuery : IRequest<IEnumerable<FoodRecipeDto>>
{
}
