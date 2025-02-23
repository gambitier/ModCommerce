using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using IdentityService.Infrastructure.Authentication.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Options;
using IdentityService.Domain.Interfaces.AuthenticationServices;
using IdentityService.Domain.Constants;
using IdentityService.Domain.Models;

namespace IdentityService.Infrastructure.Authentication.Services;

public class TokenService : ITokenService
{
    private readonly JwtOptions _jwtOptions;

    public TokenService(IOptions<JwtOptions> jwtOptions)
    {
        _jwtOptions = jwtOptions.Value;
    }

    public AuthTokenInfo GenerateToken(string userId, string email)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId),
            new Claim(JwtRegisteredClaimNames.Email, email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(_jwtOptions.ExpirationMinutes),
            signingCredentials: creds
        );

        return new AuthTokenInfo(
            AccessToken: new JwtSecurityTokenHandler().WriteToken(token),
            TokenType: TokenType.Bearer,
            ExpiresIn: _jwtOptions.ExpirationMinutes * 60,
            RefreshToken: null,
            Scope: Scopes.ApiAccess
        );
    }
}
