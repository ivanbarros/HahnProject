﻿using Hahn.Data.Dtos.Recipies;
using Hahn.Data.Interfaces.Repositories;
using Hahn.Jobs.Utils;
using Microsoft.Extensions.Logging;

namespace Hahn.Jobs.Recipes;

/// <summary>
/// Hangfire job that retrieves a recipe by ID.
/// </summary>
public class RecipeGetByIdJob
{
    private readonly IRecipeRepository _recipeRepository;
    private readonly ILogger<RecipeGetByIdJob> _logger;

    public RecipeGetByIdJob(IRecipeRepository recipeRepository, ILogger<RecipeGetByIdJob> logger)
    {
        _recipeRepository = recipeRepository;
        _logger = logger;
    }

    public async Task RunAsync(Guid id, string jobId)
    {
        _logger.LogInformation("Fetching recipe with ID: {Id}", id);

        var recipe = await _recipeRepository.GetByIdAsync(id);
        var recipeDto = recipe != null ? _recipeRepository.MapToDto<FoodRecipeDto>(recipe) : null;

        if (recipeDto != null)
        {
            _logger.LogInformation("Fetched recipe: {Id}", recipeDto.Id);
        }
        else
        {
            _logger.LogWarning("Recipe with ID {Id} not found.", id);
        }

        JobResultStore.SetJobResult(jobId, recipeDto);
    }
}
