using IdentityService.Domain.Entities;
using IdentityService.Domain.Interfaces.Repositories;
using IdentityService.Infrastructure.Persistence.Entities;
using Microsoft.AspNetCore.Identity;

namespace IdentityService.Infrastructure.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;

    public UserRepository(
        UserManager<User> userManager,
        SignInManager<User> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task<IApplicationUser?> FindByEmailAsync(string email)
    {
        return await _userManager.FindByEmailAsync(email);
    }

    public async Task<bool> CheckPasswordAsync(IApplicationUser user, string password)
    {
        var identityUser = await _userManager.FindByIdAsync(user.Id);
        if (identityUser == null) return false;

        var result = await _signInManager.CheckPasswordSignInAsync(identityUser, password, false);
        return result.Succeeded;
    }

    public async Task<(bool Succeeded, string[] Errors, string? UserId)> CreateAsync(string email, string password)
    {
        var user = new User
        {
            UserName = email,
            Email = email,
        };

        var result = await _userManager.CreateAsync(user, password);
        return (result.Succeeded, result.Errors.Select(e => e.Description).ToArray(), result.Succeeded ? user.Id : null);
    }
}