using System.ComponentModel.DataAnnotations;

namespace IdentityService.Contracts.API.Auth.Requests;

public class ResendEmailConfirmationRequest
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    public required string Email { get; init; }
}
