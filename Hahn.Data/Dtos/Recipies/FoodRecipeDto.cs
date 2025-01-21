using Hahn.Domain.Entities.BaseEntity;
using System.Text.Json.Serialization;

namespace Hahn.Data.Dtos.Recipies;
public class FoodRecipeDto : CommonEntity
{
    [JsonPropertyName("strMeal")]
    public string Title { get; set; }

    [JsonPropertyName("strInstructions")]
    public string Instructions { get; set; }

    [JsonPropertyName("strIngredient1")]
    public string Ingredients { get; set; }

    [JsonPropertyName("strMeasure1")]
    public string Measure { get; set; }
    public string Description { get; set; }
    public string Cuisine { get; set; }
    public string JobId { get; set; }
}
