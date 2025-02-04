using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityService.Application.Extensions;

/// <summary>
/// Extension methods of <see cref="IServiceCollection"/> for the Application.
/// </summary>
public static class ApplicationServiceCollectionExtensions
{
    private const string ApplicationServicesNamespace = "IdentityService.Application.Services";
    private const string ApplicationServicesInterfacesNamespace = "IdentityService.Application.Interfaces.Services";

    /// <summary>
    /// Options for the application layer services.
    /// </summary>
    public class ApplicationOptions
    {
        public ServiceLifetime ServiceLifetime { get; set; } = ServiceLifetime.Scoped;
    }

    /// <summary>
    /// Registers the application layer services.
    /// This method is a convenience method that registers all application services.
    /// </summary>
    /// <param name="services">The service collection to add the application services to.</param>
    /// <param name="lifetime">The lifetime of the services.</param>
    /// <returns>The service collection with the application services added.</returns>
    public static IServiceCollection AddApplication(
        this IServiceCollection services,
        Action<ApplicationOptions> configureOptions)
    {
        var options = new ApplicationOptions();
        configureOptions(options);

        // validate options
        Validator.ValidateObject(options, new ValidationContext(options), validateAllProperties: true);

        services.AddApplicationServices(options.ServiceLifetime);
        // Add any additional application layer registrations here
        // For example:
        // services.AddAutoMapper(...);
        // services.AddValidators(...);

        return services;
    }

    /// <summary>
    /// Registers all application services in the IdentityService.Application assembly.
    /// </summary>
    /// <param name="services">The service collection to add the services to.</param>
    /// <param name="lifetime">The lifetime of the services.</param>
    /// <returns>The service collection with the services added.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the Application assembly is not found.</exception>
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services,
        ServiceLifetime lifetime = ServiceLifetime.Scoped)
    {
        var applicationAssembly = Assembly.Load("IdentityService.Application")
            ?? throw new InvalidOperationException("Could not find Application assembly");

        var serviceInterfaces = applicationAssembly.GetTypes()
            .Where(t => t.IsInterface && t.Namespace?.StartsWith(ApplicationServicesInterfacesNamespace) == true)
            .ToList();

        foreach (var interfaceType in serviceInterfaces)
        {
            var implementation = applicationAssembly.GetTypes()
                .FirstOrDefault(t => t.IsClass
                    && !t.IsAbstract
                    && t.Namespace?.StartsWith(ApplicationServicesNamespace) == true
                    && interfaceType.IsAssignableFrom(t))
                ?? throw new InvalidOperationException($"No implementation found for {interfaceType.Name} in namespace {ApplicationServicesNamespace}");

            services.Add(new ServiceDescriptor(interfaceType, implementation, lifetime));
        }

        return services;
    }
}