using IdentityService.Application.Models;
using IdentityService.Application.Services.Interfaces;
using IdentityService.Domain.Entities;
using IdentityService.Infrastructure.Entities;
using Microsoft.AspNetCore.Identity;

namespace IdentityService.Application.Services.Implementations;

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

    public async Task<AuthResultDto> RegisterUserAsync(DomainUser domainUser, string password)
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
            return new AuthResultDto
            {
                Succeeded = false,
                Errors = result.Errors.Select(e => e.Description)
            };
        }

        return new AuthResultDto
        {
            Succeeded = true,
            Token = _tokenService.GenerateJwtToken(user.Id, user.Email)
        };
    }

    public async Task<AuthResultDto> AuthenticateAsync(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            return new AuthResultDto
            {
                Succeeded = false,
                Errors = new[] { "Invalid credentials" }
            };
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
        if (!result.Succeeded)
        {
            return new AuthResultDto
            {
                Succeeded = false,
                Errors = new[] { "Invalid credentials" }
            };
        }

        return new AuthResultDto
        {
            Succeeded = true,
            Token = _tokenService.GenerateJwtToken(user.Id, email)
        };
    }
}