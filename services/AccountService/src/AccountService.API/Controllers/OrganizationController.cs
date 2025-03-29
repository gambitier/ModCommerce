using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AccountService.Contracts.API.Organizations.Requests;
using AccountService.Contracts.API.Organizations.Enums;
using AccountService.Domain.Models.Organizations.DomainModels;
using AccountService.Domain.Interfaces.Services;
using MapsterMapper;
using AccountService.Contracts.API.Organizations.Responses;
using FluentResults.Extensions.AspNetCore;
using System.Security.Claims;
using AccountService.API.Extensions;
using Mapster;
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

    [HttpPost]
    public async Task<IActionResult> CreateOrganization([FromBody] CreateOrganizationRequest org)
    {
        var userId = User.GetUserId();
        var createDomainModel = _mapper.Map<CreateOrganizationDomainModel>(org);

        var organizationResult = await _organizationService.CreateOrganizationAsync(userId, createDomainModel);
        if (organizationResult.IsFailed)
            return organizationResult.ToActionResult();

        return Ok(new CreateOrganizationResponse
        {
            Id = organizationResult.Value,
        });
    }

    [HttpGet("{orgId}")]
    public async Task<IActionResult> GetOrganization(Guid orgId)
    {
        var organizationResult = await _organizationService.GetByIdAsync(orgId);
        if (organizationResult.IsFailed)
            return organizationResult.ToActionResult();

        return Ok(organizationResult.Value);
    }

    [HttpPost("{orgId}/invitations")]
    public async Task<IActionResult> InviteMember(Guid orgId, [FromBody] InviteOrganizationMemberRequest request)
    {
        // TODO: possible bug: check if enum mapping is correct
        var domainModel = (request, orgId).Adapt<InviteOrganizationMemberDomainModel>();

        var result = await _organizationService.InviteMemberAsync(User.GetUserId(), domainModel);
        if (result.IsFailed)
            return result.ToActionResult();

        return Ok(new { message = "User invited to organization" });
    }

    [HttpPost("invitations/{invitationId}/accept")]
    public async Task<IActionResult> AcceptInvitation(Guid invitationId)
    {
        var result = await _organizationService.AcceptOrganizationInvitationAsync(User.GetUserId(), invitationId);
        if (result.IsFailed)
            return result.ToActionResult();

        return Ok(new { message = "Invitation accepted" });
    }

    [HttpPut("{orgId}/members/{userId}")]
    public IActionResult UpdateMemberRole(string orgId, string userId, [FromBody] OrganizationRole role)
    {
        // Logic to update member role
        return NoContent();
    }

    [HttpDelete("{orgId}/members/{userId}")]
    public IActionResult RemoveMember(string orgId, string userId)
    {
        // Logic to remove member
        return NoContent();
    }
}

