using AccountService.Domain.Models.Organizations.Enums;

namespace AccountService.Domain.Models.Organizations.DomainModels;

/// <summary>
/// A domain model for inviting a user to an organization.
/// </summary>
public class InviteOrganizationMemberDomainModel
{
    /// <summary>
    /// The user id of the user to invite to the organization.
    /// </summary>
    public required string UserId { get; init; }

    /// <summary>
    /// The id of the organization to invite the user to.
    /// </summary>
    public required Guid OrganizationId { get; init; }

    /// <summary>
    /// The role of the user in the organization.
    /// </summary>
    public required UserOrganizationMembershipRole Role { get; init; }

    public void Deconstruct(
        out string userId,
        out Guid organizationId,
        out UserOrganizationMembershipRole role)
    {
        userId = UserId;
        organizationId = OrganizationId;
        role = Role;
    }
}
