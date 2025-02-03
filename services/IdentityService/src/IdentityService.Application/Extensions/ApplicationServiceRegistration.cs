using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityService.Application.Extensions;

public static class ApplicationServiceRegistration
{
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
            .Where(t => t.IsInterface && t.Namespace?.StartsWith("IdentityService.Application.Interfaces.Services") == true)
            .ToList();

        foreach (var interfaceType in serviceInterfaces)
        {
            var implementation = applicationAssembly.GetTypes()
                .FirstOrDefault(t => t.IsClass
                    && !t.IsAbstract
                    && t.Namespace?.StartsWith("IdentityService.Application.Services") == true
                    && interfaceType.IsAssignableFrom(t));

            if (implementation != null)
            {
                services.Add(new ServiceDescriptor(interfaceType, implementation, lifetime));
            }
        }

        return services;
    }
}