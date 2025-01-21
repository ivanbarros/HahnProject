using AutoMapper;
using Hahn.Data.Dtos.Recipies;
using Hahn.Domain.Entities;

namespace Hahn.Infra.Configuration;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<FoodRecipe, FoodRecipeDto>();

        CreateMap<UpsertFoodRecipeDto, FoodRecipe>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
    }
}
