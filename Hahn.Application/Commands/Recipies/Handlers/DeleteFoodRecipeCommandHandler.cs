using AutoMapper;
using Hahn.Data.Dtos.Recipies;
using Hahn.Data.Interfaces.Repositories;
using Hahn.Data.Interfaces.Repositories.BaseRepository;
using Hahn.Domain.Entities;
using Hahn.Jobs;
using Hahn.Jobs.Utils;
using Hangfire;
using MediatR;

namespace Hahn.Application.Commands.Recipies.Handlers;

public class DeleteFoodRecipeCommandHandler : IRequestHandler<DeleteFoodRecipeCommand, bool>
{
   
    public async Task<bool> Handle(DeleteFoodRecipeCommand request, CancellationToken cancellationToken)
    {
        var jobId = JobResultStore.RegisterJob();

        BackgroundJob.Enqueue<RecipeDeleteJob>(
            job => job.RunAsync(
                request.Id,
                jobId));

        var recipe = await JobResultStore.GetJobResultAsync<FoodRecipeDto>(jobId, timeoutSeconds: 30);
       
        return true;
    }
}

