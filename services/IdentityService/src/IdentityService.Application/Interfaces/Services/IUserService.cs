using FluentResults;
using IdentityService.Application.Models.Users;

namespace IdentityService.Application.Interfaces.Services;

public interface IUserService
{
    Task<Result<UserDetailsDto>> GetCurrentUserAsync(string email);
}
