namespace AccountService.Contracts.API.Organizations.Requests;

public class CreateOrganizationRequest
{
    public required string Name { get; init; }
    public string? Description { get; init; }
}
