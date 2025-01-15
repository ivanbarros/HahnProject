using AutoMapper;
using Hahn.Data.Dtos.Recipies;
using Hahn.Data.Interfaces.Repositories;
using Hahn.Domain.Entities;
using MediatR;

namespace Hahn.Application.Commands.Recipies.Handlers;

public class CreateFoodRecipeCommandHandler : IRequestHandler<CreateFoodRecipeCommand, FoodRecipeDto>
{
    private readonly IFoodRecipeRepository _recipeRepo;
    private readonly IMapper _mapper;

    public CreateFoodRecipeCommandHandler(IFoodRecipeRepository recipeRepo, IMapper mapper)
    {
        _recipeRepo = recipeRepo;
        _mapper = mapper;
    }

    public async Task<FoodRecipeDto> Handle(CreateFoodRecipeCommand request, CancellationToken cancellationToken)
    {
        var entity = new FoodRecipe(
            request.CreateDto.Title,
            request.CreateDto.Instructions,
            request.CreateDto.Ingredients
        );

        await _recipeRepo.AddAsync(entity);
        await _recipeRepo.SaveChangesAsync();

        return _mapper.Map<FoodRecipeDto>(entity);
    }
}


