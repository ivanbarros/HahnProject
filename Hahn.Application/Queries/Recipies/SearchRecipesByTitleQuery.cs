using Hahn.Data.Dtos.Recipies;
using MediatR;

namespace Hahn.Application.Queries.Recipies;

public class SearchRecipesByTitleQuery : IRequest<IEnumerable<FoodRecipeDto>>
{
    public string Title { get; }

    public SearchRecipesByTitleQuery(string title)
    {
        Title = title;
    }
}
