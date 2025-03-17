using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AccountService.Contracts.API.Organizations.Requests;
using AccountService.Contracts.API.Organizations.Enums;
using AccountService.Domain.Models.Organizations.DomainModels;
using AccountService.Domain.Interfaces.Services;
using MapsterMapper;
using AccountService.Contracts.API.Organizations.Responses;
using FluentResults.Extensions.AspNetCore;
namespace AccountService.API.Controllers;

[Authorize]
[ApiController]
[Route("api/organizations")]
public class OrganizationController : ControllerBase
{
    private readonly IOrganizationService _organizationService;
    private readonly IMapper _mapper;

    public OrganizationController(
        IOrganizationService organizationService,
        IMapper mapper)
    {
        _organizationService = organizationService;
        _mapper = mapper;
    }

    // POST /orgs
    [HttpPost]
    public async Task<IActionResult> CreateOrganization([FromBody] CreateOrganizationRequest org)
    {
        var createOrganizationDomainModel = _mapper.Map<CreateOrganizationDomainModel>(org);

        var organizationResult = await _organizationService.CreateOrganizationAsync(createOrganizationDomainModel);
        if (organizationResult.IsFailed)
            return organizationResult.ToActionResult();

        return Ok(new CreateOrganizationResponse
        {
            Id = organizationResult.Value,
        });
    }

    // GET /orgs/{orgId}
    [HttpGet("{orgId}")]
    public async Task<IActionResult> GetOrganization(Guid orgId)
    {
        var organization = await _organizationService.GetByIdAsync(orgId);
        return Ok(organization);
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

