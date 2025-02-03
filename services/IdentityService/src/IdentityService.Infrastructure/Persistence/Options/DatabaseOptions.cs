using System.ComponentModel.DataAnnotations;

namespace IdentityService.Infrastructure.Persistence.Options;

public class DatabaseOptions
{
    [Required]
    public required string ConnectionString { get; set; }
}