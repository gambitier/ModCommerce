using IdentityService.Domain.Interfaces.Repositories;
using IdentityService.Application.Interfaces.Services;
using IdentityService.Application.Models;
using IdentityService.Domain.Interfaces.AuthenticationServices;
using FluentResults;
using MapsterMapper;
using IdentityService.Domain.Interfaces.Communication;
using IdentityService.Domain.Errors;
using Microsoft.Extensions.Options;
using IdentityService.Application.Options;
using IdentityService.Domain.Interfaces.Persistence;
using IdentityService.Domain.Models;

namespace IdentityService.Application.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;
    private readonly IMapper _mapper;
    private readonly IEmailService _emailService;
    private readonly ApplicationUrlOptions _applicationUrlOptions;
    private readonly IUnitOfWork _unitOfWork;

    public AuthenticationService(
        IUserRepository userRepository,
        ITokenService tokenService,
        IMapper mapper,
        IEmailService emailService,
        IOptions<ApplicationUrlOptions> options,
        IUnitOfWork unitOfWork
    )
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
        _mapper = mapper;
        _emailService = emailService;
        _applicationUrlOptions = options.Value;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<AuthResultDto>> AuthenticateAsync(TokenRequestDto dto)
    {
        var isEmailConfirmedResult = await _userRepository.IsEmailConfirmedAsync(dto.UsernameOrEmail);
        if (isEmailConfirmedResult.IsFailed)
            return isEmailConfirmedResult.ToResult<AuthResultDto>();

        if (!isEmailConfirmedResult.Value)
            return Result.Fail(DomainErrors.User.EmailNotConfirmed);

        var pwdCheckResult = await _userRepository.VerifyUserPasswordAsync(dto.UsernameOrEmail, dto.Password);
        if (pwdCheckResult.IsFailed)
            return pwdCheckResult.ToResult<AuthResultDto>();

        var userInfo = pwdCheckResult.Value;

        var tokenResult = await _tokenService.GenerateToken(userInfo.Id, userInfo.Email);
        if (tokenResult.IsFailed)
            return tokenResult.ToResult<AuthResultDto>();

        return Result.Ok(_mapper.Map<AuthResultDto>(tokenResult.Value));
    }

    public async Task<Result> RegisterUserAsync(CreateUserDto dto, string password)
    {
        var result = await _userRepository.CreateAsync(dto.Username, dto.Email, password);
        if (result.IsFailed)
            return result.ToResult();

        await _unitOfWork.SaveChangesAsync();

        return Result.Ok();
    }

    public async Task<Result<AuthResultDto>> RefreshTokenAsync(string refreshToken)
    {
        var result = await _tokenService.RefreshToken(refreshToken);
        if (result.IsFailed)
            return result.ToResult<AuthResultDto>();

        return Result.Ok(_mapper.Map<AuthResultDto>(result.Value));
    }

    public async Task<Result<AuthResultDto>> ConfirmEmailAsync(string email, string token)
    {
        var result = await _userRepository.ConfirmEmailAsync(email, token);
        if (result.IsFailed)
            return result.ToResult<AuthResultDto>();

        return Result.Ok(_mapper.Map<AuthResultDto>(result.Value));
    }

    public async Task<Result> SendConfirmationEmailAsync(string email)
    {
        var userResult = await _userRepository.FindByEmailAsync(email);
        if (userResult.IsFailed)
            return userResult.ToResult();

        if (userResult.Value.EmailConfirmed)
            return Result.Fail(DomainErrors.User.EmailAlreadyConfirmed);

        var tokenResult = await _userRepository.GenerateEmailConfirmationTokenAsync(email);
        if (tokenResult.IsFailed)
            return tokenResult.ToResult();

        var queryParams = new Dictionary<string, string>
        {
            { "token", tokenResult.Value },
            { "email", userResult.Value.Email }
        };

        var uriBuilder = new UriBuilder(_applicationUrlOptions.BaseUrl)
        {
            Path = _applicationUrlOptions.EmailConfirmationPath,
            Query = string.Join("&", queryParams.Select(kvp =>
                $"{Uri.EscapeDataString(kvp.Key)}={Uri.EscapeDataString(kvp.Value)}"))
        };

        var confirmationLink = uriBuilder.Uri;

        await _emailService.SendConfirmationEmailAsync(userResult.Value.Email, userResult.Value.Username, confirmationLink.ToString());

        return Result.Ok();
    }

    public JsonWebKeyInfo[] GetJsonWebKeys()
    {
        return _tokenService.GetJsonWebKeys();
    }
}