using Hahn.Data.Context;
using Hahn.Data.Interfaces.Repositories;
using Hahn.Data.Repositories.BaseRepository;
using Hahn.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hahn.Data.Repositories;

public class FoodRecipeRepository : GenericRepository<FoodRecipe>, IFoodRecipeRepository
{
    public FoodRecipeRepository(HahnDbContext context) : base(context)
    {
    }
    public async Task<IEnumerable<FoodRecipe>> SearchByTitleAsync(string title)
    {
        return await _dbSet
            .Where(x => x.Title.Contains(title))
            .ToListAsync();
    }
}