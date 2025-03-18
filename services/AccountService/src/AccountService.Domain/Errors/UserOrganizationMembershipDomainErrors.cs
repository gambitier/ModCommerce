namespace AccountService.Domain.Errors;

public static class UserOrganizationMembershipDomainErrors
{
    public static NotFoundError OrganizationMembershipNotFound => new("OrganizationMembership.NotFound",
        "Organization membership not found");

    public static ConflictError UserAlreadyMemberOfOrganization => new("OrganizationMembership.UserAlreadyMemberOfOrganization",
        "User is already a member of the organization");
}
