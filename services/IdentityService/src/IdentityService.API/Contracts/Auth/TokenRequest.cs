using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace IdentityService.API.Contracts.Auth;

public class TokenRequest
{
    private string usernameOrEmail = string.Empty;
    private string password = string.Empty;

    [Required(ErrorMessage = "Username or email is required")]
    [StringLength(100, ErrorMessage = "Username or email cannot exceed 100 characters")]
    [JsonPropertyName("username")]
    public string UsernameOrEmail
    {
        get => usernameOrEmail;
        set => usernameOrEmail = value?.Trim().ToLowerInvariant() ?? string.Empty;
    }

    [Required(ErrorMessage = "Password is required")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 100 characters")]
    public string Password
    {
        get => password;
        set => password = value?.Trim() ?? string.Empty;
    }
}
