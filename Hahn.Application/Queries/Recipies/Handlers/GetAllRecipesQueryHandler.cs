using Hahn.Data.Dtos.Recipies;
using Hahn.Data.Interfaces.Repositories;
using MediatR;

namespace Hahn.Application.Queries.Recipies.Handlers;

public class GetAllRecipesQueryHandler : IRequestHandler<GetAllRecipesQuery, IEnumerable<FoodRecipeDto>>
{
    private readonly IRecipeRepository _recipeRepository;
    private readonly ILogger<GetAllRecipesQueryHandler> _logger;

    public GetAllRecipesQueryHandler(IRecipeRepository recipeRepository, ILogger<GetAllRecipesQueryHandler> logger)
    {
        _recipeRepository = recipeRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<FoodRecipeDto>> Handle(GetAllRecipesQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Fetching all recipes.");
        var recipes = await _recipeRepository.GetAllAsync();
        var recipeDtos = _recipeRepository.MapToDtos(recipes);
        _logger.LogInformation("Retrieved {Count} recipes.", recipeDtos.Count());
        return recipeDtos;
    }
}
