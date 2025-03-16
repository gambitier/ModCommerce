using IdentityService.Infrastructure.Persistence.Configurations;
using IdentityService.Infrastructure.Persistence.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Infrastructure.Persistence;

/// <summary>
/// ApplicationDbContext is the main DbContext for the IdentityService.
/// It inherits from IdentityDbContext, which provides the necessary DbSets and context for Identity entities.
/// </summary>
public class IdentityDbContext : IdentityDbContext<Entities.IdentityUser>
{
    public IdentityDbContext(DbContextOptions<IdentityDbContext> options) : base(options)
    {
    }

    public DbSet<RefreshToken> RefreshTokens { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // rename the default tables to avoid conflicts with the UserProfile table
        builder.Entity<Entities.IdentityUser>(entity => entity.ToTable(name: "Users"));
        builder.Entity<IdentityRole>(entity => entity.ToTable(name: "Roles"));
        builder.Entity<IdentityUserRole<string>>(entity => entity.ToTable("UserRoles"));
        builder.Entity<IdentityUserClaim<string>>(entity => entity.ToTable("UserClaims"));
        builder.Entity<IdentityUserLogin<string>>(entity => entity.ToTable("UserLogins"));
        builder.Entity<IdentityRoleClaim<string>>(entity => entity.ToTable("RoleClaims"));
        builder.Entity<IdentityUserToken<string>>(entity => entity.ToTable("UserTokens"));

        // apply the custom configurations
        builder.ApplyConfiguration(new IdentityUserConfiguration());
        builder.ApplyConfiguration(new RefreshTokenConfiguration());
    }
}