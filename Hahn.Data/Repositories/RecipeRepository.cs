using AutoMapper;
using Hahn.Data.Context;
using Hahn.Data.Interfaces.Repositories;
using Hahn.Data.Repositories.BaseRepository;
using Hahn.Domain.Entities;

namespace Hahn.Data.Repositories;

public class RecipeRepository : GenericRepository<FoodRecipies>, IRecipeRepository
{
    //private readonly IMapper _mapper;
    //public RecipeRepository(HahnDbContext context, IMapper mapper) : base(context)
    //{
    //    _mapper = mapper;
    //}

    //public async Task<IEnumerable<FoodRecipies>> SearchByTitleAsync(string title)
    //{
    //    return await _dbSet
    //        .Where(x => x.Title.Contains(title))
    //        .ToListAsync();
    //}

    //public FoodRecipeDto MapToDto(FoodRecipies recipe)
    //{
    //    return _mapper.Map<FoodRecipeDto>(recipe);
    //}

    //public IEnumerable<FoodRecipeDto> MapToDtos(IEnumerable<FoodRecipies> Recipies)
    //{
    //    return _mapper.Map<IEnumerable<FoodRecipeDto>>(Recipies);
    //}

    //public TRecipeDto MapToDto(TRecipe recipe)
    //{
    //    return _mapper.Map<TRecipeDto>(recipe);
    //}
    public RecipeRepository(HahnDbContext context, IMapper mapper) : base(context, mapper)
    {
    }
}