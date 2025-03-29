using AccountService.Domain.Models.Organizations.Enums;

namespace AccountService.Domain.Models.Organizations.DomainModels;

/// <summary>
/// A domain model for adding a user to an organization.
/// </summary>
public class AddOrganizationMemberDomainModel
{
    /// <summary>
    /// The user id of the user to add to the organization.
    /// </summary>
    public required string UserId { get; init; }

    /// <summary>
    /// The id of the organization to add the user to.
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
