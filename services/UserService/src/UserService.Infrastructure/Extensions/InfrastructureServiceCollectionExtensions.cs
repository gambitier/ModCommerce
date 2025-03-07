using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.DependencyInjection;
using UserService.Infrastructure.Authentication.Options;
using Microsoft.Extensions.Configuration;
using UserService.Infrastructure.Authentication.Services;

namespace UserService.Infrastructure.Extensions;


/// <summary>
/// Options for the infrastructure services.
/// </summary>
public class InfrastructureOptions
{
    /// <summary>
    /// Configuration sections defined in appsettings.json for the infrastructure services.
    /// - The null! annotation indicates this property must be set during initialization,
    /// even though it's initially null. This is validated using the Required attribute.
    /// </summary>
    [Required(ErrorMessage = "Infrastructure configuration sections are required")]
    public InfrastructureConfigurationSections InfraConfigSections { get; set; } = null!;
}

/// <summary>
/// Extension methods of <see cref="IServiceCollection"/> for the Infrastructure.
/// </summary>
public static class InfrastructureServiceCollectionExtensions
{

    /// <summary>
    /// Registers the infrastructure services.
    /// This method is a convenience method that registers the DbContext, Repositories & more.
    /// </summary>
    /// <param name="services">The service collection to add the infrastructure services to.</param>
    /// <param name="configureOptions">The configuration options for the infrastructure services.</param>
    /// <returns>The service collection with the infrastructure services added.</returns>
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration,
        Action<InfrastructureOptions> configureOptions)
    {
        var options = new InfrastructureOptions();
        configureOptions(options);

        // Validate options using Data Annotations
        Validator.ValidateObject(options, new ValidationContext(options), validateAllProperties: true);

        services.HttpClients();
        services.Services();

        services.AddJwtAuthentication();

        return services;
    }

    /// <summary>
    /// Adds the HTTP clients to the service collection.
    /// </summary>
    /// <param name="services">The service collection to add the HTTP clients to.</param>
    /// <returns>The service collection with the HTTP clients added.</returns>
    public static IServiceCollection HttpClients(this IServiceCollection services)
    {
        services.AddHttpClient<IJwksFetcher, JwksFetcher>();
        return services;
    }

    /// <summary>
    /// Adds the infrastructure services to the service collection.
    /// </summary>
    /// <param name="services">The service collection to add the services to.</param>
    /// <returns>The service collection with the services added.</returns>
    public static IServiceCollection Services(this IServiceCollection services)
    {
        services.AddSingleton<IJwksManager, JwksManager>();
        return services;
    }

    /// <summary>
    /// Adds the JWT authentication to the service collection.
    /// </summary>
    /// <param name="services">The service collection to add the JWT authentication to.</param>
    /// <returns>The service collection with the JWT authentication added.</returns>
    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services)
    {
        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer();

        services.ConfigureOptions<ConfigureJwtBearerOptions>();

        return services;
    }

    private class ConfigureJwtBearerOptions : IConfigureNamedOptions<JwtBearerOptions>
    {
        private readonly JwtOptions _jwtOptions;
        private readonly IJwksManager _jwksManager;

        public ConfigureJwtBearerOptions(IOptions<JwtOptions> options, IJwksManager jwksManager)
        {
            _jwtOptions = options.Value;
            _jwksManager = jwksManager;
        }

        public void Configure(string? name, JwtBearerOptions options)
        {
            Configure(options);
        }

        public void Configure(JwtBearerOptions options)
        {
            options.Authority = _jwtOptions.Authority;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = _jwtOptions.ValidIssuer,
                ValidAudience = _jwtOptions.ValidAudience,
                IssuerSigningKeyResolver = (token, securityToken, kid, validationParameters) =>
                {
                    if (string.IsNullOrEmpty(kid))
                    {
                        return [];
                    }

                    var publicKey = _jwksManager.GetPublicKey(kid).GetAwaiter().GetResult();
                    return publicKey != null
                        ? new[] { new RsaSecurityKey(publicKey) { KeyId = kid } }
                        : [];
                }
            };
        }
    }
}
