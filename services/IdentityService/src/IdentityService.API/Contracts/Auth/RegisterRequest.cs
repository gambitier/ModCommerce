using System.ComponentModel.DataAnnotations;

namespace IdentityService.API.Contracts.Auth;

public class RegisterRequest
{
    private string username = string.Empty;
    private string email = string.Empty;
    private string password = string.Empty;

    [Required(ErrorMessage = "Username is required")]
    [StringLength(100, ErrorMessage = "Username cannot exceed 100 characters")]
    [RegularExpression(@"^[a-zA-Z0-9_-]+$", ErrorMessage = "Username can only contain letters, numbers, underscores and hyphens")]
    public required string Username
    {
        get => username;
        set => username = value?.Trim().ToLowerInvariant() ?? string.Empty;
    }

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    [StringLength(100, ErrorMessage = "Email cannot exceed 100 characters")]
    public required string Email
    {
        get => email;
        set => email = value?.Trim().ToLowerInvariant() ?? string.Empty;
    }

    [Required(ErrorMessage = "Password is required")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 100 characters")]
    public required string Password
    {
        get => password;
        set => password = value?.Trim() ?? string.Empty;
    }
}
