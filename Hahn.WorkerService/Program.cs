using Hahn.Infra.Configuration;
using Hahn.Jobs;
using Hahn.Jobs.Interfaces;
using Hangfire;

public class Program
{
    public static void Main(string[] args) =>
        CreateHostBuilder(args).Build().Run();

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                var config = hostContext.Configuration;
                services.AddApplicationServices();
                services.AddInfrastructureServices(config);
                services.AddHangfire(hfConfig =>
                {
                    hfConfig.UseSqlServerStorage(config.GetConnectionString("DefaultConnection"));
                });
                services.AddHangfireServer();
                services.AddScoped<IFoodRecipeUpsertJob, FoodRecipeUpsertJob>();
                services.AddHostedService<Worker>();
            });
}
