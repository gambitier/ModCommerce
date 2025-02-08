using FluentResults;
using IdentityService.Application.Interfaces.Services;
using IdentityService.Application.Models.Users;
using IdentityService.Domain.Interfaces.Repositories;

namespace IdentityService.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result<UserDetailsDto>> GetCurrentUserAsync(string email)
    {
        var result = await _userRepository.FindByEmailAsync(email);
        if (result.IsFailed)
            return result.ToResult<UserDetailsDto>();

        return Result.Ok(new UserDetailsDto
        {
            Id = result.Value.Id,
            Email = result.Value.Email,
            CreatedAt = result.Value.CreatedAt
        });
    }
}
