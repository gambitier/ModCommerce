using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityService.Infrastructure.Extensions;

public static class RepositoryRegistration
{
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
                    && t.Namespace?.StartsWith("IdentityService.Infrastructure.Repositories") == true
                    && interfaceType.IsAssignableFrom(t));

            if (implementation != null)
            {
                services.Add(new ServiceDescriptor(interfaceType, implementation, lifetime));
            }
        }

        return services;
    }
}