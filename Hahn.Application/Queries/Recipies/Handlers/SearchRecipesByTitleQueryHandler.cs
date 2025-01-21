﻿using AutoMapper;
using Hahn.Data.Dtos.Recipies;
using Hahn.Data.Interfaces.Repositories;
using Hahn.Domain.Entities;
using MediatR;
using System.Text.Json;

namespace Hahn.Application.Queries.Recipies.Handlers
{
    public class SearchRecipesByTitleQueryHandler : IRequestHandler<SearchRecipesByTitleQuery, IEnumerable<FoodRecipeDto>>
    {
        private readonly IRecipeRepository _recipeRepository;
        private readonly IMapper _mapper;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<SearchRecipesByTitleQueryHandler> _logger;

        public SearchRecipesByTitleQueryHandler(IRecipeRepository recipeRepository, IMapper mapper, IHttpClientFactory httpClientFactory, ILogger<SearchRecipesByTitleQueryHandler> logger)
        {
            _recipeRepository = recipeRepository;
            _mapper = mapper;
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<IEnumerable<FoodRecipeDto>> Handle(SearchRecipesByTitleQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Searching for recipes with title: {Title}", request.Title);

            if (string.IsNullOrWhiteSpace(request.Title))
            {
                return Enumerable.Empty<FoodRecipeDto>();
            }

            string trimmedTitle = request.Title.Trim();

            // 1. Search in the local database
            var matchingRecipes = await _recipeRepository.SearchByTitleAsync(trimmedTitle);

            if (matchingRecipes != null && matchingRecipes.Any())
            {
                _logger.LogInformation("Found {Count} recipe(s) locally.", matchingRecipes.Count());

                var localRecipeDtos = _mapper.Map<IEnumerable<FoodRecipeDto>>(matchingRecipes);
                return localRecipeDtos;
            }

            // 2. If not found locally, search the external API
            var externalRecipeDtos = await SearchExternalApiAsync(trimmedTitle, cancellationToken);

            if (externalRecipeDtos != null && externalRecipeDtos.Any())
            {
                _logger.LogInformation("Found {Count} recipe(s) via external API.", externalRecipeDtos.Count());

                // Optionally, save the external recipes to the local database
                foreach (var recipeDto in externalRecipeDtos)
                {
                    // Convert DTO back to Entity for persistence
                    var recipeEntity = new FoodRecipe(recipeDto.Title, recipeDto.Ingredients, recipeDto.Instructions);

                    await _recipeRepository.AddAsync(recipeEntity);
                }

                return externalRecipeDtos;
            }

            // 3. If not found externally, return empty
            _logger.LogInformation("No recipes found for title: {Title}", trimmedTitle);
            return Enumerable.Empty<FoodRecipeDto>();
        }

        private async Task<IEnumerable<FoodRecipeDto>> SearchExternalApiAsync(string title, CancellationToken cancellationToken)
        {
            var client = _httpClientFactory.CreateClient("TheMealDb");
            string requestUri = $"search.php?s={Uri.EscapeDataString(title)}";

            try
            {
                var response = await client.GetAsync(requestUri, cancellationToken);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("External API call failed with status code: {StatusCode}", response.StatusCode);
                    return Enumerable.Empty<FoodRecipeDto>();
                }

                var contentStream = await response.Content.ReadAsStreamAsync(cancellationToken);
                var apiResponse = await JsonSerializer.DeserializeAsync<TheMealDbApiResponse>(contentStream, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }, cancellationToken);

                if (apiResponse?.Meals == null || !apiResponse.Meals.Any())
                {
                    _logger.LogInformation("External API returned no meals for title: {Title}", title);
                    return Enumerable.Empty<FoodRecipeDto>();
                }

                // Map external API meals to FoodRecipeDto
                var externalRecipeDtos = apiResponse.Meals.Select(meal => new FoodRecipeDto
                {
                    Title = meal.StrMeal,
                    Ingredients = CombineIngredients(meal),
                    Instructions = meal.StrInstructions
                });

                return externalRecipeDtos;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP request error occurred while calling external API.");
                return Enumerable.Empty<FoodRecipeDto>();
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "JSON deserialization error occurred while processing external API response.");
                return Enumerable.Empty<FoodRecipeDto>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while searching external API.");
                return Enumerable.Empty<FoodRecipeDto>();
            }
        }

        /// <summary>
        /// Combines the ingredients and measures from the external API response into a single string.
        /// </summary>
        /// <param name="meal">The meal object from the external API.</param>
        /// <returns>A formatted string of ingredients and measures.</returns>
        private string CombineIngredients(Meal meal)
        {
            var ingredients = new List<string>();

            for (int i = 1; i <= 20; i++)
            {
                var ingredientProperty = meal.GetType().GetProperty($"StrIngredient{i}");
                var measureProperty = meal.GetType().GetProperty($"StrMeasure{i}");

                var ingredient = ingredientProperty?.GetValue(meal)?.ToString();
                var measure = measureProperty?.GetValue(meal)?.ToString();

                if (!string.IsNullOrWhiteSpace(ingredient))
                {
                    string formattedIngredient = !string.IsNullOrWhiteSpace(measure)
                        ? $"{measure.Trim()} {ingredient.Trim()}"
                        : ingredient.Trim();

                    ingredients.Add(formattedIngredient);
                }
            }

            return string.Join(", ", ingredients);
        }
    }
}
