using System.ComponentModel.DataAnnotations;

namespace IdentityService.Infrastructure.Options;

public class DatabaseOptions
{
    [Required]
    public required string ConnectionString { get; set; }
}