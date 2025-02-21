using Hahn.Data.Dtos.Recipies;
using Hahn.Data.Interfaces.Repositories.BaseRepository;
using Hahn.Domain.Entities;

namespace Hahn.Data.Interfaces.Repositories;

public interface IRecipeRepository : IGenericRepository<FoodRecipies>
{
}
