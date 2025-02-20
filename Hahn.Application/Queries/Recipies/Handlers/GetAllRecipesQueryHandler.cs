using Hahn.Data.Dtos.Recipies;
using Hahn.Jobs.Recipes;
using Hahn.Jobs.Utils;
using Hangfire;
using MediatR;

namespace Hahn.Application.Queries.Recipies.Handlers;

public class GetAllRecipiesQueryHandler : IRequestHandler<GetAllRecipiesQuery, IEnumerable<FoodRecipeDto>>
{

    private readonly ILogger<GetAllRecipiesQueryHandler> _logger;

    public GetAllRecipiesQueryHandler(ILogger<GetAllRecipiesQueryHandler> logger)
    {
        _logger = logger;
    }

    public async Task<IEnumerable<FoodRecipeDto>> Handle(GetAllRecipiesQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Fetching all Recipies.");
        var jobId = JobResultStore.RegisterJob();

        // Enqueue the concrete job class
        BackgroundJob.Enqueue<RecipeGetAllJob>(job => job.RunAsync(jobId));

        var recipies = await JobResultStore.GetJobResultAsync<IEnumerable<FoodRecipeDto>>(jobId, timeoutSeconds: 30);
        if (recipies == null)
        {
            throw new TimeoutException("The get all Recipies job timed out.");
        }

        _logger.LogInformation("Retrieved {Count} Recipies.", recipies.Count());
        return recipies;
    }
}
