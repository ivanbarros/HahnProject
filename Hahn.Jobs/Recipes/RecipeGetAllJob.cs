using Hahn.Data.Dtos.Recipies;
using Hahn.Data.Interfaces.Repositories;
using Hahn.Domain.Entities;
using Hahn.Jobs.Utils;
using Microsoft.Extensions.Logging;

namespace Hahn.Jobs.Recipes;

/// <summary>
/// Hangfire job that retrieves all Recipies.
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
        _logger.LogInformation("Job {JobId}: Fetching all Recipies.", jobId);

        try
        {
            var Recipies = await _recipeRepository.GetAllAsync();
            var recipeDtos = _recipeRepository.MapToDtos<FoodRecipeDto, FoodRecipies>(Recipies);

            _logger.LogInformation("Job {JobId}: Retrieved {Count} Recipies.", jobId, recipeDtos.Count());

            // Explicitly specify the type parameter
            JobResultStore.SetJobResult(jobId, recipeDtos);

            _logger.LogInformation("Job {JobId}: Result set successfully.", jobId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Job {JobId}: An error occurred while fetching Recipies.", jobId);
            // Optionally, set a default or error result
            JobResultStore.SetJobResult<IEnumerable<FoodRecipeDto>>(jobId, null);
        }
    }
}
