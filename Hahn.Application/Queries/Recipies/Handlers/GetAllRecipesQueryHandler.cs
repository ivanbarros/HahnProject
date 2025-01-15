using AutoMapper;
using Hahn.Data.Dtos.Recipies;
using Hahn.Data.Interfaces.Repositories;
using MediatR;

namespace Hahn.Application.Queries.Recipies.Handlers;

public class GetAllRecipesQueryHandler : IRequestHandler<GetAllRecipesQuery, IEnumerable<FoodRecipeDto>>
{
    private readonly IFoodRecipeRepository _recipeRepo;
    private readonly IMapper _mapper;

    public GetAllRecipesQueryHandler(IFoodRecipeRepository recipeRepo, IMapper mapper)
    {
        _recipeRepo = recipeRepo;
        _mapper = mapper;
    }

    public async Task<IEnumerable<FoodRecipeDto>> Handle(GetAllRecipesQuery request, CancellationToken cancellationToken)
    {
        var allRecipes = await _recipeRepo.GetAllAsync();
        return _mapper.Map<IEnumerable<FoodRecipeDto>>(allRecipes);
    }
}
