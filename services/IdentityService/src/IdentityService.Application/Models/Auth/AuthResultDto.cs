using IdentityService.Domain.Constants;

public class AuthResultDto
{
    public required string AccessToken { get; set; }
    public required int ExpiresIn { get; set; }
    public required TokenType TokenType { get; set; }
    public string? RefreshToken { get; set; }
    public string? Scope { get; set; }
}
