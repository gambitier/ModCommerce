using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using IdentityService.Domain.Entities;

namespace IdentityService.Infrastructure.Persistence.Entities;

public class User : IdentityUser, IApplicationUser
{
    [Column(TypeName = "varchar(100)")]
    [StringLength(100)]
    public string? FirstName { get; set; }

    [Column(TypeName = "varchar(100)")]
    [StringLength(100)]
    public string? LastName { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
