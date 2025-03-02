using System.ComponentModel.DataAnnotations;

namespace IdentityService.API.Contracts.Auth;

public class ResendEmailConfirmationRequest
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    public required string Email { get; init; }
}
