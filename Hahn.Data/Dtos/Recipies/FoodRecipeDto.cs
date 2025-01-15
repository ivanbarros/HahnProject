using Hahn.Domain.Entities.BaseEntity;

namespace Hahn.Data.Dtos.Recipies;
public class FoodRecipeDto : CommonEntity
{    
    public string Title { get; set; }
    public string Description { get; set; }
    public string Cuisine { get; set; }
    public string Instructions { get; set; }
    public string Ingredients { get; set; }
}
