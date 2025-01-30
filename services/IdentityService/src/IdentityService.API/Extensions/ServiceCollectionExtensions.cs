using System.Reflection;

namespace IdentityService.API.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services,
        ServiceLifetime lifetime = ServiceLifetime.Scoped)
    {
        var assembly = Assembly.GetAssembly(typeof(IdentityService.Application.Services.IAuthenticationService));

        var interfaces = assembly.GetTypes()
            .Where(t => t.IsInterface && t.Namespace.StartsWith("IdentityService.Application.Services"))
            .ToList();

        foreach (var interfaceType in interfaces)
        {
            var implementation = assembly.GetTypes()
                .FirstOrDefault(t => t.IsClass
                    && !t.IsAbstract
                    && interfaceType.IsAssignableFrom(t));

            if (implementation != null)
            {
                services.Add(new ServiceDescriptor(interfaceType, implementation, lifetime));
            }
        }

        return services;
    }
}