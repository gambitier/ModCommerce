namespace AccountService.Domain.Errors;

public static class OrganizationMemberInvitationsDomainErrors
{
    public static NotFoundError OrganizationInvitationNotFound => new("OrganizationMemberInvitations.NotFound",
        "Organization member invitation not found");

    public static ConflictError InvitationExists => new("OrganizationMemberInvitations.InvitationExists",
        "Invitation already exists");

    public static ConflictError InvitationAlreadyAccepted => new("OrganizationMemberInvitations.InvitationAlreadyAccepted",
        "Invitation already accepted");

    public static ConflictError InvitationAlreadyRejected => new("OrganizationMemberInvitations.InvitationAlreadyRejected",
        "Invitation already rejected");

    public static ConflictError InvitationExpired => new("OrganizationMemberInvitations.InvitationExpired",
        "Invitation expired");

    public static UnauthorizedError InvitationNotForUser => new("OrganizationMemberInvitations.InvitationNotForUser",
        "Invitation is not for the user");
}
