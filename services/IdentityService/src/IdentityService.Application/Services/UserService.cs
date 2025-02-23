using FluentResults;
using IdentityService.Application.Interfaces.Services;
using IdentityService.Application.Models.Users;
using IdentityService.Domain.Interfaces.Repositories;
using MapsterMapper;

namespace IdentityService.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public UserService(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<Result<UserDetailsDto>> GetCurrentUserAsync(string email)
    {
        var result = await _userRepository.FindByEmailAsync(email);
        if (result.IsFailed)
            return result.ToResult<UserDetailsDto>();

        return Result.Ok(_mapper.Map<UserDetailsDto>(result.Value));
    }

    public async Task<Result<IEnumerable<UserDetailsDto>>> GetAllUsersAsync()
    {
        var result = await _userRepository.GetAllAsync();
        if (result.IsFailed)
            return result.ToResult<IEnumerable<UserDetailsDto>>();

        return Result.Ok(_mapper.Map<IEnumerable<UserDetailsDto>>(result.Value));
    }
}
