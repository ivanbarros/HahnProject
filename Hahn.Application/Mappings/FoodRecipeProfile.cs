using AutoMapper;
using Hahn.Data.Dtos.Recipies;
using Hahn.Domain.Entities;

namespace Hahn.Application.Mappings;

public class FoodRecipeProfile : Profile
{
    public FoodRecipeProfile()
    {
        CreateMap<FoodRecipe, FoodRecipeDto>().ReverseMap();
        CreateMap<CreateFoodRecipeDto, FoodRecipe>();
        CreateMap<UpdateFoodRecipeDto, FoodRecipe>();
    }
}
