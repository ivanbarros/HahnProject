using Hahn.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Hahn.Infra.Configuration;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(
            this IServiceCollection services,
            IConfiguration configuration)
    {
        
        services.AddDbContext<HahnDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

       
        var allAssemblies = AppDomain.CurrentDomain.GetAssemblies()
            .Where(a => a.FullName is not null && a.FullName.StartsWith("Hahn."))
            .ToArray();

        services.Scan(scan =>
        {
           
            scan.FromAssemblies(allAssemblies)
                .AddClasses(c => c.Where(t => t.Name.EndsWith("Repository")))
                .AsImplementedInterfaces()
                .WithScopedLifetime();

            
            scan.FromAssemblies(allAssemblies)
                .AddClasses(c => c.Where(t => t.Name.EndsWith("ApiClient")))
                .AsImplementedInterfaces()
                .WithScopedLifetime();
        });
       
        services.AddHttpClient();

        return services;
    }
}