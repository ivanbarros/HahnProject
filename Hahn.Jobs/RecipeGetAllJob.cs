
using Hahn.Data.Dtos.Recipies;
using Hahn.Data.Interfaces.Repositories;
using Hahn.Jobs.Utils;
using Microsoft.Extensions.Logging;

namespace Hahn.Jobs;

/// <summary>
/// Hangfire job that retrieves all recipes.
/// </summary>
public class RecipeGetAllJob
{
    private readonly IRecipeRepository _recipeRepository;
    private readonly ILogger<RecipeGetAllJob> _logger;

    public RecipeGetAllJob(IRecipeRepository recipeRepository, ILogger<RecipeGetAllJob> logger)
    {
        _recipeRepository = recipeRepository;
        _logger = logger;
    }

    public async Task RunAsync(string jobId)
    {
        _logger.LogInformation("Job {JobId}: Fetching all recipes.", jobId);

        try
        {
            var recipes = await _recipeRepository.GetAllAsync();
            var recipeDtos = _recipeRepository.MapToDtos(recipes);

            _logger.LogInformation("Job {JobId}: Retrieved {Count} recipes.", jobId, recipeDtos.Count());

            // Explicitly specify the type parameter
            JobResultStore.SetJobResult<IEnumerable<FoodRecipeDto>>(jobId, recipeDtos);

            _logger.LogInformation("Job {JobId}: Result set successfully.", jobId);
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "Job {JobId}: An error occurred while fetching recipes.", jobId);
            // Optionally, set a default or error result
            JobResultStore.SetJobResult<IEnumerable<FoodRecipeDto>>(jobId, null);
        }
    }
}
