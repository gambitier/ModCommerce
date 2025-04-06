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
using AccountService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

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
        var context = scope.ServiceProvider.GetRequiredService<AccountDbContext>();
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
