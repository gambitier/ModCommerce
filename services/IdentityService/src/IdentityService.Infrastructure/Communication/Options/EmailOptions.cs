using System.ComponentModel.DataAnnotations;

namespace IdentityService.Infrastructure.Communication.Options;

public class EmailOptions
{
    [Required]
    public string SmtpServer { get; init; } = null!;

    [Required, Range(1, 65535)]
    public int SmtpPort { get; init; }

    [Required, EmailAddress]
    public string SenderEmail { get; init; } = null!;

    [Required]
    public string SenderName { get; init; } = null!;

    [Required]
    public string SmtpUsername { get; init; } = null!;

    [Required]
    public string SmtpPassword { get; init; } = null!;
}