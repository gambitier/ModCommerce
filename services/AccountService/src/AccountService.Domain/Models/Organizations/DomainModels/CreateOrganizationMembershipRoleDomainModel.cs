using AccountService.Domain.Models.Organizations.Enums;

namespace AccountService.Domain.Models.Organizations.DomainModels;

public class CreateOrganizationMembershipRoleDomainModel
{
    public required string UserId { get; init; }
    public required Guid OrganizationId { get; init; }
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
