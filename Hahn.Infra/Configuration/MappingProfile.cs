using AutoMapper;
using Hahn.Data.Dtos.Events;
using Hahn.Data.Dtos.Recipies;
using Hahn.Domain.Entities;

namespace Hahn.Infra.Configuration;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<FoodRecipies, FoodRecipeDto>();

        CreateMap<Events,EventsDto>();

        CreateMap<UpsertFoodRecipeDto, FoodRecipies>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
        
        CreateMap<UpsertEventDto, Events>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
    }
}
