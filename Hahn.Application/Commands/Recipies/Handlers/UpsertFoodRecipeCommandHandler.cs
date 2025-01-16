using AutoMapper;
using Hahn.Jobs.Utils;
using Hahn.Data.Dtos.Recipies;
using Hahn.Data.Interfaces.Repositories;
using Hahn.Jobs;
using Hangfire;
using MediatR;

namespace Hahn.Application.Commands.Recipies.Handlers
{
    public class UpsertFoodRecipeCommandHandler : IRequestHandler<UpsertFoodRecipeCommand, FoodRecipeDto>
    {
        private readonly IMapper _mapper;
        private readonly IRecipeRepository _recipeRepository;

        public UpsertFoodRecipeCommandHandler(IMapper mapper, IRecipeRepository recipeRepository)
        {
            _mapper = mapper;
            _recipeRepository = recipeRepository;
        }

        public async Task<FoodRecipeDto> Handle(UpsertFoodRecipeCommand request, CancellationToken cancellationToken)
        {
            // Register job and get JobId
            var jobId = JobResultStore.RegisterJob();

            // Enqueue the background job, passing the jobId
            BackgroundJob.Enqueue<RecipeUpsertJob>(
                job => job.RunAsync(
                    request.Dto.Id,
                    request.Dto.Title,
                    request.Dto.Ingredients,
                    request.Dto.Instructions,
                    jobId));

            // Await the job result
            var recipe = await JobResultStore.GetJobResultAsync<FoodRecipeDto>(jobId, timeoutSeconds: 30);
            if (recipe == null)
            {
                throw new TimeoutException("The upsert recipe job timed out.");
            }

            return recipe;
        }
    }
}
