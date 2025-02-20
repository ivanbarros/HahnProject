using Hahn.Data.Dtos.Recipies;
using Hahn.Data.Interfaces.Repositories;
using Hahn.Domain.Entities;
using Hahn.Jobs.Utils;
using Microsoft.Extensions.Logging;

namespace Hahn.Jobs.Recipes
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

        public async Task RunAsync(string title, string ingredients, string instructions, string ImgUrl, string jobId)
        {
            _logger.LogInformation("Attempting to upsert recipe: {Title}", title);

            // Check if recipe with the same title exists
            var existingRecipies = await _recipeRepository.SearchByTitleAsync(title);
            if (existingRecipies.Any())
            {
                _logger.LogInformation("Recipe '{Title}' already exists. Skipping insert.", title);
                // Return the existing recipe
                var existingRecipe = _recipeRepository.MapToDto<FoodRecipeDto>(existingRecipies.First());
                JobResultStore.SetJobResult(jobId, existingRecipe);
                return;
            }

            // Insert new recipe
            var newRecipe = new FoodRecipies(title, ingredients, instructions, ImgUrl);
            await _recipeRepository.AddAsync(newRecipe);
            await _recipeRepository.SaveChangesAsync();

            _logger.LogInformation("Recipe '{Title}' inserted successfully.", title);

            // Map to DTO
            var recipeDto = _recipeRepository.MapToDto<FoodRecipeDto>(newRecipe);

            // Return the new recipe
            JobResultStore.SetJobResult(jobId, recipeDto);
        }
    }
}
