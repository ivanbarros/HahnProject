using Hahn.Data.Interfaces.Repositories.BaseRepository;
using Hahn.Domain.Entities;

namespace Hahn.Data.Interfaces.Repositories;

public interface IFoodRecipeRepository : IGenericRepository<FoodRecipe>
{
    Task<IEnumerable<FoodRecipe>> SearchByTitleAsync(string title);
}
