using System.ComponentModel.DataAnnotations;

namespace IdentityService.Infrastructure.Authentication.Options;

public class JwtOptions
{
    [Required]
    public required string Secret { get; set; }

    [Required]
    public required string Issuer { get; set; }

    [Required]
    public required string Audience { get; set; }

    [Range(1, 1440, ErrorMessage = "ExpirationMinutes must be between 1 and 1440.")]
    public required int ExpirationMinutes { get; set; } = 30;

    [Required]
    public required RefreshTokenOptions RefreshToken { get; set; } = new();
}

public class RefreshTokenOptions
{
    [Range(1, 365, ErrorMessage = "ExpirationDays must be between 1 and 365.")]
    public int ExpirationDays { get; set; } = 30;

    public bool UseRotation { get; set; } = true;
}
