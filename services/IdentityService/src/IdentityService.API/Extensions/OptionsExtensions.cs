using IdentityService.Infrastructure.Persistence.Options;
using IdentityService.API.Constants;
using IdentityService.Infrastructure.Authentication.Options;

namespace IdentityService.API.Extensions;

/// <summary>
/// Extension methods for adding options to the service collection.
/// This class centralizes all options configuration and retrieval for the application,
/// ensuring consistent access to configuration settings.
/// </summary>
public static class OptionsExtensions
{
    /// <summary>
    /// Add options to the service collection.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    public static void AddOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddOptions<JwtOptions>()
            .Bind(configuration.GetSection(ConfigurationConstants.JwtSection))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services
            .AddOptions<DatabaseOptions>()
            .Bind(configuration.GetSection(ConfigurationConstants.DatabaseSection))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services
            .AddOptions<RateLimitOptions>()
            .Bind(configuration.GetSection(ConfigurationConstants.RateLimitSection))
            .ValidateDataAnnotations()
            .ValidateOnStart();
    }

    public static JwtOptions GetJwtOptions(this IConfiguration configuration)
    {
        return configuration.GetSection(ConfigurationConstants.JwtSection).Get<JwtOptions>()
            ?? throw new InvalidOperationException("JWT options are not configured");
    }

    public static DatabaseOptions GetDatabaseOptions(this IConfiguration configuration)
    {
        return configuration.GetSection(ConfigurationConstants.DatabaseSection).Get<DatabaseOptions>()
            ?? throw new InvalidOperationException("Database options are not configured");
    }

    public static RateLimitOptions GetRateLimitOptions(this IConfiguration configuration)
    {
        return configuration.GetSection(ConfigurationConstants.RateLimitSection).Get<RateLimitOptions>()
            ?? throw new InvalidOperationException("Rate limit options are not configured");
    }
}
