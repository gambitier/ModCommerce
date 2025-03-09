using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace UserService.Application.Extensions;

/// <summary>
/// Options for the application layer services.
/// </summary>
public class ApplicationOptions
{
    public ServiceLifetime ServiceLifetime { get; set; } = ServiceLifetime.Scoped;
}


/// <summary>
/// Extension methods of <see cref="IServiceCollection"/> for the Application.
/// </summary>
public static class ApplicationServiceCollectionExtensions
{
    private const string ApplicationServicesNamespace = "UserService.Application.Services";
    private const string ApplicationServicesInterfacesNamespace = "UserService.Domain.Interfaces.Services";

    /// <summary>
    /// Registers the application layer services.
    /// This method is a convenience method that registers all application services.
    /// </summary>
    /// <param name="services">The service collection to add the application services to.</param>
    /// <param name="configureOptions">The configuration options for the application services.</param>
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
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        return services;
    }

    /// <summary>
    /// Registers all application services in the UserService.Application assembly.
    /// </summary>
    /// <param name="services">The service collection to add the services to.</param>
    /// <param name="lifetime">The lifetime of the services.</param>
    /// <returns>The service collection with the services added.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the Application assembly is not found.</exception>
    private static IServiceCollection AddApplicationServices(
        this IServiceCollection services,
        ServiceLifetime lifetime = ServiceLifetime.Scoped)
    {
        var applicationAssembly = Assembly.Load("UserService.Application")
            ?? throw new InvalidOperationException("Could not find Application assembly");

        var domainAssembly = Assembly.Load("UserService.Domain")
            ?? throw new InvalidOperationException("Could not find Domain assembly");

        var serviceInterfaces = domainAssembly.GetTypes()
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