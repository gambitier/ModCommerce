using IdentityService.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using IdentityService.Infrastructure.Persistence.Mappings;
using System.Security.Cryptography;
using FluentResults;
using RefreshTokenDomain = IdentityService.Domain.Entities.RefreshToken;
using RefreshTokenEntity = IdentityService.Infrastructure.Persistence.Entities.RefreshToken;
using IdentityService.Domain.Errors;
using Microsoft.AspNetCore.WebUtilities;

namespace IdentityService.Infrastructure.Persistence.Repositories;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly IdentityDbContext _context;

    public RefreshTokenRepository(IdentityDbContext context)
    {
        _context = context;
    }

    public async Task<Result<RefreshTokenDomain>> CreateAsync(string userId, TimeSpan lifetime)
    {
        var refreshToken = new RefreshTokenEntity
        {
            Token = GenerateUniqueToken(),
            UserId = userId,
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.Add(lifetime),
            IsRevoked = false
        };

        await _context.RefreshTokens.AddAsync(refreshToken);

        return Result.Ok(refreshToken.ToDomain());
    }

    public async Task<Result<RefreshTokenDomain>> FindByTokenAsync(string token)
    {
        var entity = await _context.RefreshTokens
            .FirstOrDefaultAsync(rt => rt.Token == token);

        if (entity == null || entity.IsRevoked || entity.ExpiresAt < DateTime.UtcNow)
            return Result.Fail(DomainErrors.Authentication.InvalidRefreshToken);

        return Result.Ok(entity.ToDomain());
    }

    public async Task<Result> RevokeAsync(string token)
    {
        var rowsAffected = await _context.RefreshTokens
            .Where(rt => rt.Token == token)
            .ExecuteUpdateAsync(s =>
                s.SetProperty(rt => rt.IsRevoked, true));

        return rowsAffected > 0
            ? Result.Ok()
            : Result.Fail(DomainErrors.Authentication.InvalidRefreshToken);
    }

    private static string GenerateUniqueToken()
    {
        // According to RFC 6749 Section 10.10, the token needs 
        // to have sufficient entropy (at least 128 bits) to prevent guessing.
        var randomBytes = new byte[32];
        RandomNumberGenerator.Fill(randomBytes);
        return WebEncoders.Base64UrlEncode(randomBytes);
    }
}