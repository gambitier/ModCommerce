using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace AccountService.API.Controllers;

[Authorize]
[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok("Hello World");
    }

    [HttpGet("me/orgs")]
    public IActionResult GetUserOrganizations()
    {
        // Logic to get user organizations
        return Ok(new[] { new { org_id = "456", role = "admin" } });
    }
}
