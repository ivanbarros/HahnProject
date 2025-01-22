using System.Text.Json.Serialization;

namespace Hahn.Domain.Entities;

public class TheMealDbApiResponse
{
    [JsonPropertyName("meals")]
    public List<Meal> Meals { get; set; }
}

public class Meal
{
    [JsonPropertyName("idMeal")]
    public string IdMeal { get; set; }

    [JsonPropertyName("strMeal")]
    public string StrMeal { get; set; }

    [JsonPropertyName("strInstructions")]
    public string StrInstructions { get; set; }
    [JsonPropertyName("strIngredient1")]
    public string StrIngredient1 { get; set; }

    [JsonPropertyName("strIngredient2")]
    public string StrIngredient2 { get; set; }
    [JsonPropertyName("strIngredient20")]
    public string StrIngredient20 { get; set; }

    [JsonPropertyName("strMeasure1")]
    public string StrMeasure1 { get; set; }

    [JsonPropertyName("strMeasure2")]
    public string StrMeasure2 { get; set; }

    [JsonPropertyName("strMeasure20")]
    public string StrMeasure20 { get; set; }
}