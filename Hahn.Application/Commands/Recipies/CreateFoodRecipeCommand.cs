using Hahn.Data.Dtos.Recipies;
using MediatR;

namespace Hahn.Application.Commands.Recipies;

public class CreateFoodRecipeCommand : IRequest<FoodRecipeDto>
{
    public CreateFoodRecipeDto CreateDto { get; set; }
    public CreateFoodRecipeCommand(CreateFoodRecipeDto dto)
    {
        CreateDto = dto;
    }
}
