using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace AccountService.API.Controllers;

[Authorize]
[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    [HttpGet("me/orgs")]
    [EndpointSummary("Get User Organizations")]
    [EndpointDescription("Gets the organizations that the user is a member of")]
    public IActionResult GetUserOrganizations()
    {
        // Logic to get user organizations
        return Ok(new[] { new { org_id = "456", role = "admin" } });
    }
}
