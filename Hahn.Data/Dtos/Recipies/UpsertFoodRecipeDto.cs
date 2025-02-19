using Hahn.Domain.Entities.BaseEntity;

namespace Hahn.Data.Dtos.Recipies;

public class UpsertFoodRecipeDto : CommonEntity
{
    public string Title { get; set; }
    public string Instructions { get; set; }
    public string Ingredients { get; set; }
    public string ImgUrl { get; set; }
}
