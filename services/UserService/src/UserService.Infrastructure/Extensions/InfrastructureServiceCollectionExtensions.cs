using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.DependencyInjection;

namespace UserService.Infrastructure.Extensions;


/// <summary>
/// Options for the infrastructure services.
/// </summary>
public class InfrastructureOptions
{
}

/// <summary>
/// Extension methods of <see cref="IServiceCollection"/> for the Infrastructure.
/// </summary>
public static class InfrastructureServiceCollectionExtensions
{

    /// <summary>
    /// Registers the infrastructure services.
    /// This method is a convenience method that registers the DbContext, Repositories & more.
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

        return services;
    }
}
