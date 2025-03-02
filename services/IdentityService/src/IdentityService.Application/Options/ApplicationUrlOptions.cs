using System.ComponentModel.DataAnnotations;

namespace IdentityService.Application.Options;

public class ApplicationUrlOptions
{
    [Required(ErrorMessage = "BaseUrl is required")]
    [Url(ErrorMessage = "BaseUrl must be a valid URL")]
    public required string BaseUrl { get; init; }

    [Required(ErrorMessage = "EmailConfirmationPath is required")]
    public required string EmailConfirmationPath { get; init; }
}
