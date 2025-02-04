using IdentityService.Infrastructure.Persistence.Data;
using IdentityService.Infrastructure.Persistence.Entities;
using IdentityService.Infrastructure.Persistence.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.ComponentModel.DataAnnotations;

namespace IdentityService.Infrastructure.Extensions;

/// <summary>
/// Extension methods of <see cref="IServiceCollection"/> for the Infrastructure.
/// </summary>
public static class InfrastructureServiceCollectionExtensions
{
    public class InfrastructureOptions
    {
        /// <summary>
        /// Database configuration options required for setting up the infrastructure.
        /// The null! annotation indicates this property must be set during initialization,
        /// even though it's initially null. This is validated using the Required attribute.
        /// </summary>
        [Required(ErrorMessage = "DatabaseOptions must be configured when calling AddInfrastructure")]
        public DatabaseOptions DatabaseOptions { get; set; } = null!;
        public ServiceLifetime RepositoryLifetime { get; set; } = ServiceLifetime.Scoped;
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
        Action<InfrastructureOptions> configureOptions)
    {
        var options = new InfrastructureOptions();
        configureOptions(options);

        // Validate options using Data Annotations
        Validator.ValidateObject(options, new ValidationContext(options), validateAllProperties: true);

        services.AddDbContext(options.DatabaseOptions);
        services.AddIdentity();
        services.AddRepositories(options.RepositoryLifetime);

        return services;
    }

    /// <summary>   
    /// Registers the DbContext for the IdentityService.
    /// </summary>
    /// <param name="services">The service collection to add the DbContext to.</param>
    /// <param name="databaseOptions">The database options.</param>
    /// <returns>The service collection with the DbContext added.</returns>
    public static IServiceCollection AddDbContext(this IServiceCollection services, DatabaseOptions databaseOptions)
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
    public static IServiceCollection AddIdentity(this IServiceCollection services)
    {
        // AddIdentity() registers the services needed to manage users, handle authentication, etc
        // Without AddIdentity()
        // - you would have the database tables (as ApplicationDbContext inherits from IdentityDbContext)
        // - you wont be able to inject the UserManager, SignInManager, etc to services/controllers
        services.AddIdentity<User, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        return services;
    }

    /// <summary>
    /// Registers all repositories in the IdentityService.Domain and IdentityService.Infrastructure assemblies.
    /// </summary>
    /// <param name="services">The service collection to add the repositories to.</param>
    /// <param name="lifetime">The lifetime of the repositories.</param>
    /// <returns>The service collection with the repositories added.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the Domain or Infrastructure assemblies are not found.</exception>
    public static IServiceCollection AddRepositories(
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
                    && t.Namespace?.StartsWith("IdentityService.Infrastructure.Persistence.Repositories") == true
                    && interfaceType.IsAssignableFrom(t))
                ?? throw new InvalidOperationException($"No implementation found for {interfaceType.Name}");

            services.Add(new ServiceDescriptor(interfaceType, implementation, lifetime));
        }

        return services;
    }
}
