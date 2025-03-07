using System.ComponentModel.DataAnnotations;

namespace UserService.Infrastructure.Authentication.Options;

public class NoTrailingSlashAttribute : ValidationAttribute
{
    public NoTrailingSlashAttribute() : base("The {0} must not end with a forward slash (/)")
    {
    }

    public NoTrailingSlashAttribute(string errorMessage) : base(errorMessage)
    {
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is string str && str.EndsWith("/"))
        {
            return new ValidationResult(
                FormatErrorMessage(validationContext.DisplayName),
                new[] { validationContext.MemberName! });
        }
        return ValidationResult.Success;
    }
}

public class NoLeadingSlashAttribute : ValidationAttribute
{
    public NoLeadingSlashAttribute() : base("The {0} must not start with a forward slash (/)")
    {
    }

    public NoLeadingSlashAttribute(string errorMessage) : base(errorMessage)
    {
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is string str && str.StartsWith("/"))
        {
            return new ValidationResult(
                FormatErrorMessage(validationContext.DisplayName),
                new[] { validationContext.MemberName! });
        }
        return ValidationResult.Success;
    }
}

public class JwtOptions
{
    [Required(ErrorMessage = "Authority URL is required.")]
    [Url(ErrorMessage = "Authority must be a valid URL.")]
    [NoTrailingSlash(ErrorMessage = "Authority URL must not end with a forward slash (/).")]
    public required string Authority { get; init; }

    [Required(ErrorMessage = "JWKS URL path is required.")]
    [NoLeadingSlash(ErrorMessage = "JWKS URL path must not start with a forward slash (/).")]
    public required string JWKSUrlPath { get; init; }

    [Required(ErrorMessage = "ValidIssuer is required.")]
    public required string ValidIssuer { get; init; }

    [Required(ErrorMessage = "ValidAudience is required.")]
    public required string ValidAudience { get; init; }
}
