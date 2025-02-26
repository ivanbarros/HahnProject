using Hahn.Data.Dtos.Events;
using Hahn.Data.Interfaces.Repositories;
using Hahn.Jobs.Utils;
using Microsoft.Extensions.Logging;

namespace Hahn.Jobs.Events;

public class EventGetAllJob
{
    private readonly ILogger<EventGetAllJob> _logger;
    private readonly IEventsRepository _eventRepository;

    public EventGetAllJob(ILogger<EventGetAllJob> logger, IEventsRepository eventRepository)
    {
        _logger = logger;
        _eventRepository = eventRepository;
    }

    public async Task RunAsync(string jobId)
    {
        _logger.LogInformation("Job {JobId}: Fetching all Events.", jobId);

        try
        {
            var events = await _eventRepository.GetAllAsync();
            var eventsDto = _eventRepository.MapToDtos<EventsDto, Domain.Entities.Events>(events);

            _logger.LogInformation("Job {JobId}: Retrieved {Count} Events.", jobId, eventsDto.Count());
            
            JobResultStore.SetJobResult(jobId, eventsDto);

            _logger.LogInformation("Job {JobId}: Result set successfully.", jobId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Job {JobId}: An error occurred while fetching events.", jobId);

            JobResultStore.SetJobResult<IEnumerable<EventsDto>>(jobId, null);
        }
    }
}
