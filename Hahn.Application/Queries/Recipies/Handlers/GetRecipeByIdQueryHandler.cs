using Hahn.Data.Dtos.Recipies;
using Hahn.Jobs;
using Hahn.Jobs.Utils;
using Hangfire;
using MediatR;

namespace Hahn.Application.Queries.Recipies.Handlers
{
    public class GetRecipeByIdQueryHandler : IRequestHandler<GetRecipeByIdQuery, FoodRecipeDto>
    {
        public async Task<FoodRecipeDto> Handle(GetRecipeByIdQuery request, CancellationToken cancellationToken)
        {
            // Register job and get JobId
            var jobId = JobResultStore.RegisterJob();

            // Enqueue the background job, passing the jobId
            BackgroundJob.Enqueue<RecipeGetByIdJob>(
                job => job.RunAsync(
                    request.Id,
                    jobId));

            // Await the job result
            var recipe = await JobResultStore.GetJobResultAsync<FoodRecipeDto>(jobId, timeoutSeconds: 30);
            if (recipe == null)
            {
                throw new KeyNotFoundException($"Recipe with ID {request.Id} not found or job timed out.");
            }

            return recipe;
        }
    }
}
