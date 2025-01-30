using System.Reflection;

namespace IdentityService.API.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services,
        ServiceLifetime lifetime = ServiceLifetime.Scoped)
    {
        var assembly = Assembly.GetAssembly(typeof(Application.Services.Interfaces.IAuthenticationService))
            ?? throw new InvalidOperationException("Could not find Application assembly");

        var interfaces = assembly.GetTypes()
            .Where(t => t.IsInterface && t.Namespace?.StartsWith("IdentityService.Application.Services.Interfaces") == true)
            .ToList();

        foreach (var interfaceType in interfaces)
        {
            var implementation = assembly.GetTypes()
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
}