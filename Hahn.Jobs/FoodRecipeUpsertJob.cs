using Hahn.Data.Interfaces.ExternalServices;
using Hahn.Data.Interfaces.Repositories;
using Hahn.Domain.Entities;
using Hahn.Jobs.Interfaces;

namespace Hahn.Jobs;

public class FoodRecipeUpsertJob : IFoodRecipeUpsertJob
{
    private readonly IExternalFoodApiClient _externalClient;
    private readonly IRecipeRepository _recipeRepo;

    public FoodRecipeUpsertJob(IExternalFoodApiClient externalClient, IRecipeRepository recipeRepo)
    {
        _externalClient = externalClient;
        _recipeRepo = recipeRepo;
    }

    public async Task RunUpsertAsync()
    {
        var externalRecipies = await _externalClient.GetLatestRecipiesAsync();
        if (externalRecipies == null) return;

        var currentRecipies = await _recipeRepo.GetAllAsync();
        var currentTitles = currentRecipies.Select(r => r.Title.ToLower()).ToHashSet();

        foreach (var ext in externalRecipies)
        {
            if (string.IsNullOrWhiteSpace(ext.Title)) continue;

            if (currentTitles.Contains(ext.Title.ToLower()))
            {

                var existing = currentRecipies.FirstOrDefault(r =>
                    r.Title.ToLower() == ext.Title.ToLower());
                existing.Update(ext.Title, ext.Instructions, ext.Ingredients);
            }
            else
            {
                // Insert new
                var newRecipe = new FoodRecipies(
                    ext.Title, ext.Instructions, ext.Ingredients
                );
                await _recipeRepo.AddAsync(newRecipe);
            }
        }

        await _recipeRepo.SaveChangesAsync();
    }
}