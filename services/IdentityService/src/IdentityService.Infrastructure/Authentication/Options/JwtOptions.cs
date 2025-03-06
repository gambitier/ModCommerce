using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Options;

namespace IdentityService.Infrastructure.Authentication.Options;

public class JwtOptions
{
    [Required(ErrorMessage = "Keys configuration is required")]
    public required List<JwtKeyConfig> Keys { get; set; }

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

    public ValidateOptionsResult Validate(string? name, JwtOptions options)
    {
        var failures = new List<string>();

        if (options.Keys == null || options.Keys.Count == 0)
        {
            failures.Add("At least one key must be configured");
        }
        else
        {
            var activeKeys = options.Keys.Count(k => k.IsActive);
            if (activeKeys == 0)
            {
                failures.Add("At least one key must be marked as active");
            }
            else if (activeKeys > 1)
            {
                failures.Add($"Multiple keys are marked as active ({activeKeys}). Only one key can be active at a time.");
            }
        }

        return failures.Count > 0
            ? ValidateOptionsResult.Fail(failures)
            : ValidateOptionsResult.Success;
    }
}

public class JwtKeyConfig
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

    [Required(ErrorMessage = "IsActive is required")]
    public required bool IsActive { get; set; }
}

public class RefreshTokenOptions
{
    [Required(ErrorMessage = "ExpirationDays is required")]
    [Range(1, int.MaxValue, ErrorMessage = "ExpirationDays must be greater than 0")]
    public required int ExpirationDays { get; set; }
}
