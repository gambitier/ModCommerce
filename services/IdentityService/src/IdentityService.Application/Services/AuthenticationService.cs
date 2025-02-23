using IdentityService.Domain.Interfaces.Repositories;
using IdentityService.Application.Interfaces.Services;
using IdentityService.Application.Models;
using IdentityService.Domain.Interfaces.AuthenticationServices;
using FluentResults;
using MapsterMapper;

namespace IdentityService.Application.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;
    private readonly IMapper _mapper;

    public AuthenticationService(
        IUserRepository userRepository,
        ITokenService tokenService,
        IMapper mapper)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
        _mapper = mapper;
    }

    public async Task<Result<AuthResultDto>> AuthenticateAsync(LoginUserDto user)
    {
        var pwdCheckResult = await _userRepository.VerifyUserPasswordAsync(user.Email, user.Password);
        if (pwdCheckResult.IsFailed)
            return pwdCheckResult.ToResult<AuthResultDto>();

        var userInfo = pwdCheckResult.Value;

        var tokenResult = await _tokenService.GenerateToken(userInfo.Id, userInfo.Email);
        if (tokenResult.IsFailed)
            return tokenResult.ToResult<AuthResultDto>();

        return Result.Ok(_mapper.Map<AuthResultDto>(tokenResult.Value));
    }

    public async Task<Result<AuthResultDto>> RegisterUserAsync(CreateUserDto user, string password)
    {
        var result = await _userRepository.CreateAsync(user.Email, password);
        if (result.IsFailed)
            return result.ToResult<AuthResultDto>();

        var tokenResult = await _tokenService.GenerateToken(result.Value.Id, result.Value.Email);
        if (tokenResult.IsFailed)
            return tokenResult.ToResult<AuthResultDto>();

        return Result.Ok(_mapper.Map<AuthResultDto>(tokenResult.Value));
    }

    public async Task<Result<AuthResultDto>> RefreshTokenAsync(string refreshToken)
    {
        var result = await _tokenService.RefreshToken(refreshToken);
        if (result.IsFailed)
            return result.ToResult<AuthResultDto>();

        return Result.Ok(_mapper.Map<AuthResultDto>(result.Value));
    }
}
