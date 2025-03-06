using System.ComponentModel.DataAnnotations;

namespace IdentityService.Infrastructure.Authentication.Options;

public class JwtOptions
{
    [Required(ErrorMessage = "KeyId is required")]
    [StringLength(int.MaxValue, MinimumLength = 1, ErrorMessage = "KeyId cannot be empty")]
    public required string KeyId { get; set; }

    [Required(ErrorMessage = "PrivateKeyPem is required")]
    [StringLength(int.MaxValue, MinimumLength = 1, ErrorMessage = "PrivateKeyPem cannot be empty")]
    public required string PrivateKeyPem { get; set; }

    [Required(ErrorMessage = "PublicKeyPem is required")]
    [StringLength(int.MaxValue, MinimumLength = 1, ErrorMessage = "PublicKeyPem cannot be empty")]
    public required string PublicKeyPem { get; set; }

    [Required(ErrorMessage = "Issuer is required")]
    [StringLength(int.MaxValue, MinimumLength = 1, ErrorMessage = "Issuer cannot be empty")]
    public required string Issuer { get; set; }

    [Required(ErrorMessage = "Audience is required")]
    [StringLength(int.MaxValue, MinimumLength = 1, ErrorMessage = "Audience cannot be empty")]
    public required string Audience { get; set; }

    [Required(ErrorMessage = "ExpirationMinutes is required")]
    [Range(1, int.MaxValue, ErrorMessage = "ExpirationMinutes must be greater than 0")]
    public required int ExpirationMinutes { get; set; }

    [Required(ErrorMessage = "RefreshToken configuration is required")]
    public required RefreshTokenOptions RefreshToken { get; set; }
}

public class RefreshTokenOptions
{
    [Required(ErrorMessage = "ExpirationDays is required")]
    [Range(1, int.MaxValue, ErrorMessage = "ExpirationDays must be greater than 0")]
    public required int ExpirationDays { get; set; }
}
