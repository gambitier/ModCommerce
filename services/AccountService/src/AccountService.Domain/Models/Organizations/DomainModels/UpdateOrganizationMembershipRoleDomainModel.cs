using AccountService.Domain.Models.Organizations.Enums;

namespace AccountService.Domain.Models.Organizations.DomainModels;

public class UpdateOrganizationMembershipRoleDomainModel
{
    public required string UserId { get; init; }
    public required Guid OrganizationId { get; init; }
    public required UserOrganizationMembershipRole Role { get; init; }
}
