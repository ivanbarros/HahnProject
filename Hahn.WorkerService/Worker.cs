using Hahn.Jobs.Interfaces;
using Hangfire;

public class Worker : BackgroundService
{
    private readonly IRecurringJobManager _recurringJobManager;

    public Worker(IRecurringJobManager recurringJobManager)
    {
        _recurringJobManager = recurringJobManager;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
       
        _recurringJobManager.AddOrUpdate<IFoodRecipeUpsertJob>(
            "food-recipe-upsert-job",
            job => job.RunUpsertAsync(),
            Cron.Hourly
        );

        return Task.CompletedTask;
    }
}
