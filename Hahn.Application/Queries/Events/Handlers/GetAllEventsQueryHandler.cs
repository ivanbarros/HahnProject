using Hahn.Data.Dtos.Events;
using Hahn.Jobs.Events;
using Hahn.Jobs.Utils;
using Hangfire;
using MediatR;

namespace Hahn.Application.Queries.Events.Handlers;

public class GetAllEventsQueryHandler : IRequestHandler<GetAllEventsQuery, IEnumerable<EventsDto>>
{
    private readonly ILogger<GetAllEventsQueryHandler> _logger;
    public GetAllEventsQueryHandler(ILogger<GetAllEventsQueryHandler> logger)
    {
        _logger = logger;
    }
    public async Task<IEnumerable<EventsDto>> Handle(GetAllEventsQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Fetching all Events.");
        var jobId = JobResultStore.RegisterJob();
        BackgroundJob.Enqueue<EventGetAllJob>(job => job.RunAsync(jobId));
        var events = await JobResultStore.GetJobResultAsync<IEnumerable<EventsDto>>(jobId, timeoutSeconds: 30);
        if (events == null)
        {
            throw new TimeoutException("The get all events job timed out.");
        }

        _logger.LogInformation("Retrieved {Count} events.", events.Count());
        return events;


    }
}