using AccountService.Domain.Models.Organizations.DomainModels;
using AccountService.Domain.Models.Organizations.Enums;

namespace AccountService.Infrastructure.Persistence.Entities;

/// <summary>
/// Represents a user's membership in an organization database entity.
/// </summary>
public class UserOrganizationMembership
{
    public Guid Id { get; private set; }
    public string UserId { get; private set; }
    public Guid OrganizationId { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public UserOrganizationMembershipRole Role { get; private set; }

    private UserOrganizationMembership(
        string userId,
        Guid organizationId,
        UserOrganizationMembershipRole role)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        OrganizationId = organizationId;
        Role = role;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public static UserOrganizationMembership Create(CreateOrganizationMembershipRoleDomainModel createOrganizationMembershipRoleDomainModel)
    {
        var (userId, organizationId, role) = createOrganizationMembershipRoleDomainModel;
        return new UserOrganizationMembership(userId, organizationId, role);
    }

    public void UpdateRole(UserOrganizationMembershipRole role)
    {
        Role = role;
        UpdatedAt = DateTime.UtcNow;
    }
}
