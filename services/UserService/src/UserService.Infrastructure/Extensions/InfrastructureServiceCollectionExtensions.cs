using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.DependencyInjection;
using UserService.Infrastructure.Authentication.Options;
using Microsoft.Extensions.Configuration;
using UserService.Infrastructure.Authentication.Services;
using System.IdentityModel.Tokens.Jwt;
using MassTransit;
using UserService.Infrastructure.Communication.Options;
using System.Reflection;
using UserService.Infrastructure.Persistence;
using UserService.Infrastructure.Persistence.Options;

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

    public ServiceLifetime RepositoryLifetime { get; set; } = ServiceLifetime.Scoped;
}

/// <summary>
/// Extension methods of <see cref="IServiceCollection"/> for the Infrastructure.
/// </summary>
public static class InfrastructureServiceCollectionExtensions
{
    private const string InfraRepositoriesNamespace = "UserService.Infrastructure.Persistence.Repositories";
    private const string InfraRepositoryInterfacesNamespace = "UserService.Domain.Interfaces.Repositories";

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
        services.AddDbContext();
        services.AddServices();
        services.AddRepositories(options.RepositoryLifetime);

        services.AddJwtAuthentication();
        services.AddMessageQueue();
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
    /// Registers the DbContext for the IdentityService.
    /// </summary>
    /// <param name="services">The service collection to add the DbContext to.</param>
    /// <param name="databaseOptions">The database options.</param>
    /// <returns>The service collection with the DbContext added.</returns>
    private static IServiceCollection AddDbContext(this IServiceCollection services)
    {
        services.AddDbContext<UserServiceDbContext>((sp, options) =>
        {
            var dbOptions = sp.GetRequiredService<IOptions<DatabaseOptions>>().Value;
            options.UseNpgsql(dbOptions.ConnectionString);
        });

        return services;
    }

    /// <summary>
    /// Adds the infrastructure services to the service collection.
    /// </summary>
    /// <param name="services">The service collection to add the services to.</param>
    /// <returns>The service collection with the services added.</returns>
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddSingleton<IJwksManager, JwksManager>();
        return services;
    }

    /// <summary>
    /// Adds the repositories to the service collection.
    /// </summary>
    /// <param name="services">The service collection to add the repositories to.</param>
    /// <returns>The service collection with the repositories added.</returns>
    public static IServiceCollection AddRepositories(this IServiceCollection services, ServiceLifetime lifetime)
    {
        var infrastructureAssembly = Assembly.Load("UserService.Infrastructure")
            ?? throw new InvalidOperationException("Could not find Infrastructure assembly");

        var domainAssembly = Assembly.Load("UserService.Domain")
            ?? throw new InvalidOperationException("Could not find Domain assembly");

        var repositoryInterfaces = domainAssembly.GetTypes()
            .Where(t => t.IsInterface && t.Namespace?.StartsWith(InfraRepositoryInterfacesNamespace) == true)
            .ToList();

        foreach (var interfaceType in repositoryInterfaces)
        {
            var implementation = infrastructureAssembly.GetTypes()
                .FirstOrDefault(t => t.IsClass
                    && !t.IsAbstract
                    && t.Namespace?.StartsWith(InfraRepositoriesNamespace) == true
                    && interfaceType.IsAssignableFrom(t))
                ?? throw new InvalidOperationException($"No implementation found for {interfaceType.Name} in namespace {InfraRepositoriesNamespace}");

            services.Add(new ServiceDescriptor(interfaceType, implementation, lifetime));
        }

        return services;
    }

    /// <summary>
    /// Adds the event consumers to the service collection.
    /// </summary>
    /// <param name="services">The service collection to add the event consumers to.</param>
    /// <returns>The service collection with the event consumers added.</returns>
    // public static IServiceCollection AddEventConsumers(this IServiceCollection services)
    // {
    //     // services.AddHostedService<UserProfileCreatedConsumer>();
    //     // add all consumers here from UserService.Infrastructure.Consumers namespace
    //     var consumerTypes = Assembly.Load("UserService.Infrastructure.Consumers")
    //         .GetTypes()
    //         .Where(t => t.IsClass && !t.IsAbstract && t.Namespace?.StartsWith("UserService.Infrastructure.Consumers") == true)
    //         .ToList();

    //     foreach (var consumerType in consumerTypes)
    //     {
    //         services.AddHostedService(sp =>
    //         {
    //             return new BackgroundService(sp =>
    //             {
    //                 var consumer = sp.GetRequiredService(consumerType) as IConsumer;
    //                 return consumer.Consume(context);
    //             });
    //         });
    //     }
    //     return services;
    // }

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

    /// <summary>
    /// Adds the MassTransit to the service collection.
    /// </summary>
    /// <param name="services">The service collection to add the MassTransit to.</param>
    /// <returns>The service collection with the MassTransit added.</returns>
    public static IServiceCollection AddMessageQueue(this IServiceCollection services)
    {
        services.AddMassTransit(busConfig =>
        {
            busConfig.SetKebabCaseEndpointNameFormatter();
            busConfig.AddConsumers(Assembly.GetExecutingAssembly());

            busConfig.UsingRabbitMq((context, cfg) =>
            {
                var options = context.GetRequiredService<IOptions<RabbitMQOptions>>().Value;

                cfg.Host(options.Host, options.VirtualHost, h =>
                {
                    h.Username(options.Username);
                    h.Password(options.Password);
                });

                cfg.ConfigureEndpoints(context);
            });
        });

        return services;
    }

    private class ConfigureJwtBearerOptions : IConfigureNamedOptions<JwtBearerOptions>
    {
        private readonly JwtOptions _jwtOptions;

        public ConfigureJwtBearerOptions(IOptions<JwtOptions> options)
        {
            _jwtOptions = options.Value;
        }

        public void Configure(string? name, JwtBearerOptions options)
        {
            Configure(options);
        }

        public void Configure(JwtBearerOptions options)
        {
            options.Authority = _jwtOptions.Authority;
            options.MapInboundClaims = false;

            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = _jwtOptions.ValidIssuer,
                ValidAudience = _jwtOptions.ValidAudience
            };

            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = async context =>
                {
                    var authHeader = context.Request.Headers.Authorization.ToString();
                    if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
                    {
                        context.Fail("Missing or invalid Authorization header");
                        return;
                    }

                    // Extract token from "Bearer <token>"
                    var token = authHeader.Substring("Bearer ".Length).Trim();
                    if (string.IsNullOrEmpty(token))
                    {
                        context.Fail("No token present in the request");
                        return;
                    }

                    try
                    {
                        var handler = new JwtSecurityTokenHandler();
                        var jwtToken = handler.ReadJwtToken(token);
                        var kid = jwtToken.Header.Kid;

                        if (string.IsNullOrEmpty(kid))
                        {
                            context.Fail("No 'kid' header present in token");
                            return;
                        }

                        var jwksManager = context.HttpContext.RequestServices.GetRequiredService<IJwksManager>();
                        var key = await jwksManager.GetPublicKey(kid);
                        if (key == null)
                        {
                            context.Fail($"Unable to find a signing key that matches the 'kid' {kid}");
                            return;
                        }

                        options.TokenValidationParameters.IssuerSigningKey = new RsaSecurityKey(key) { KeyId = kid };
                        context.Token = token;  // Set the token for further processing
                    }
                    catch (Exception ex)
                    {
                        context.Fail($"Error processing token: {ex.Message}");
                    }
                }
            };
        }
    }
}
