using UserService.Infrastructure.Extensions;

namespace UserService.API.Extensions;

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
    /// <param name="infraConfigSections"></param>
    public static void AddOptions(
        this IServiceCollection services,
        IConfiguration configuration,
        InfrastructureConfigurationSections infraConfigSections)
    {
        services.AddInfrastructureOptions(configuration, infraConfigSections);
    }
}
