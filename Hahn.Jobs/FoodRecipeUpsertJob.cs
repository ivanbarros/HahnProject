using Hahn.Data.Interfaces.ExternalServices;
using Hahn.Data.Interfaces.Repositories;
using Hahn.Jobs.Interfaces;

namespace Hahn.Jobs;

public class FoodRecipeUpsertJob : IFoodRecipeUpsertJob
{
    private readonly IExternalFoodApiClient _externalClient;
    private readonly IFoodRecipeRepository _recipeRepo;

    public FoodRecipeUpsertJob(IExternalFoodApiClient externalClient, IFoodRecipeRepository recipeRepo)
    {
        _externalClient = externalClient;
        _recipeRepo = recipeRepo;
    }

    public async Task RunUpsertAsync()
    {
        var externalRecipes = await _externalClient.GetLatestRecipesAsync();
        if (externalRecipes == null) return;

        var currentRecipes = await _recipeRepo.GetAllAsync();
        var currentTitles = currentRecipes.Select(r => r.Title.ToLower()).ToHashSet();

        foreach (var ext in externalRecipes)
        {
            if (string.IsNullOrWhiteSpace(ext.Title)) continue;

            if (currentTitles.Contains(ext.Title.ToLower()))
            {

                var existing = currentRecipes.FirstOrDefault(r =>
                    r.Title.ToLower() == ext.Title.ToLower());
                existing.Update(ext.Title, ext.Instructions, ext.Ingredients);
            }
            else
            {
                // Insert new
                var newRecipe = new Hahn.Domain.Entities.FoodRecipe(
                    ext.Title, ext.Instructions, ext.Ingredients
                );
                await _recipeRepo.AddAsync(newRecipe);
            }
        }

        await _recipeRepo.SaveChangesAsync();
    }
}