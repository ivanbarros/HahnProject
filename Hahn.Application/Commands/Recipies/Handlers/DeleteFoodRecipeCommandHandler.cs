using Hahn.Application.Commands.Recipies;
using Hahn.Jobs.Utils;
using Hahn.Data.Interfaces.Repositories;
using Hahn.Jobs;
using Hangfire;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Hahn.Application.Commands.Recipies.Handlers
{
    public class DeleteFoodRecipeCommandHandler : IRequestHandler<DeleteFoodRecipeCommand, Unit>
    {
        public async Task<Unit> Handle(DeleteFoodRecipeCommand request, CancellationToken cancellationToken)
        {
            // Register job and get JobId
            var jobId = JobResultStore.RegisterJob();

            // Enqueue the background job, passing the jobId
            BackgroundJob.Enqueue<RecipeDeleteJob>(
                job => job.RunAsync(
                    request.Id,
                    jobId));

            // Await the job result
            var deleteResult = await JobResultStore.GetJobResultAsync<bool>(jobId, timeoutSeconds: 30);
            if (!deleteResult)
            {
                throw new Exception($"Failed to delete recipe with ID {request.Id}.");
            }

            return Unit.Value;
        }
    }
}
