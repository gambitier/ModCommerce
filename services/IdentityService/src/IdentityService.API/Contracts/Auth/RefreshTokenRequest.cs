using System.ComponentModel.DataAnnotations;

namespace IdentityService.API.Contracts.Auth;

public class RefreshTokenRequest
{
    [Required(ErrorMessage = "Refresh token required")]
    public string RefreshToken { get; set; } = string.Empty;
}
