using IdentityService.Domain.Entities;
using IdentityService.Application.Services;
using Microsoft.AspNetCore.Mvc;

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

public class RegisterRequest
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}

public class LoginRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
}
