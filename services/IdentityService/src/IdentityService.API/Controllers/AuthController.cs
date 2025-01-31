using IdentityService.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using IdentityService.API.Contracts.Auth;
using IdentityService.Domain.DomainModels;

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
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var result = await _authService.RegisterUserAsync(new DomainUser
        {
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
        }, request.Password);

        if (!result.Succeeded)
            return BadRequest(result.Errors);

        return Ok(new { message = "User registered successfully!" });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var result = await _authService.AuthenticateAsync(request.Email, request.Password);

        if (!result.Succeeded)
            return Unauthorized("Invalid credentials");

        return Ok(new { access_token = result.Token });
    }
}

