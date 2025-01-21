using Hahn.Data.Dtos.Recipies;
using MediatR;

namespace Hahn.Application.Queries.Recipies;

public class SearchRecipiesByTitleQuery : IRequest<IEnumerable<FoodRecipeDto>>
{
    public string Title { get; }

    public SearchRecipiesByTitleQuery(string title)
    {
        Title = title;
    }
}
