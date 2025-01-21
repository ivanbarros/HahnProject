using Hahn.Infra.Configuration;
using Hangfire;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddHangfire(config =>
{
    config.UseSqlServerStorage(
        builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddHttpClient("TheMealDb", client =>
{
    client.BaseAddress = new Uri("https://www.themealdb.com/api/json/v1/1/");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

builder.Services.AddMemoryCache();

builder.Services.AddHangfireServer();

builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});


// 3. Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Food Recipe API", Version = "v1" });
});

var app = builder.Build();
app.UseHangfireDashboard("/hangfire");
app.UseCors("AllowAll");


// 4. Configure middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Food Recipe API v1");
    });
}

app.MapControllers();

app.Run();
