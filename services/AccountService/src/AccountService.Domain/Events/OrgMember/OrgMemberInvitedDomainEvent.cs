using AccountService.Domain.Models.Organizations.Enums;
using MediatR;

namespace AccountService.Domain.Events.OrgMember;

public class OrgMemberInvitedDomainEvent : INotification
{
    public Guid InvitationId { get; private set; }
    public string UserId { get; private set; }
    public string InvitedByUserId { get; private set; }
    public Guid OrganizationId { get; private set; }
    public DateTime ExpiresAt { get; private set; }
    public UserOrganizationMembershipRole Role { get; private set; }

    public OrgMemberInvitedDomainEvent(
        Guid invitationId,
        string userId,
        string invitedByUserId,
        Guid organizationId,
        UserOrganizationMembershipRole role,
        DateTime expiresAt)
    {
        InvitationId = invitationId;
        UserId = userId;
        InvitedByUserId = invitedByUserId;
        OrganizationId = organizationId;
        Role = role;
        ExpiresAt = expiresAt;
    }
}
