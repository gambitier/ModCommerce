using IdentityService.Application.Options;
using IdentityService.API.Constants;
namespace IdentityService.API.Extensions;

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
    }
}
