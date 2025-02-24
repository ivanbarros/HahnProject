using AutoMapper;
using Hahn.Data.Dtos.Events;
using Hahn.Data.Dtos.Recipies;
using Hahn.Data.Interfaces.Repositories;
using Hahn.Data.Repositories;
using Hahn.Jobs.Events;
using Hahn.Jobs.Recipes;
using Hahn.Jobs.Utils;
using Hangfire;
using MediatR;

namespace Hahn.Application.Queries.Events.Handlers;

public class SearchEventsByTitleQueryHandler : IRequestHandler<SearchEventsByTitleQuery, IEnumerable<EventsDto>>
{
    private readonly IEventsRepository _eventsRepository;
    private readonly IMapper _mapper;    
    private readonly ILogger<SearchEventsByTitleQueryHandler> _logger;

    public SearchEventsByTitleQueryHandler(IEventsRepository eventsRepository, IMapper mapper, ILogger<SearchEventsByTitleQueryHandler> logger)
    {
        _eventsRepository = eventsRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<EventsDto>> Handle(SearchEventsByTitleQuery request, CancellationToken cancellationToken)
    {
        var jobId = JobResultStore.RegisterJob();
        
        BackgroundJob.Enqueue<EventsGetByTitleJob>(
            job => job.RunAsync(
                request.Title,
                jobId));

        var events = await JobResultStore.GetJobResultAsync<IEnumerable<EventsDto>>(jobId, timeoutSeconds: 30);
        if (events == null)
        {
            throw new KeyNotFoundException($"Event with TITLE {request.Title} not found or job timed out.");
        }

        return events;
    }
}
