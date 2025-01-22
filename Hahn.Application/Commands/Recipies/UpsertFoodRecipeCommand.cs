using Hahn.Data.Dtos.Recipies;
using MediatR;

namespace Hahn.Application.Commands.Recipies;

public class UpsertFoodRecipeCommand : IRequest<FoodRecipeDto>
{
    public UpsertFoodRecipeDto Dto { get; }

    public UpsertFoodRecipeCommand(UpsertFoodRecipeDto dto)
    {
        Dto = dto;
    }
}

