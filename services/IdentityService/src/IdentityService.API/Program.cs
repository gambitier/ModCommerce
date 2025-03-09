using IdentityService.Infrastructure.Extensions;
using IdentityService.Application.Extensions;
using IdentityService.API.Extensions;
using Scalar.AspNetCore;
using IdentityService.API.Middleware;
using FluentResults.Extensions.AspNetCore;
using IdentityService.API.ErrorHandling;
using Mapster;
using System.Reflection;
using IdentityService.Application.Mapping;
using IdentityService.API.Mapping;
using IdentityService.API.Constants;

var builder = WebApplication.CreateBuilder(args);
//add and validate options at startup
var infraConfigSections = new InfrastructureConfigurationSections
{
    JwtSection = ConfigurationConstants.JwtSection,
    DatabaseSection = ConfigurationConstants.DatabaseSection,
    EmailSection = ConfigurationConstants.EmailSection,
    EmailConfirmationSection = ConfigurationConstants.EmailConfirmationSection,
    RabbitMQSection = ConfigurationConstants.RabbitMQSection
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
    options.RepositoryLifetime = ServiceLifetime.Scoped;
    options.AuthenticationServicesLifetime = ServiceLifetime.Scoped;
    options.InfraConfigSections = infraConfigSections;
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
