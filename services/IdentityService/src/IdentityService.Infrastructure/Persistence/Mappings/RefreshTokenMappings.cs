using RefreshTokenDomain = IdentityService.Domain.Entities.RefreshToken;
using RefreshTokenEntity = IdentityService.Infrastructure.Persistence.Entities.RefreshToken;

namespace IdentityService.Infrastructure.Persistence.Mappings;

public static class RefreshTokenMappings
{
    public static RefreshTokenDomain ToDomain(this RefreshTokenEntity entity)
    {
        return new RefreshTokenDomain
        {
            Token = entity.Token,
            UserId = entity.UserId,
            ExpiresAt = entity.ExpiresAt,
            CreatedAt = entity.CreatedAt,
            IsRevoked = entity.IsRevoked
        };
    }

    public static RefreshTokenEntity ToEntity(this RefreshTokenDomain domain)
    {
        return new RefreshTokenEntity
        {
            Token = domain.Token,
            UserId = domain.UserId,
            ExpiresAt = domain.ExpiresAt,
            CreatedAt = domain.CreatedAt,
            IsRevoked = domain.IsRevoked
        };
    }
}
