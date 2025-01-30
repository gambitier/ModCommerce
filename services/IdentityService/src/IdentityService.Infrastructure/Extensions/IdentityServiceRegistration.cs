using IdentityService.Infrastructure.Data;
using IdentityService.Infrastructure.Entities;
using IdentityService.Infrastructure.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityService.Infrastructure.Extensions;

public static class IdentityServiceRegistration
{
    public static IServiceCollection AddIdentityInfrastructure(
        this IServiceCollection services,
        DatabaseOptions databaseOptions)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(databaseOptions.ConnectionString));

        // ===== Register Identity services =====
        // AddIdentity() registers the services needed to manage users, handle authentication, etc
        // Without AddIdentity()
        // - you would have the database tables (as ApplicationDbContext inherits from IdentityDbContext)
        // - you wont be able to inject the UserManager, SignInManager, etc to services/controllers
        services.AddIdentity<User, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        return services;
    }
}
