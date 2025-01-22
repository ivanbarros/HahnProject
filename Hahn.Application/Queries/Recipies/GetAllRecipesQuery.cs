using Hahn.Data.Dtos.Recipies;
using MediatR;

namespace Hahn.Application.Queries.Recipies;

public class GetAllRecipiesQuery : IRequest<IEnumerable<FoodRecipeDto>>
{
}
