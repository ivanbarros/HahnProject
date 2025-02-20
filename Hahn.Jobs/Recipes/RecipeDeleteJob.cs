using Hahn.Data.Interfaces.Repositories;
using Hahn.Jobs.Utils;
using Microsoft.Extensions.Logging;

namespace Hahn.Jobs.Recipes
{

    public class RecipeDeleteJob
    {
        private readonly IRecipeRepository _recipeRepository;
        private readonly ILogger<RecipeDeleteJob> _logger;

        public RecipeDeleteJob(IRecipeRepository recipeRepository, ILogger<RecipeDeleteJob> logger)
        {
            _recipeRepository = recipeRepository;
            _logger = logger;
        }

        public async Task RunAsync(Guid id, string jobId)
        {
            _logger.LogInformation("Attempting to delete recipe with ID: {Id}", id);

            var recipe = await _recipeRepository.GetByIdAsync(id);
            if (recipe == null)
            {
                _logger.LogWarning("Recipe with ID {Id} does not exist. Skipping deletion.", id);
                JobResultStore.SetJobResult(jobId, false);
                return;
            }

            await _recipeRepository.RemoveAsync(recipe);
            await _recipeRepository.SaveChangesAsync();

            _logger.LogInformation("Recipe with ID {Id} deleted successfully.", id);
            JobResultStore.SetJobResult(jobId, true);
        }
    }
}
