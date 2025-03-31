using Microsoft.EntityFrameworkCore;
using AccountService.Infrastructure.Persistence.Entities;

namespace AccountService.Infrastructure.Persistence;

public class AccountDbContext : DbContext
{
    public AccountDbContext(DbContextOptions<AccountDbContext> options)
        : base(options)
    {
    }

    public DbSet<UserProfileEntity> UserProfiles { get; set; }
    public DbSet<OrganizationEntity> Organizations { get; set; }
    public DbSet<OrganizationMemberInvitation> OrganizationMemberInvitations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AccountDbContext).Assembly);
    }
}