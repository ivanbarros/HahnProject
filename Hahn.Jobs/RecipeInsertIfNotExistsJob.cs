using Hahn.Data.Interfaces.Repositories;
using Hahn.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Hahn.Jobs;

public class RecipeInsertIfNotExistsJob
{
    private readonly IFoodRecipeRepository _recipeRepository;
    private readonly ILogger<RecipeInsertIfNotExistsJob> _logger;

    public RecipeInsertIfNotExistsJob(
        IFoodRecipeRepository recipeRepository,
        ILogger<RecipeInsertIfNotExistsJob> logger)
    {
        _recipeRepository = recipeRepository;
        _logger = logger;
    }

    public async Task RunAsync(string title, string ingredients, string instructions)
    {
        _logger.LogInformation("Checking if recipe '{Title}' already exists...", title);

       
        var allRecipes = await _recipeRepository.GetAllAsync();
        var existing = allRecipes.FirstOrDefault(r => r.Title == title);

        if (existing != null)
        {
            _logger.LogInformation("Recipe '{Title}' already exists. Skipping insert.", title);
            throw new Exception("Recipe '{Title}' already exists. Skipping insert.\", title");
        }

        var newRecipe = new FoodRecipe(title, ingredients, instructions);
        await _recipeRepository.AddAsync(newRecipe);
        await _recipeRepository.SaveChangesAsync();

        _logger.LogInformation("Recipe '{Title}' inserted successfully!", title);
    }
}
