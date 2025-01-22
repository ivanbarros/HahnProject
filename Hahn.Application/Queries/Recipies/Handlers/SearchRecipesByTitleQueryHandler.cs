using AutoMapper;
using Hahn.Application.Commands.Recipies;
using Hahn.Data.Dtos.Recipies;
using Hahn.Data.Interfaces.Repositories;
using Hahn.Domain.Entities;
using MediatR;
using System.Text.Json;

namespace Hahn.Application.Queries.Recipies.Handlers
{
    public class SearchRecipiesByTitleQueryHandler : IRequestHandler<SearchRecipiesByTitleQuery, IEnumerable<FoodRecipeDto>>
    {
        private readonly IRecipeRepository _recipeRepository;
        private readonly IMapper _mapper;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<SearchRecipiesByTitleQueryHandler> _logger;
        private readonly IMediator _mediator;

        public SearchRecipiesByTitleQueryHandler(
            IRecipeRepository recipeRepository,
            IMapper mapper,
            IHttpClientFactory httpClientFactory,
            ILogger<SearchRecipiesByTitleQueryHandler> logger,
            IMediator mediator)
        {
            _recipeRepository = recipeRepository;
            _mapper = mapper;
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _mediator = mediator;
        }

        public async Task<IEnumerable<FoodRecipeDto>> Handle(SearchRecipiesByTitleQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Searching for Recipies with title: {Title}", request.Title);

            if (string.IsNullOrWhiteSpace(request.Title))
            {
                return Enumerable.Empty<FoodRecipeDto>();
            }

            string trimmedTitle = request.Title.Trim();

            // 1. Search in the local database
            var matchingRecipies = await _recipeRepository.SearchByTitleAsync(trimmedTitle);

            if (matchingRecipies != null && matchingRecipies.Any())
            {
                _logger.LogInformation("Found {Count} recipe(s) locally.", matchingRecipies.Count());

                var localRecipeDtos = _mapper.Map<IEnumerable<FoodRecipeDto>>(matchingRecipies);
                return localRecipeDtos;
            }
               

            var externalRecipeDtos = await SearchExternalApiAsync(trimmedTitle, cancellationToken);

            if (externalRecipeDtos != null && externalRecipeDtos.Any())
            {
                _logger.LogInformation("Found {Count} recipe(s) via external API.", externalRecipeDtos.Count());
                return externalRecipeDtos;
            }

            
            _logger.LogInformation("No Recipies found for title: {Title}", trimmedTitle);
            return Enumerable.Empty<FoodRecipeDto>();
        }

        private async Task<IEnumerable<FoodRecipeDto>> SearchExternalApiAsync(string title, CancellationToken cancellationToken)
        {
            List<FoodRecipeDto> listRecipies = new List<FoodRecipeDto>();
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
               
                var externalRecipeDtos = apiResponse.Meals.Select(meal => new FoodRecipeDto
                {
                    Title = meal.StrMeal,
                    Ingredients = CombineIngredients(meal),
                    Instructions = meal.StrInstructions
                });

                foreach (var item in externalRecipeDtos)
                {
                    var recipe = new FoodRecipeDto {Ingredients = item.Ingredients, Instructions = item.Instructions, Title = item.Title };
                    listRecipies.Add(recipe);
                    var recipeUpsertDto = new UpsertFoodRecipeDto { Ingredients = recipe.Ingredients, Instructions = recipe.Instructions, Title = recipe.Title };
                    var recipeCommand = new UpsertFoodRecipeCommand(recipeUpsertDto);
                    await _mediator.Send(recipeCommand);
                    
                }
                

                return listRecipies;
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
