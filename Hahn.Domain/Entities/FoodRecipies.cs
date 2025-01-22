using Hahn.Domain.Entities.BaseEntity;
using Hahn.Domain.Entities.Enums;
using System.Text.Json.Serialization;

namespace Hahn.Domain.Entities;
public class FoodRecipies : CommonEntity
{
    [JsonPropertyName("strMeal")]
    public string Title { get; set; }

    [JsonPropertyName("strInstructions")]
    public string Instructions { get; set; }

    [JsonPropertyName("strIngredient1")]
    public string Ingredients { get; set; }

    private FoodRecipies() { }

    public FoodRecipies(string title, string ingredients, string instructions)
    {
        Title = title;
        Ingredients = ingredients;
        Instructions = instructions;
    }

    public void Update(string title, string instructions, string ingredients)
    {
        Title = title;
        Ingredients = ingredients;
        Instructions = instructions;
    }
}


