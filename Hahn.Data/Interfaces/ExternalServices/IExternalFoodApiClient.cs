﻿using Hahn.Data.Dtos.Recipies;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace Hahn.Data.Interfaces.ExternalServices;

public interface IExternalFoodApiClient
{
    Task<IEnumerable<FoodRecipeDto>> GetLatestRecipiesAsync();
}

public class ExternalFoodApiClient : IExternalFoodApiClient
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;

    public ExternalFoodApiClient(HttpClient httpClient, IConfiguration config)
    {
        _httpClient = httpClient;
        _baseUrl = config.GetConnectionString("ExternalFoodApiBaseUrl");
    }

    public async Task<IEnumerable<FoodRecipeDto>> GetLatestRecipiesAsync()
    {
        var response = await _httpClient.GetAsync($"{_baseUrl}/Recipies");
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<IEnumerable<FoodRecipeDto>>(json) ?? new List<FoodRecipeDto>();
    }
}