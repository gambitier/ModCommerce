using AccountService.Contracts.API.Organizations.Enums;

namespace AccountService.Contracts.API.Organizations.Requests;

public class AddOrganizationMemberRequest
{
    public string UserId { get; set; }
    public string OrgId { get; set; }
    public OrganizationRole Role { get; set; }
}