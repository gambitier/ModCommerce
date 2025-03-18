namespace AccountService.Domain.Errors;

public static class OrganizationDomainErrors
{
    public static NotFoundError OrganizationNotFound => new("Organization.NotFound",
        "Organization not found");
}
