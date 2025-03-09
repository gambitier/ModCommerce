using System.ComponentModel.DataAnnotations;

namespace UserService.Infrastructure.Persistence.Options;

public class DatabaseOptions
{
    [Required]
    public required string ConnectionString { get; set; }
}