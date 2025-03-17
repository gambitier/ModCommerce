namespace AccountService.Domain.Models.Organizations.DomainModels;

public class CreateOrganizationDomainModel
{
    public required string Name { get; init; }
    public string? Description { get; init; }
}