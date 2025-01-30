namespace IdentityService.Application.Models;

public class AuthResultDto
{
    public bool Succeeded { get; set; }
    public string? Token { get; set; }
    public IEnumerable<string> Errors { get; set; } = Array.Empty<string>();
}
