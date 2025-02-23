using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace IdentityService.API.Contracts.Auth;

public class TokenRequest
{
    [Required(ErrorMessage = "Username or email is required")]
    [StringLength(100, ErrorMessage = "Username or email cannot exceed 100 characters")]
    [JsonPropertyName("username")]
    public required string UsernameOrEmail { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 100 characters")]
    public string Password { get; set; } = string.Empty;
}
