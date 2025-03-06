using Microsoft.AspNetCore.Mvc;
using IdentityService.Application.Interfaces.Services;

namespace IdentityService.API.Controllers;

[ApiController]
[Route(".well-known")]
public class WellKnownController : ControllerBase
{
    private readonly IAuthenticationService _authService;

    public WellKnownController(IAuthenticationService authService)
    {
        _authService = authService;
    }

    [HttpGet("jwks.json")]
    public IActionResult GetJwks()
    {
        return Ok(new
        {
            keys = new[] { _authService.GetJsonWebKey() }
        });
    }
}
