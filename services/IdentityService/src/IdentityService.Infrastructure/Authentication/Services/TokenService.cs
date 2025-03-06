using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using IdentityService.Infrastructure.Authentication.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Options;
using IdentityService.Domain.Interfaces.AuthenticationServices;
using IdentityService.Domain.Constants;
using IdentityService.Domain.Models;
using IdentityService.Domain.Interfaces.Repositories;
using FluentResults;
using IdentityService.Domain.Interfaces.Persistence;

namespace IdentityService.Infrastructure.Authentication.Services;

public class TokenService : ITokenService
{
    private readonly JwtOptions _jwtOptions;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly RsaSecurityKey _privateKey;
    private readonly RsaSecurityKey _publicKey;

    public TokenService(
        IOptions<JwtOptions> jwtOptions,
        IRefreshTokenRepository refreshTokenRepository,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork)
    {
        _jwtOptions = jwtOptions.Value;
        _refreshTokenRepository = refreshTokenRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;

        // Initialize RSA keys
        using var privateRsa = RSA.Create();
        privateRsa.ImportFromPem(_jwtOptions.PrivateKeyPem);
        _privateKey = new RsaSecurityKey(privateRsa.ExportParameters(true));
        _privateKey.KeyId = _jwtOptions.KeyId;

        using var publicRsa = RSA.Create();
        publicRsa.ImportFromPem(_jwtOptions.PublicKeyPem);
        _publicKey = new RsaSecurityKey(publicRsa.ExportParameters(false));
        _publicKey.KeyId = _jwtOptions.KeyId;
    }

    public async Task<Result<AuthTokenInfo>> GenerateToken(string userId, string email)
    {
        var refreshTokenResult = await _refreshTokenRepository.CreateAsync(
            userId,
            TimeSpan.FromDays(_jwtOptions.RefreshToken.ExpirationDays)
        );
        if (refreshTokenResult.IsFailed)
            return refreshTokenResult.ToResult<AuthTokenInfo>();

        await _unitOfWork.SaveChangesAsync();

        return Result.Ok(new AuthTokenInfo(
            AccessToken: GenerateAccessToken(userId, email),
            TokenType: TokenType.Bearer,
            ExpiresIn: _jwtOptions.ExpirationMinutes * 60,
            RefreshToken: refreshTokenResult.Value.Token,
            Scope: Scopes.ApiAccess
        ));
    }

    public async Task<Result<AuthTokenInfo>> RefreshToken(string refreshToken)
    {
        var tokenResult = await _refreshTokenRepository.FindByTokenAsync(refreshToken);
        if (tokenResult.IsFailed)
            return tokenResult.ToResult<AuthTokenInfo>();

        var userResult = await _userRepository.FindByIdAsync(tokenResult.Value.UserId);
        if (userResult.IsFailed)
            return userResult.ToResult<AuthTokenInfo>();

        var revokeResult = await _refreshTokenRepository.RevokeAsync(refreshToken);
        if (revokeResult.IsFailed)
            return revokeResult.ToResult<AuthTokenInfo>();

        var newTokenResult = await _refreshTokenRepository.CreateAsync(
            tokenResult.Value.UserId,
            TimeSpan.FromDays(_jwtOptions.RefreshToken.ExpirationDays)
        );
        if (newTokenResult.IsFailed)
            return newTokenResult.ToResult<AuthTokenInfo>();

        await _unitOfWork.SaveChangesAsync();

        return Result.Ok(new AuthTokenInfo(
            AccessToken: GenerateAccessToken(userResult.Value.Id, userResult.Value.Email),
            TokenType: TokenType.Bearer,
            ExpiresIn: _jwtOptions.ExpirationMinutes * 60,
            RefreshToken: newTokenResult.Value.Token,
            Scope: Scopes.ApiAccess
        ));
    }

    private string GenerateAccessToken(string userId, string email)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId),
            new Claim(JwtRegisteredClaimNames.Email, email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("kid", _jwtOptions.KeyId)
        };

        var creds = new SigningCredentials(_privateKey, SecurityAlgorithms.RsaSha256);
        var jwt = new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtOptions.ExpirationMinutes),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }

    public JsonWebKeyInfo GetJsonWebKey()
    {
        var jwk = new JsonWebKey
        {
            Kty = JsonWebAlgorithmsKeyTypes.RSA,
            Kid = _jwtOptions.KeyId,
            Use = "sig",
            Alg = SecurityAlgorithms.RsaSha256,
            N = Base64UrlEncoder.Encode(_publicKey.Parameters.Modulus),
            E = Base64UrlEncoder.Encode(_publicKey.Parameters.Exponent)
        };

        return new JsonWebKeyInfo
        {
            Kty = jwk.Kty,
            Kid = jwk.Kid,
            Use = jwk.Use!,
            Alg = jwk.Alg,
            N = jwk.N,
            E = jwk.E
        };
    }
}
