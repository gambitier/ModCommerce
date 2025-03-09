using System.ComponentModel.DataAnnotations;

namespace IdentityService.Contracts.API.Auth.Requests;

public class RefreshTokenRequest
{
    [Required(ErrorMessage = "Refresh token required")]
    public string RefreshToken { get; set; } = string.Empty;
}
