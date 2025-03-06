namespace IdentityService.Domain.Models;

public record JsonWebKey(
    string Kty,
    string Kid,
    string Use,
    string Alg,
    string N,
    string E
);
