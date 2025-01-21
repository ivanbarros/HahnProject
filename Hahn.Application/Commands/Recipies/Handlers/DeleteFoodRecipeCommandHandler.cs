using AutoMapper;
using Hahn.Data.Interfaces.Repositories;
using Hahn.Domain.Entities;
using MediatR;

namespace Hahn.Application.Commands.Recipies.Handlers
{
    public class DeleteFoodRecipeCommandHandler : IRequestHandler<DeleteFoodRecipeCommand, bool>
    {
        private readonly IRecipeRepository _recipeRepository;
        private readonly IMapper _mapper;

        public DeleteFoodRecipeCommandHandler(IRecipeRepository recipeRepository, IMapper mapper)
        {
            _recipeRepository = recipeRepository;
            _mapper = mapper;
        }

        public async Task<bool> Handle(DeleteFoodRecipeCommand request, CancellationToken cancellationToken)
        {
            // Map the command to Recipe entity
            var recipeEntity = _mapper.Map<FoodRecipe>(request);

            // Call the repository to remove the recipe
            var success = await _recipeRepository.RemoveAsync(recipeEntity);

            return success;
        }
    }
}

