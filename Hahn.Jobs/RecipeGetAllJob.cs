using Hahn.Jobs.Utils;
using Hahn.Data.Interfaces.Repositories;
using Microsoft.Extensions.Logging;

namespace Hahn.Jobs
{
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
            _logger.LogInformation("Fetching all recipes.");

            var recipes = await _recipeRepository.GetAllAsync();
            var recipeDtos = _recipeRepository.MapToDtos(recipes);

            _logger.LogInformation("Fetched {Count} recipes.", recipeDtos.Count());

            JobResultStore.SetJobResult(jobId, recipeDtos);
        }
    }
}
