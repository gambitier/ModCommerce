namespace IdentityService.Application.Models;

public class AuthResultDto
{
    public bool Succeeded { get; set; }
    public string? Token { get; set; }
    public IEnumerable<string> Errors { get; set; } = Array.Empty<string>();

    public static AuthResultDto Success(string token) => new AuthResultDto { Succeeded = true, Token = token };
    public static AuthResultDto Failure(string error) => new AuthResultDto { Succeeded = false, Errors = new[] { error } };
}
