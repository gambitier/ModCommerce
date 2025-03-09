using Microsoft.EntityFrameworkCore;
using UserService.Domain.Entities;

namespace UserService.Infrastructure.Persistence;

public class UserServiceDbContext : DbContext
{
    public UserServiceDbContext(DbContextOptions<UserServiceDbContext> options)
        : base(options)
    {
    }

    public DbSet<UserProfile> UserProfiles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserServiceDbContext).Assembly);
    }
}