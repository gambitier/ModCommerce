using Microsoft.AspNetCore.Mvc;
using IdentityService.API.Contracts.Auth;
using IdentityService.Application.Interfaces.Services;
using IdentityService.Application.Models;
using FluentResults.Extensions.AspNetCore;

namespace IdentityService.API.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthenticationService _authService;

    public AuthController(IAuthenticationService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthResponse>> Register([FromBody] RegisterRequest request)
    {
        var result = await _authService.RegisterUserAsync(
            new CreateUserDto { Email = request.Email },
            request.Password
        );
        if (result.IsFailed || result.Value.Token == null)
            return result.ToActionResult();

        var authResponse = new AuthResponse
        {
            Token = result.Value.Token,
        };

        return Ok(authResponse);

    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginRequest request)
    {
        var result = await _authService.AuthenticateAsync(request.Email, request.Password);
        if (result.IsFailed || result.Value.Token == null)
            return result.ToActionResult();

        var authResponse = new AuthResponse
        {
            Token = result.Value.Token,
        };

        return Ok(authResponse);
    }
}
