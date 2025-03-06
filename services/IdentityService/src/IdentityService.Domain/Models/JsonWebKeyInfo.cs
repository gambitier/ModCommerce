namespace IdentityService.Domain.Models;

public class JsonWebKeyInfo
{
    public string Kty { get; set; } = string.Empty;
    public string Kid { get; set; } = string.Empty;
    public string Use { get; set; } = string.Empty;
    public string Alg { get; set; } = string.Empty;
    public string N { get; set; } = string.Empty;
    public string E { get; set; } = string.Empty;
}