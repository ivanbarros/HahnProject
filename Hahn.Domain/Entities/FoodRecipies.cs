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

    [JsonPropertyName("imgUrl")]
    public string ImgUrl { get; set; }

    private FoodRecipies() { }

    public FoodRecipies(string title, string ingredients, string instructions, string imgUrl)
    {
        Title = title;
        Ingredients = ingredients;
        Instructions = instructions;
        ImgUrl = imgUrl;
    }

    public void Update(string title, string instructions, string ingredients, string imgUrl)
    {
        Title = title;
        Ingredients = ingredients;
        Instructions = instructions;
        ImgUrl = imgUrl;
    }
}


