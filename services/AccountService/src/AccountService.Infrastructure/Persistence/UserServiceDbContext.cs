using Microsoft.EntityFrameworkCore;
using AccountService.Infrastructure.Persistence.Entities;

namespace AccountService.Infrastructure.Persistence;

public class UserServiceDbContext : DbContext
{
    public UserServiceDbContext(DbContextOptions<UserServiceDbContext> options)
        : base(options)
    {
    }

    public DbSet<UserProfileEntity> UserProfiles { get; set; }
    public DbSet<OrganizationEntity> Organizations { get; set; }
    public DbSet<OrganizationMemberInvitation> OrganizationMemberInvitations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserServiceDbContext).Assembly);
    }
}