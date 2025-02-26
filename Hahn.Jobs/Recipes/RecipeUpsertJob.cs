﻿using Hahn.Jobs.Utils;
using Hahn.Data.Dtos.Recipies;
using Hahn.Data.Interfaces.Repositories;
using Hahn.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Hahn.Jobs.Recipes
{
    /// <summary>
    /// Hangfire job that upserts a recipe (creates or updates).
    /// </summary>
    public class RecipeUpsertJob
    {
        private readonly IRecipeRepository _recipeRepository;
        private readonly ILogger<RecipeUpsertJob> _logger;

        public RecipeUpsertJob(IRecipeRepository recipeRepository, ILogger<RecipeUpsertJob> logger)
        {
            _recipeRepository = recipeRepository;
            _logger = logger;
        }

        public async Task RunAsync(Guid? id, string title, string ingredients, string instructions, string ImgUrl, string jobId)
        {
            _logger.LogInformation("Attempting to upsert recipe: {Title}", title);

            FoodRecipeDto recipeDto;

            if (id.HasValue)
            {
                // Update existing recipe
                var existingRecipe = await _recipeRepository.GetByIdAsync(id.Value);
                if (existingRecipe != null)
                {
                    existingRecipe.Title = title;
                    existingRecipe.Ingredients = ingredients;
                    existingRecipe.Instructions = instructions;
                    existingRecipe.ImgUrl = ImgUrl;
                    await _recipeRepository.UpdateAsync(existingRecipe);
                    await _recipeRepository.SaveChangesAsync();

                    _logger.LogInformation("Recipe '{Title}' updated successfully.", title);
                    recipeDto = _recipeRepository.MapToDto<FoodRecipeDto>(existingRecipe);
                }
                else
                {
                    // If recipe doesn't exist, create a new one
                    var newRecipe = new FoodRecipies(title, ingredients, instructions, ImgUrl);
                    await _recipeRepository.AddAsync(newRecipe);
                    await _recipeRepository.SaveChangesAsync();

                    _logger.LogInformation("Recipe '{Title}' created successfully.", title);
                    recipeDto = _recipeRepository.MapToDto<FoodRecipeDto>(newRecipe);
                }
            }
            else
            {
                // Create new recipe
                var newRecipe = new FoodRecipies(title, ingredients, instructions, ImgUrl);
                await _recipeRepository.AddAsync(newRecipe);
                await _recipeRepository.SaveChangesAsync();

                _logger.LogInformation("Recipe '{Title}' created successfully.", title);
                recipeDto = _recipeRepository.MapToDto<FoodRecipeDto>(newRecipe);
            }

            // Return the upserted recipe
            JobResultStore.SetJobResult(jobId, recipeDto);
        }
    }
}
