using System.ComponentModel.DataAnnotations;

namespace UserService.Infrastructure.Authentication.Options;

public class JwtOptions
{
    /// <summary>
    /// The authority URL.
    /// Note
    /// - it should not include the trailing slash (correct: "https://auth.example.com", incorrect: "https://auth.example.com/").
    /// </summary>
    [Required(ErrorMessage = "Authority URL is required.")]
    [Url(ErrorMessage = "Authority must be a valid URL.")]
    public required string Authority { get; init; }

    /// <summary>
    /// The path to the JWKS endpoint.
    /// Note 
    /// - its relative to the authority URL.
    /// - it should not include the leading slash (correct: ".well-known/jwks.json", incorrect: "/well-known/jwks.json").
    /// </summary>
    [Required(ErrorMessage = "JWKS URL path is required.")]
    public required string JWKSUrlPath { get; init; }

    [Required(ErrorMessage = "ValidIssuer is required.")]
    public required string ValidIssuer { get; init; }

    [Required(ErrorMessage = "ValidAudience is required.")]
    public required string ValidAudience { get; init; }
}
