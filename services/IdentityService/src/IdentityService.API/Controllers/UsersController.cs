using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using IdentityService.Contracts.API.Users.Responses;
using IdentityService.Application.Interfaces.Services;
using System.Security.Claims;
using FluentResults.Extensions.AspNetCore;
using MapsterMapper;

namespace IdentityService.API.Controllers;

[Authorize]
[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IMapper _mapper;

    public UsersController(IUserService userService, IMapper mapper)
    {
        _userService = userService;
        _mapper = mapper;
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

        return Ok(_mapper.Map<UserResponse>(result.Value));
    }

    [HttpGet]
    public async Task<ActionResult<UsersResponse>> GetAllUsers()
    {
        var result = await _userService.GetAllUsersAsync();
        if (result.IsFailed)
            return result.ToActionResult();

        return Ok(new UsersResponse(
           Users: _mapper.Map<IEnumerable<UserResponse>>(result.Value)
        ));
    }
}
