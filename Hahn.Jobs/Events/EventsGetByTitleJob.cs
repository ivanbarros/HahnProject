using AutoMapper;
using Azure.Core;
using Hahn.Data.Dtos.Events;
using Hahn.Data.Interfaces.Repositories;
using Hahn.Jobs.Utils;
using Microsoft.Extensions.Logging;

namespace Hahn.Jobs.Events;

public class EventsGetByTitleJob
{
    private readonly IEventsRepository _eventsRepository;
    private readonly ILogger<EventsGetByTitleJob> _logger;
    private readonly IMapper _mapper;

    public EventsGetByTitleJob(IEventsRepository eventsRepository, ILogger<EventsGetByTitleJob> logger, IMapper mapper)
    {
        _eventsRepository = eventsRepository;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task RunAsync(string title, string jobId)
    {
        _logger.LogInformation("Searching for Recipies with title: {Title}", title);

        if (string.IsNullOrWhiteSpace(title))
        {
            JobResultStore.SetJobResult<IEnumerable<EventsDto>>(jobId, null);
        }

        string trimmedTitle = title.Trim();

        var matchingRecipies = await _eventsRepository.SearchByTitleAsync(trimmedTitle);

        if (matchingRecipies != null && matchingRecipies.Any())
        {
            _logger.LogInformation("Found {Count} recipe(s) locally.", matchingRecipies.Count());

            var eventsDto = _mapper.Map<IEnumerable<EventsDto>>(matchingRecipies);
            JobResultStore.SetJobResult(jobId, eventsDto);
        }
    }
}
