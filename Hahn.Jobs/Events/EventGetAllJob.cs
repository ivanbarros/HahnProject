using Hahn.Data.Dtos.Events;
using Hahn.Data.Dtos.Recipies;
using Hahn.Data.Interfaces.Repositories;
using Hahn.Jobs.Utils;
using Microsoft.Extensions.Logging;

namespace Hahn.Jobs.Events;

public class EventGetAllJob
{
    private readonly ILogger<EventGetAllJob> _logger;
    private readonly IEventsRepository _eventRepository;
    public async Task RunAsync(string jobId)
    {
        _logger.LogInformation("Job {JobId}: Fetching all Recipies.", jobId);

        try
        {
            var events = await _eventRepository.GetAllAsync();
            var recipeDtos = _eventRepository.MapToDtos<EventsDto, Domain.Entities.Events>(events);

            _logger.LogInformation("Job {JobId}: Retrieved {Count} Recipies.", jobId, recipeDtos.Count());

            // Explicitly specify the type parameter
            JobResultStore.SetJobResult(jobId, recipeDtos);

            _logger.LogInformation("Job {JobId}: Result set successfully.", jobId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Job {JobId}: An error occurred while fetching Recipies.", jobId);
            // Optionally, set a default or error result
            JobResultStore.SetJobResult<IEnumerable<EventsDto>>(jobId, null);
        }
    }
}
