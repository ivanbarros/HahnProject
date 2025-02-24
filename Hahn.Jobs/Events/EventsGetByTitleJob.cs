using Hahn.Data.Dtos.Events;
using Hahn.Data.Interfaces.Repositories;
using Hahn.Jobs.Utils;
using Microsoft.Extensions.Logging;

namespace Hahn.Jobs.Events;

public class EventsGetByTitleJob
{
    private readonly IEventsRepository _eventsRepository;
    private readonly ILogger<EventsGetByTitleJob> _logger;

    public EventsGetByTitleJob(IEventsRepository eventsRepository, ILogger<EventsGetByTitleJob> logger)
    {
        _eventsRepository = eventsRepository;
        _logger = logger;
    }

    public async Task RunAsync(string title, string jobId)
    {
        _logger.LogInformation("Fetching recipe with ID: {Id}", title);

        var events = await _eventsRepository.SearchByTitleAsync(title);
        var eventsDto = events != null ? _eventsRepository.MapToDtos<EventsDto, Domain.Entities.Events>(events) : null;

        if (eventsDto != null)
        {
            _logger.LogInformation("Fetched event: {Id}", eventsDto.Count());
        }
        else
        {
            _logger.LogWarning("Event with ID {Id} not found.", title);
        }

        JobResultStore.SetJobResult(jobId, eventsDto);
    }
}
