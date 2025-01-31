using System.Reflection;

namespace IdentityService.API.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services,
        ServiceLifetime lifetime = ServiceLifetime.Scoped)
    {
        var applicationAssembly = Assembly.Load("IdentityService.Application")
            ?? throw new InvalidOperationException("Could not find Application assembly");

        var interfaces = applicationAssembly.GetTypes()
            .Where(t => t.IsInterface && t.Namespace?.StartsWith("IdentityService.Application.Services.Interfaces") == true)
            .ToList();

        foreach (var interfaceType in interfaces)
        {
            var implementation = applicationAssembly.GetTypes()
                .FirstOrDefault(t => t.IsClass
                    && !t.IsAbstract
                    && t.Namespace?.StartsWith("IdentityService.Application.Services.Implementations") == true
                    && interfaceType.IsAssignableFrom(t));

            if (implementation != null)
            {
                services.Add(new ServiceDescriptor(interfaceType, implementation, lifetime));
            }
        }

        return services;
    }

    public static IServiceCollection AddInfrastructureServices(
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