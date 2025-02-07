using IdentityService.Domain.Interfaces.Repositories;
using IdentityService.Application.Interfaces.Services;
using IdentityService.Application.Models;
using IdentityService.Domain.Interfaces.AuthenticationServices;
using FluentResults;

namespace IdentityService.Application.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;

    public AuthenticationService(
        IUserRepository userRepository,
        ITokenService tokenService)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
    }

    public async Task<Result<AuthResultDto>> AuthenticateAsync(string email, string password)
    {
        var userResult = await _userRepository.FindByEmailAsync(email);
        if (userResult.IsFailed)
            return userResult.ToResult<AuthResultDto>();

        var passwordResult = await _userRepository.CheckPasswordAsync(userResult.Value.Id, password);
        if (passwordResult.IsFailed)
            return passwordResult.ToResult<AuthResultDto>();

        var token = _tokenService.GenerateJwtToken(userResult.Value.Id, email);
        return Result.Ok(new AuthResultDto(token));
    }

    public async Task<Result<AuthResultDto>> RegisterUserAsync(UserDto user, string password)
    {
        var result = await _userRepository.CreateAsync(user.Email, password);
        if (result.IsFailed)
            return result.ToResult<AuthResultDto>();

        var (userId, email) = result.Value;
        var token = _tokenService.GenerateJwtToken(userId, email);
        return Result.Ok(new AuthResultDto(token));
    }
}
