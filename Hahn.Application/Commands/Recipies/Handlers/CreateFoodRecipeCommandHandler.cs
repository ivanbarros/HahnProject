using AutoMapper;
using Hahn.Data.Dtos.Recipies;
using Hahn.Data.Interfaces.Repositories;
using Hahn.Domain.Entities;
using Hahn.Jobs;
using Hangfire;
using MediatR;

namespace Hahn.Application.Commands.Recipies.Handlers;

public class CreateFoodRecipeCommandHandler : IRequestHandler<CreateFoodRecipeCommand, FoodRecipeDto>
{   
    private readonly IMapper _mapper;

    public CreateFoodRecipeCommandHandler(IMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task<FoodRecipeDto> Handle(CreateFoodRecipeCommand request, CancellationToken cancellationToken)
    {
        var entity = new FoodRecipe(
            request.CreateDto.Title,
            request.CreateDto.Instructions,
            request.CreateDto.Ingredients
        );

        var worker = BackgroundJob.Enqueue<RecipeInsertIfNotExistsJob>(
            job => job.RunAsync(
                request.CreateDto.Title,
                request.CreateDto.Ingredients,
                request.CreateDto.Instructions));

        return _mapper.Map<FoodRecipeDto>(entity);
    }
}


