using IdentityService.Domain.Interfaces.Repositories;
using IdentityService.Application.Interfaces.Services;
using IdentityService.Application.Models;
using IdentityService.Domain.Interfaces.AuthenticationServices;
using FluentResults;
using MapsterMapper;
using IdentityService.Domain.Interfaces.Persistence;

namespace IdentityService.Application.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public AuthenticationService(
        IUserRepository userRepository,
        ITokenService tokenService,
        IMapper mapper,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<AuthResultDto>> AuthenticateAsync(TokenRequestDto dto)
    {
        var pwdCheckResult = await _userRepository.VerifyUserPasswordAsync(dto.UsernameOrEmail, dto.Password);
        if (pwdCheckResult.IsFailed)
            return pwdCheckResult.ToResult<AuthResultDto>();

        var userInfo = pwdCheckResult.Value;

        var tokenResult = await _tokenService.GenerateToken(userInfo.Id, userInfo.Email);
        if (tokenResult.IsFailed)
            return tokenResult.ToResult<AuthResultDto>();

        return Result.Ok(_mapper.Map<AuthResultDto>(tokenResult.Value));
    }

    public async Task<Result<AuthResultDto>> RegisterUserAsync(CreateUserDto dto, string password)
    {
        return await _unitOfWork.ExecuteTransactionAsync(async () => await CreateUserWithTokenAsync(dto, password));
    }

    private async Task<Result<AuthResultDto>> CreateUserWithTokenAsync(CreateUserDto dto, string password)
    {
        var result = await _userRepository.CreateAsync(dto.Username, dto.Email, password);
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
