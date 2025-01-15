using Hahn.Data.Context;
using Hahn.Data.Interfaces.ExternalServices;
using Hahn.Data.Interfaces.Repositories;
using Hahn.Data.Repositories;
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

       
        services.AddScoped<IFoodRecipeRepository, FoodRecipeRepository>();

        // (Optional) External API Client
        //services.AddHttpClient();
        services.AddScoped<IExternalFoodApiClient, ExternalFoodApiClient>();

        return services;
    }
}
