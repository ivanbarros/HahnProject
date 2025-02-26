using Hahn.Data.Dtos.Events;
using Hahn.Data.Dtos.Recipies;
using Hahn.Jobs.Events;
using Hahn.Jobs.Recipes;
using Hahn.Jobs.Utils;
using Hangfire;
using MediatR;

namespace Hahn.Application.Commands.Events.Handlers
{
    public class UpsertEventCommandHandler : IRequestHandler<UpsertEventCommand, EventsDto>
    {
        public async Task<EventsDto> Handle(UpsertEventCommand request, CancellationToken cancellationToken)
        {
            var jobId = JobResultStore.RegisterJob();

            BackgroundJob.Enqueue<EventUpsertJob>(
                job => job.RunAsync(
                    request.Dto.Id,
                    request.Dto.Title,
                    request.Dto.Location,
                    request.Dto.EventDate,
                    request.Dto.ImagemUrl,
                    jobId,
                    request.Dto.QntPeople
                    ));

            var recipe = await JobResultStore.GetJobResultAsync<EventsDto>(jobId, timeoutSeconds: 30);
            if (recipe == null)
            {
                throw new TimeoutException("The upsert recipe job timed out.");
            }

            return recipe;
        }
    }
}
