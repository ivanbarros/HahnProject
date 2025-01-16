using Hahn.Data.Dtos.Recipies;
using Hahn.Data.Interfaces.Repositories;
using Hahn.Domain.Entities;
using Hahn.Jobs.Utils;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace Hahn.Jobs
{
    /// <summary>
    /// Hangfire job that inserts a recipe if it does not already exist.
    /// </summary>
    public class RecipeInsertIfNotExistsJob
    {
        private readonly IRecipeRepository _recipeRepository;
        private readonly ILogger<RecipeInsertIfNotExistsJob> _logger;

        public RecipeInsertIfNotExistsJob(IRecipeRepository recipeRepository, ILogger<RecipeInsertIfNotExistsJob> logger)
        {
            _recipeRepository = recipeRepository;
            _logger = logger;
        }

        public async Task RunAsync(string title, string ingredients, string instructions, string jobId)
        {
            _logger.LogInformation("Attempting to upsert recipe: {Title}", title);

            // Check if recipe with the same title exists
            var existingRecipes = await _recipeRepository.SearchByTitleAsync(title);
            if (existingRecipes.Any())
            {
                _logger.LogInformation("Recipe '{Title}' already exists. Skipping insert.", title);
                // Return the existing recipe
                var existingRecipe = _recipeRepository.MapToDto(existingRecipes.First());
                JobResultStore.SetJobResult(jobId, existingRecipe);
                return;
            }

            // Insert new recipe
            var newRecipe = new FoodRecipe(title, ingredients, instructions);
            await _recipeRepository.AddAsync(newRecipe);
            await _recipeRepository.SaveChangesAsync();

            _logger.LogInformation("Recipe '{Title}' inserted successfully.", title);

            // Map to DTO
            var recipeDto = _recipeRepository.MapToDto(newRecipe);

            // Return the new recipe
            JobResultStore.SetJobResult(jobId, recipeDto);
        }
    }
}
