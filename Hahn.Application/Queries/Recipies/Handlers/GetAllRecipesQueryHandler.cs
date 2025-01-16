using AutoMapper;
using Hahn.Jobs.Utils;
using Hahn.Data.Dtos.Recipies;
using Hahn.Jobs;
using Hangfire;
using MediatR;

namespace Hahn.Application.Queries.Recipies.Handlers
{
    public class GetAllRecipesQueryHandler : IRequestHandler<GetAllRecipesQuery, IEnumerable<FoodRecipeDto>>
    {
        private readonly IMapper _mapper;

        public GetAllRecipesQueryHandler(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<IEnumerable<FoodRecipeDto>> Handle(GetAllRecipesQuery request, CancellationToken cancellationToken)
        {
            // Register job and get JobId
            var jobId = JobResultStore.RegisterJob();

            // Enqueue the background job, passing the jobId
            BackgroundJob.Enqueue<RecipeGetAllJob>(
                job => job.RunAsync(jobId));

            // Await the job result
            var recipes = await JobResultStore.GetJobResultAsync<IEnumerable<FoodRecipeDto>>(jobId, timeoutSeconds: 30);
            if (recipes == null)
            {
                throw new TimeoutException("The get all recipes job timed out.");
            }

            return recipes;
        }
    }
}
