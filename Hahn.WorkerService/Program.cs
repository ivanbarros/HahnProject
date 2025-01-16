using Hahn.Infra.Configuration;
using Hahn.Jobs;
using Hangfire;

var builder = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        var configuration = hostContext.Configuration;
        services.AddApplicationServices();
        services.AddInfrastructureServices(configuration);        
        services.AddHttpClient();

        services.AddHangfire(config =>
        {
            config.UseSqlServerStorage(configuration.GetConnectionString("DefaultConnection"));
        });
        services.AddHangfireServer();
        services.AddTransient<RecipeInsertIfNotExistsJob>();
        services.AddHostedService<Worker>();
    });

await builder.RunConsoleAsync();
