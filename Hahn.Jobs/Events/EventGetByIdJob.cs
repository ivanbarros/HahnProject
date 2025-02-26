using Hahn.Data.Dtos.Events;
using Hahn.Data.Interfaces.Repositories;
using Hahn.Jobs.Utils;
using Microsoft.Extensions.Logging;

namespace Hahn.Jobs.Events;

public class EventGetByIdJob
{
    private readonly IEventsRepository _eventsRepository;
    private readonly ILogger<EventGetByIdJob> _logger;

    public EventGetByIdJob(IEventsRepository eventsRepository, ILogger<EventGetByIdJob> logger)
    {
        _eventsRepository = eventsRepository;
        _logger = logger;
    }

    public async Task RunAsync(Guid id, string jobId)
    {
        _logger.LogInformation("Fetching recipe with ID: {Id}", id);

        var events = await _eventsRepository.GetByIdAsync(id);
        var eventsDto = events != null ? _eventsRepository.MapToDto<EventsDto>(events) : null;

        if (eventsDto != null)
        {
            _logger.LogInformation("Fetched event: {Id}", eventsDto.Id);
        }
        else
        {
            _logger.LogWarning("Event with ID {Id} not found.", id);
        }

        JobResultStore.SetJobResult(jobId, eventsDto);
    }
}
