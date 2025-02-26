using Hahn.Jobs.Utils;
using Hahn.Data.Dtos.Recipies;
using Hahn.Data.Interfaces.Repositories;
using Hahn.Domain.Entities;
using Microsoft.Extensions.Logging;
using Hahn.Data.Dtos.Events;

namespace Hahn.Jobs.Events
{
    /// <summary>
    /// Hangfire job that upserts a event (creates or updates).
    /// </summary>
    public class EventUpsertJob
    {
        private readonly IEventsRepository _eventsRepository;
        private readonly ILogger<EventUpsertJob> _logger;

        public EventUpsertJob(IEventsRepository eventsRepository, ILogger<EventUpsertJob> logger)
        {
            _eventsRepository = eventsRepository;
            _logger = logger;
        }

        public async Task RunAsync(Guid? id, string title, string location, DateTime EventDate, string ImgUrl, string jobId, int QntPeople)
        {
            _logger.LogInformation("Attempting to upsert event: {Title}", title);

            EventsDto eventDto;

            if (id.HasValue)
            {
                // Update existing event
                var existingEvent = await _eventsRepository.GetByIdAsync(id.Value);
                if (existingEvent != null)
                {
                    existingEvent.Title = title;
                    existingEvent.Location = location;
                    existingEvent.QntPeople = QntPeople;
                    existingEvent.ImagemUrl = ImgUrl;
                    existingEvent.DateEvent = EventDate;

                    await _eventsRepository.UpdateAsync(existingEvent);
                    await _eventsRepository.SaveChangesAsync();

                    _logger.LogInformation("event '{Title}' updated successfully.", title);
                    eventDto = _eventsRepository.MapToDto<EventsDto>(existingEvent);
                }
                else
                {
                    // If event doesn't exist, create a new one
                    var newevent = new Domain.Entities.Events(location,EventDate,title,QntPeople,ImgUrl);
                    await _eventsRepository.AddAsync(newevent);
                    await _eventsRepository.SaveChangesAsync();

                    _logger.LogInformation("event '{Title}' created successfully.", title);
                    eventDto = _eventsRepository.MapToDto<EventsDto>(newevent);
                }
            }
            else
            {
                // Create new event
                var newevent = new Domain.Entities.Events(location,EventDate,title,QntPeople,ImgUrl);
                await _eventsRepository.AddAsync(newevent);
                await _eventsRepository.SaveChangesAsync();

                _logger.LogInformation("event '{Title}' created successfully.", title);
                eventDto = _eventsRepository.MapToDto<EventsDto>(newevent);
            }

            // Return the upserted event
            JobResultStore.SetJobResult(jobId, eventDto);
        }
    }
}
