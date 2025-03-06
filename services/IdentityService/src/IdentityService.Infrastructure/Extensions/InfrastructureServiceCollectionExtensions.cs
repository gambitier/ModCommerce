using IdentityService.Infrastructure.Persistence;
using IdentityService.Infrastructure.Persistence.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.ComponentModel.DataAnnotations;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using IdentityService.Infrastructure.Authentication.Options;
using IdentityService.Domain.Interfaces.Persistence;
using IdentityService.Domain.Interfaces.Communication;
using IdentityService.Infrastructure.Communication;
using IdentityService.Infrastructure.Authentication.Services;
using System.Security.Cryptography;
using Microsoft.Extensions.Configuration;

namespace IdentityService.Infrastructure.Extensions;

/// <summary>
/// Extension methods of <see cref="IServiceCollection"/> for the Infrastructure.
/// </summary>
public static class InfrastructureServiceCollectionExtensions
{
    private const string AuthenticationServicesNamespace = "IdentityService.Infrastructure.Authentication.Services";
    private const string RepositoriesNamespace = "IdentityService.Infrastructure.Persistence.Repositories";

    public class InfrastructureOptions
    {
        public ServiceLifetime RepositoryLifetime { get; set; } = ServiceLifetime.Scoped;
        public ServiceLifetime AuthenticationServicesLifetime { get; set; } = ServiceLifetime.Scoped;
    }

    /// <summary>
    /// Registers the infrastructure services.
    /// This method is a convenience method that registers the DbContext, Identity, Repositories & more.
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

        services.AddDbContext(
            configuration.Get<DatabaseOptions>()
            ?? throw new InvalidOperationException("DatabaseOptions should be configured")
        );
        services.AddIdentity();
        services.AddUnitOfWork();
        services.AddRepositories(options.RepositoryLifetime);
        services.AddAuthenticationServices(options.AuthenticationServicesLifetime);
        services.AddJwtAuthentication(
            configuration.Get<JwtOptions>()
            ?? throw new InvalidOperationException("JwtOptions should be configured")
        );
        services.AddAuthorizationServices();
        services.AddEmailService();

        return services;
    }

    /// <summary>   
    /// Registers the DbContext for the IdentityService.
    /// </summary>
    /// <param name="services">The service collection to add the DbContext to.</param>
    /// <param name="databaseOptions">The database options.</param>
    /// <returns>The service collection with the DbContext added.</returns>
    private static IServiceCollection AddDbContext(this IServiceCollection services, DatabaseOptions databaseOptions)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(databaseOptions.ConnectionString));

        return services;
    }

    /// <summary>
    /// Registers the Identity services.
    /// </summary>
    /// <param name="services">The service collection to add the Identity services to.</param>
    /// <returns>The service collection with the Identity services added.</returns>
    private static IServiceCollection AddIdentity(this IServiceCollection services)
    {
        const string EmailConfirmationTokenProvider = "CustomEmailConfirmation";
        services.AddTransient<CustomEmailConfirmationTokenProvider<Persistence.Entities.IdentityUser>>();

        // AddIdentity() registers the services needed to manage users, handle authentication, etc
        // Without AddIdentity()
        // - you would have the database tables (as ApplicationDbContext inherits from IdentityDbContext)
        // - you wont be able to inject the UserManager, SignInManager, etc to services/controllers
        services
            .AddIdentity<Persistence.Entities.IdentityUser, IdentityRole>(options =>
            {
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireDigit = false;

                options.Tokens.EmailConfirmationTokenProvider = EmailConfirmationTokenProvider;
                options.Tokens.ProviderMap.Add(EmailConfirmationTokenProvider,
                    new TokenProviderDescriptor(
                        typeof(CustomEmailConfirmationTokenProvider<Persistence.Entities.IdentityUser>)));
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        return services;
    }

    /// <summary>
    /// Registers the JWT authentication services.
    /// </summary>
    /// <param name="services">The service collection to add the JWT authentication services to.</param>
    /// <param name="jwtOptions">The JWT options.</param>
    /// <returns>The service collection with the JWT authentication services added.</returns>
    private static void AddJwtAuthentication(this IServiceCollection services, JwtOptions jwtOptions)
    {
        using var rsa = RSA.Create();
        rsa.ImportFromPem(jwtOptions.PublicKeyPem);
        var securityKey = new RsaSecurityKey(rsa.ExportParameters(false))
        {
            KeyId = jwtOptions.KeyId
        };

        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,

                    ValidateIssuer = true,
                    ValidIssuer = jwtOptions.Issuer,

                    ValidateAudience = true,
                    ValidAudience = jwtOptions.Audience,

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = securityKey
                };
            });
    }

    /// <summary>
    /// Registers the Authorization services.
    /// </summary>
    /// <param name="services">The service collection to add the Authorization services to.</param>
    /// <returns>The service collection with the Authorization services added.</returns>
    private static IServiceCollection AddAuthorizationServices(this IServiceCollection services)
    {
        services.AddAuthorization();
        return services;
    }

    /// <summary>
    /// Registers the Authentication services.
    /// </summary>
    /// <param name="services">The service collection to add the Authentication services to.</param>
    /// <param name="lifetime">The lifetime of the Authentication services.</param>
    /// <returns>The service collection with the Authentication services added.</returns>
    private static IServiceCollection AddAuthenticationServices(this IServiceCollection services, ServiceLifetime lifetime)
    {
        var domainAssembly = Assembly.Load("IdentityService.Domain")
            ?? throw new InvalidOperationException("Could not find Domain assembly");

        var infrastructureAssembly = Assembly.Load("IdentityService.Infrastructure")
            ?? throw new InvalidOperationException("Could not find Infrastructure assembly");

        var interfaces = domainAssembly.GetTypes()
            .Where(t => t.IsInterface && t.Namespace?.StartsWith("IdentityService.Domain.Interfaces.AuthenticationServices") == true)
            .ToList();

        foreach (var interfaceType in interfaces)
        {
            var implementation = infrastructureAssembly.GetTypes()
                .FirstOrDefault(t => t.IsClass
                    && !t.IsAbstract
                    && t.Namespace?.StartsWith(AuthenticationServicesNamespace) == true
                    && interfaceType.IsAssignableFrom(t))
                ?? throw new InvalidOperationException($"No implementation found for {interfaceType.Name} in namespace {AuthenticationServicesNamespace}");

            services.Add(new ServiceDescriptor(interfaceType, implementation, lifetime));
        }

        return services;
    }


    /// <summary>
    /// Registers all repositories in the IdentityService.Domain and IdentityService.Infrastructure assemblies.
    /// </summary>
    /// <param name="services">The service collection to add the repositories to.</param>
    /// <param name="lifetime">The lifetime of the repositories.</param>
    /// <returns>The service collection with the repositories added.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the Domain or Infrastructure assemblies are not found.</exception>
    private static IServiceCollection AddRepositories(
        this IServiceCollection services,
        ServiceLifetime lifetime = ServiceLifetime.Scoped)
    {
        var domainAssembly = Assembly.Load("IdentityService.Domain")
            ?? throw new InvalidOperationException("Could not find Domain assembly");

        var infrastructureAssembly = Assembly.Load("IdentityService.Infrastructure")
            ?? throw new InvalidOperationException("Could not find Infrastructure assembly");

        var interfaces = domainAssembly.GetTypes()
            .Where(t => t.IsInterface && t.Namespace?.StartsWith("IdentityService.Domain.Interfaces.Repositories") == true)
            .ToList();

        foreach (var interfaceType in interfaces)
        {
            var implementation = infrastructureAssembly.GetTypes()
                .FirstOrDefault(t => t.IsClass
                    && !t.IsAbstract
                    && t.Namespace?.StartsWith(RepositoriesNamespace) == true
                    && interfaceType.IsAssignableFrom(t))
                ?? throw new InvalidOperationException($"No implementation found for {interfaceType.Name} in namespace {RepositoriesNamespace}");

            services.Add(new ServiceDescriptor(interfaceType, implementation, lifetime));
        }

        return services;
    }

    private static IServiceCollection AddUnitOfWork(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        return services;
    }

    /// <summary>
    /// Registers the email service with a transient lifetime.
    /// </summary>
    /// <remarks>
    /// Using transient lifetime because:
    /// 1. The service doesn't maintain shared state between requests
    /// 2. Resources (SMTP connections) should be disposed quickly after use
    /// 3. Avoids potential issues with stale connections in scoped lifetime
    /// </remarks>
    private static IServiceCollection AddEmailService(this IServiceCollection services)
    {
        services.AddTransient<IEmailService, EmailService>();
        return services;
    }
}
