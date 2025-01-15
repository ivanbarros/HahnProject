using Hahn.Domain.Entities.BaseEntity;
using Hahn.Domain.Entities.Enums;

namespace Hahn.Domain.Entities;
public class FoodRecipe : CommonEntity
{
    public string Title { get; private set; }
    public string Instructions { get; private set; }
    public string Ingredients { get; private set; }

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


