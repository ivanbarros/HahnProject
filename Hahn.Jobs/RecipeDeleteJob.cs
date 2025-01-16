using Hahn.Jobs.Utils;
using Hahn.Data.Interfaces.Repositories;
using Hahn.Domain.Entities;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Hahn.Jobs
{
    /// <summary>
    /// Hangfire job that deletes a recipe by ID.
    /// </summary>
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

            _recipeRepository.RemoveAsync(recipe);
            await _recipeRepository.SaveChangesAsync();

            _logger.LogInformation("Recipe with ID {Id} deleted successfully.", id);
            JobResultStore.SetJobResult(jobId, true);
        }
    }
}
