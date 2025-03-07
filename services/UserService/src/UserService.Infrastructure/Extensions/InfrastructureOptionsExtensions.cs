
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using UserService.Infrastructure.Authentication.Options;
namespace UserService.Infrastructure.Extensions;

public record InfrastructureConfigurationSections
{
    public required string JwtSection { get; init; }
}

public static class InfrastructureOptionsExtensions
{
    private static IServiceCollection ConfigureOptions<TOptions>(
        this IServiceCollection services,
        IConfiguration configuration,
        string sectionName,
        Action<OptionsBuilder<TOptions>>? additionalConfiguration = null)
        where TOptions : class
    {
        var builder = services
            .AddOptions<TOptions>()
            .Bind(configuration.GetSection(sectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        additionalConfiguration?.Invoke(builder);

        return services;
    }

    public static IServiceCollection AddInfrastructureOptions(
        this IServiceCollection services,
        IConfiguration configuration,
        InfrastructureConfigurationSections sections)
    {
        services.ConfigureOptions<JwtOptions>(
            configuration,
            sections.JwtSection);

        return services;
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
