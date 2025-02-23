using FluentResults;

using IdentityService.Domain.Entities;

namespace IdentityService.Domain.Interfaces.Repositories;

public interface IRefreshTokenRepository
{
    Task<Result<RefreshToken>> CreateAsync(string userId, TimeSpan lifetime);
    Task<Result<RefreshToken>> FindByTokenAsync(string token);
    Task<Result> RevokeAsync(string token);
}
