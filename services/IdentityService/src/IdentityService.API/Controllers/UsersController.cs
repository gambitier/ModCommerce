using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using IdentityService.API.Contracts.Users;
using IdentityService.Application.Interfaces.Services;
using System.Security.Claims;
using FluentResults.Extensions.AspNetCore;
namespace IdentityService.API.Controllers;

[Authorize]
[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("me")]
    public async Task<ActionResult<UserResponse>> GetCurrentUser()
    {
        var email = User.FindFirst(ClaimTypes.Email)?.Value;
        if (email == null)
            return Unauthorized();

        var result = await _userService.GetCurrentUserAsync(email);
        if (result.IsFailed)
            return result.ToActionResult();

        return new UserResponse
        {
            Id = result.Value.Id,
            Email = result.Value.Email,
            CreatedAt = result.Value.CreatedAt
        };
    }
}
