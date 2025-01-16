using AutoMapper;
using Hahn.Data.Context;
using Hahn.Data.Dtos.Recipies;
using Hahn.Data.Interfaces.Repositories;
using Hahn.Data.Repositories.BaseRepository;
using Hahn.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hahn.Data.Repositories;

public class RecipeRepository : GenericRepository<FoodRecipe>, IRecipeRepository
{
    private readonly IMapper _mapper;
    public RecipeRepository(HahnDbContext context, IMapper mapper) : base(context)
    {
        _mapper = mapper;
    }
  
    public async Task<IEnumerable<FoodRecipe>> SearchByTitleAsync(string title)
    {
        return await _dbSet
            .Where(x => x.Title.Contains(title))
            .ToListAsync();
    }

    public FoodRecipeDto MapToDto(FoodRecipe recipe)
    {
        return _mapper.Map<FoodRecipeDto>(recipe);
    }

    public IEnumerable<FoodRecipeDto> MapToDtos(IEnumerable<FoodRecipe> recipes)
    {
        return _mapper.Map<IEnumerable<FoodRecipeDto>>(recipes);
    }
}