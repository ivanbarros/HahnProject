// Program.cs
using Hahn.Application.Commands.Recipies.Handlers;
using Hahn.Data.Context;
using Hahn.Data.Interfaces.Repositories;
using Hahn.Data.Repositories;
using Hahn.Infra.Configuration;
using Hahn.Jobs;
using Hahn.WorkerService.HangFireConfig;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

var builder = Host.CreateDefaultBuilder(args)
    .ConfigureWebHostDefaults(webBuilder =>
    {
        webBuilder.ConfigureServices((context, services) =>
        {
            var configuration = context.Configuration;

            services.AddDbContext<HahnDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            services.AddScoped<IRecipeRepository, RecipeRepository>();
            services.AddAutoMapper(typeof(MappingProfile));
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(UpsertFoodRecipeCommandHandler).Assembly));

            services.AddHangfireServer();
            services.AddHangfire(config =>
            {
                config.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                      .UseSimpleAssemblyNameTypeSerializer()
                      .UseRecommendedSerializerSettings()
                      .UseSqlServerStorage(configuration.GetConnectionString("DefaultConnection"), new SqlServerStorageOptions
                      {
                          CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                          SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                          QueuePollInterval = TimeSpan.Zero,
                          UseRecommendedIsolationLevel = true,
                          DisableGlobalLocks = true
                      });
            });

            services.AddHangfireServer();

           
            services.AddTransient<RecipeInsertIfNotExistsJob>();
            services.AddTransient<RecipeUpsertJob>();
            services.AddTransient<RecipeDeleteJob>();
            services.AddTransient<RecipeGetAllJob>();
            services.AddTransient<RecipeGetByIdJob>();

            services.AddControllers();
        });

        webBuilder.Configure(app =>
        {
            var env = app.ApplicationServices.GetRequiredService<IHostEnvironment>();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            
            app.UseAuthentication();
            app.UseAuthorization();

            
            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                Authorization = new[] { new HangfireAuthorizationFilter() }
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHangfireDashboard();
            });
        });
    });

var host = builder.Build();


host.Run();
