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
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    public static void AddOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddInfrastructureOptions(configuration, new InfrastructureConfigurationSections
        {
            JwtSection = ConfigurationConstants.JwtSection,
            DatabaseSection = ConfigurationConstants.DatabaseSection,
            EmailSection = ConfigurationConstants.EmailSection,
            EmailConfirmationSection = ConfigurationConstants.EmailConfirmationSection
        });

        // Add application-specific options
        services
            .AddOptions<ApplicationUrlOptions>()
            .Bind(configuration.GetSection(ConfigurationConstants.ApplicationSection))
            .ValidateDataAnnotations()
            .ValidateOnStart();
    }
}
