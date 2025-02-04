using Microsoft.AspNetCore.Identity;
using IdentityService.Domain.Entities;

namespace IdentityService.Infrastructure.Persistence.Entities;

public class User : IdentityUser, IApplicationUser
{
    // Empty - using only IdentityUser base functionality
}
