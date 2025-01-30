using IdentityService.Infrastructure.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Infrastructure.Data;

/// <summary>
/// ApplicationDbContext is the main DbContext for the IdentityService.
/// It inherits from IdentityDbContext, which provides the necessary DbSets and context for Identity entities.
/// </summary>
public class ApplicationDbContext : IdentityDbContext<User>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
}
