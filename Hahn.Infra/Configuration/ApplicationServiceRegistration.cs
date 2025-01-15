using Microsoft.Extensions.DependencyInjection;

namespace Hahn.Infra.Configuration;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        
        var allAssemblies = AppDomain.CurrentDomain.GetAssemblies()
            .Where(a => a.FullName is not null && a.FullName.StartsWith("Hahn."))
            .ToArray();
        
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(allAssemblies));
        
        services.AddAutoMapper(allAssemblies);

        services.Scan(scan =>
        {
            scan.FromAssemblies(allAssemblies)
                .AddClasses(c => c.Where(t => t.Name.EndsWith("Service")))
                .AsImplementedInterfaces()
                .WithScopedLifetime();
        });

        return services;
    }
}

