using AutoMapper;
using Hahn.Data.Context;
using Hahn.Data.Interfaces.Repositories;
using Hahn.Data.Repositories.BaseRepository;
using Hahn.Domain.Entities;

namespace Hahn.Data.Repositories;

public class RecipeRepository : GenericRepository<FoodRecipies>, IRecipeRepository
{
    public RecipeRepository(HahnDbContext context, IMapper mapper) : base(context, mapper)
    {
    }
}