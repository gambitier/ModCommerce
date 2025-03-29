using AccountService.Domain.Errors;
using AccountService.Domain.Models.Organizations.DomainModels;
using AccountService.Domain.Models.Organizations.Enums;
using FluentResults;

namespace AccountService.Infrastructure.Persistence.Entities;

/// <summary>
/// Represents a user's invitation to join an organization database entity.
/// </summary>
public class OrganizationMemberInvitation
{
    public Guid Id { get; private set; }
    public string UserId { get; private set; }
    public string InvitedByUserId { get; private set; }
    public Guid OrganizationId { get; private set; }
    public DateTime? AcceptedAt { get; private set; }
    public DateTime? RejectedAt { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public DateTime ExpiresAt { get; private set; }
    public UserOrganizationMembershipRole Role { get; private set; }

    private OrganizationMemberInvitation(
        string userId,
        string invitedByUserId,
        Guid organizationId,
        UserOrganizationMembershipRole role)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        InvitedByUserId = invitedByUserId;
        OrganizationId = organizationId;
        Role = role;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
        // TODO: make this expiration configurable
        ExpiresAt = DateTime.UtcNow.AddDays(7);
    }

    public static OrganizationMemberInvitation Create(
        string invitedByUserId,
        InviteOrganizationMemberDomainModel createOrganizationMembershipRoleDomainModel)
    {
        var (userId, organizationId, role) = createOrganizationMembershipRoleDomainModel;
        var invitation = new OrganizationMemberInvitation(userId, invitedByUserId, organizationId, role);
        // TODO: emit event for organization member invited
        // handler of event will send invitation email
        return invitation;
    }

    private Result ValidateInvitationStatus(string userId)
    {
        if (AcceptedAt != null)
            return Result.Fail(OrganizationMemberInvitationsDomainErrors.InvitationAlreadyAccepted);

        if (RejectedAt != null)
            return Result.Fail(OrganizationMemberInvitationsDomainErrors.InvitationAlreadyRejected);

        if (UserId != userId)
            return Result.Fail(OrganizationMemberInvitationsDomainErrors.InvitationNotForUser);

        if (ExpiresAt < DateTime.UtcNow)
            return Result.Fail(OrganizationMemberInvitationsDomainErrors.InvitationExpired);

        return Result.Ok();
    }

    public Result Accept(string acceptedByUserId)
    {
        var validationResult = ValidateInvitationStatus(acceptedByUserId);
        if (validationResult.IsFailed)
            return validationResult;

        AcceptedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;

        // TODO: emit event for organization member added, 
        // handler of event will add the user to the organization's member list with the role
        return Result.Ok();
    }

    public Result Reject(string rejectedByUserId)
    {
        var validationResult = ValidateInvitationStatus(rejectedByUserId);
        if (validationResult.IsFailed)
            return validationResult;

        RejectedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
        return Result.Ok();
    }
}
