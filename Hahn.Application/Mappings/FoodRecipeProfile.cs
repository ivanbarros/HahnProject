using AutoMapper;
using Hahn.Data.Dtos.Recipies;
using Hahn.Domain.Entities;

namespace Hahn.Application.Mappings;

public class FoodRecipeProfile : Profile
{
    public FoodRecipeProfile()
    {
        CreateMap<FoodRecipies, FoodRecipeDto>().ReverseMap();
        CreateMap<UpsertFoodRecipeDto, FoodRecipies>();
        CreateMap<UpdateFoodRecipeDto, FoodRecipies>();
    }
}
