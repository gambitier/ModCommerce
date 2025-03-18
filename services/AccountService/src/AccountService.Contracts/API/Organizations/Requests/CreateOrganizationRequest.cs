using System.ComponentModel.DataAnnotations;

namespace AccountService.Contracts.API.Organizations.Requests;

public class CreateOrganizationRequest
{
    [Required(ErrorMessage = "Name is required")]
    public required string Name { get; init; }

    public string? Description { get; init; }
}
