using IdentityService.API.Constants;
using IdentityService.Application.Options;
using IdentityService.Infrastructure.Extensions;

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
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The configuration.</param>
    /// <param name="infrastructureSections">The infrastructure sections.</param>
    public static void AddOptions(
        this IServiceCollection services,
        IConfiguration configuration,
        InfrastructureConfigurationSections infrastructureSections)
    {
        services.AddInfrastructureOptions(configuration, infrastructureSections);

        // Add application-specific options
        services
            .AddOptions<ApplicationUrlOptions>()
            .Bind(configuration.GetSection(ConfigurationConstants.ApplicationSection))
            .ValidateDataAnnotations()
            .ValidateOnStart();
    }

    /// <summary>
    /// Convenience method to get options from the configuration.
    /// </summary>
    /// <typeparam name="T">The type of the options.</typeparam>
    /// <param name="configuration">The configuration.</param>
    /// <param name="section">The section of the options.</param>
    /// <returns>The options.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the options are not configured.</exception>
    public static T GetOptions<T>(this IConfiguration configuration, string section) where T : class
    {
        return configuration.GetSection(section).Get<T>()
            ?? throw new InvalidOperationException($"{section} are not configured");
    }
}
