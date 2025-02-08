namespace IdentityService.Infrastructure.Persistence.Entities;

public class IdentityUser : Microsoft.AspNetCore.Identity.IdentityUser
{
    // Empty - using only IdentityUser base functionality
    public static IdentityUser Create(string email)
    {
        return new IdentityUser
        {
            UserName = email,
            Email = email,
        };
    }
}
