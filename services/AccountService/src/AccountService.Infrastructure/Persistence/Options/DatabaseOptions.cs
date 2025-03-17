using System.ComponentModel.DataAnnotations;

namespace AccountService.Infrastructure.Persistence.Options;

public class DatabaseOptions
{
    [Required]
    public required string ConnectionString { get; set; }
}