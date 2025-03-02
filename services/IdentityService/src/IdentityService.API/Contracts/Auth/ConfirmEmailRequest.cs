using System.ComponentModel.DataAnnotations;

namespace IdentityService.API.Contracts.Auth;

public class ConfirmEmailRequest
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    public required string Email { get; init; }

    [Required(ErrorMessage = "Token is required")]
    public required string Token { get; init; }
}
