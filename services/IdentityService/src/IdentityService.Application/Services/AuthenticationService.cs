using IdentityService.Domain.Entities;
using IdentityService.Infrastructure.Entities;
using Microsoft.AspNetCore.Identity;

namespace IdentityService.Application.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly ITokenService _tokenService;

    public AuthenticationService(
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        ITokenService tokenService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
    }

    public async Task<AuthResult> RegisterUserAsync(DomainUser domainUser, string password)
    {
        var user = new User
        {
            UserName = domainUser.Email,
            Email = domainUser.Email,
            FirstName = domainUser.FirstName,
            LastName = domainUser.LastName
        };

        var result = await _userManager.CreateAsync(user, password);

        if (!result.Succeeded)
        {
            return new AuthResult
            {
                Succeeded = false,
                Errors = result.Errors.Select(e => e.Description)
            };
        }

        return new AuthResult
        {
            Succeeded = true,
            Token = _tokenService.GenerateJwtToken(user.Id, user.Email)
        };
    }

    public async Task<AuthResult> AuthenticateAsync(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            return new AuthResult
            {
                Succeeded = false,
                Errors = new[] { "Invalid credentials" }
            };
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
        if (!result.Succeeded)
        {
            return new AuthResult
            {
                Succeeded = false,
                Errors = new[] { "Invalid credentials" }
            };
        }

        return new AuthResult
        {
            Succeeded = true,
            Token = _tokenService.GenerateJwtToken(user.Id, user.Email)
        };
    }
}