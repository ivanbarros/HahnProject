using Hahn.Infra.Configuration;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// 1. Add services from Application & Infrastructure
builder.Services.AddApplicationServices(); // Registers MediatR, AutoMapper
builder.Services.AddInfrastructureServices(builder.Configuration);

// 2. Add Controllers
builder.Services.AddControllers();

// 3. Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Food Recipe API", Version = "v1" });
});

var app = builder.Build();

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
