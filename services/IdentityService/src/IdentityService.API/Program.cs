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
using IdentityService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

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

    using (var scope = app.Services.CreateScope())
    {
        // Create a scope for database operations to ensure proper disposal of the DbContext.
        // 
        // Why we need a scope:
        // - DbContext is registered as a scoped service by default when using AddDbContext()
        // - DbContext maintains database connections and tracks entity changes in memory
        // - Getting services directly from app.Services (root container) creates services
        //   that live for the entire application lifetime
        // - If DbContext lives too long, it can cause:
        //   1. Memory usage will grow as entity changes accumulate i.e. Memory leaks
        //   2. Open database connections not being released
        //   3. Thread safety issues (DbContext isn't thread-safe)
        //   4. Stale data (cached entities don't reflect database changes)
        //
        // The 'using' statement ensures the scope and all its services (including DbContext)
        // are properly disposed after the migration is complete
        var context = scope.ServiceProvider.GetRequiredService<IdentityDbContext>();
        context.Database.Migrate();
    }
}

app.UseHttpsRedirection();
app.UseMiddleware<GlobalExceptionHandlingMiddleware>();
app.UseRouting();

// These two must be in this order, after UseRouting and before MapControllers
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
