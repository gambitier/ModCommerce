using AccountService.Infrastructure.Extensions;
using AccountService.Application.Extensions;
using AccountService.API.Extensions;
using Scalar.AspNetCore;
using AccountService.API.Middleware;
using FluentResults.Extensions.AspNetCore;
using AccountService.API.ErrorHandling;
using Mapster;
using System.Reflection;
using AccountService.Application.Mapping;
using AccountService.API.Mapping;
using AccountService.API.Constants;

var builder = WebApplication.CreateBuilder(args);
//add and validate options at startup
var infraConfigSections = new InfrastructureConfigurationSections
{
    JwtSection = ConfigurationConstants.JwtSection,
    DatabaseSection = ConfigurationConstants.DatabaseSection,
    RabbitMQSection = ConfigurationConstants.RabbitMQSection,
};
builder.Services.AddOptions(builder.Configuration, infraConfigSections);

// Add services to the container.
builder.Services.AddOpenApi();
builder.Services.AddProblemDetails();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<CustomAspNetCoreResultEndpointProfile>();
builder.Services.AddControllers();

builder.Services.AddInfrastructure(builder.Configuration, options =>
{
    options.InfraConfigSections = infraConfigSections;
    options.RepositoryLifetime = ServiceLifetime.Scoped;
});

builder.Services.AddApplication(options =>
{
    options.ServiceLifetime = ServiceLifetime.Scoped;
});

// Register Mapster
builder.Services.AddMapster();
TypeAdapterConfig.GlobalSettings.Scan(
    Assembly.GetExecutingAssembly(),
    typeof(ApplicationMappingConfig).Assembly,
    typeof(ApiMappingConfig).Assembly
);

var app = builder.Build();

AspNetCoreResult.Setup(config =>
    config.DefaultProfile = app.Services.GetRequiredService<CustomAspNetCoreResultEndpointProfile>()
);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.Theme = ScalarTheme.BluePlanet;
    });
}

app.UseHttpsRedirection();
app.UseMiddleware<GlobalExceptionHandlingMiddleware>();
app.UseRouting();

// These two must be in this order, after UseRouting and before MapControllers
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
