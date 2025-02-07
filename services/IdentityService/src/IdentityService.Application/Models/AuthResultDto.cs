namespace IdentityService.Application.Models;

public class AuthResultDto
{
    public string? Token { get; set; }

    public AuthResultDto(string token)
    {
        Token = token;
    }
}
