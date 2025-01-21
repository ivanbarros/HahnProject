using Hahn.Domain.Entities.BaseEntity;
using Hahn.Domain.Entities.Enums;
using System.Text.Json.Serialization;

namespace Hahn.Domain.Entities;
public class FoodRecipe : CommonEntity
{
    [JsonPropertyName("strMeal")]
    public string Title { get; set; }

    [JsonPropertyName("strInstructions")]
    public string Instructions { get; set; }

    [JsonPropertyName("strIngredient1")]
    public string Ingredients { get; set; }

    private FoodRecipe() { }

    public FoodRecipe(string title, string instructions, string ingredients)
    {
        Title = title;
        Instructions = instructions;
        Ingredients = ingredients;
    }

    public void Update(string title, string instructions, string ingredients)
    {
        Title = title;
        Instructions = instructions;
        Ingredients = ingredients;
    }
}


