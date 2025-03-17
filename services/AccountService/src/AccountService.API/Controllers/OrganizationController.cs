using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AccountService.Contracts.API.Organizations.Requests;
using AccountService.Contracts.API.Organizations.Enums;

namespace AccountService.API.Controllers;

[Authorize]
[ApiController]
[Route("api/organizations")]
public class OrganizationController : ControllerBase
{
    // POST /orgs
    [HttpPost]
    public IActionResult CreateOrganization([FromBody] CreateOrganizationRequest org)
    {
        // Logic to create organization
        return Ok(new { org_id = "456", name = org.Name });
    }

    // GET /orgs/{orgId}
    [HttpGet("{orgId}")]
    public IActionResult GetOrganization(string orgId)
    {
        // Logic to get organization details
        return Ok(new { org_id = "456", name = "Acme Corp", owner = "123" });
    }

    // POST /orgs/{org_id}/members
    [HttpPost("{orgId}/members")]
    public IActionResult AddMember(string orgId, [FromBody] AddOrganizationMemberRequest request)
    {
        // Logic to add member
        return Ok(new { message = "User added to org" });
    }

    // PUT /orgs/{org_id}/members/{user_id}
    [HttpPut("{orgId}/members/{userId}")]
    public IActionResult UpdateMemberRole(string orgId, string userId, [FromBody] OrganizationRole role)
    {
        // Logic to update member role
        return NoContent();
    }

    // DELETE /orgs/{org_id}/members/{user_id}
    [HttpDelete("{orgId}/members/{userId}")]
    public IActionResult RemoveMember(string orgId, string userId)
    {
        // Logic to remove member
        return NoContent();
    }
}

