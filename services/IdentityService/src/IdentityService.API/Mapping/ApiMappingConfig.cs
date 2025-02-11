using IdentityService.API.Contracts.Auth;
using IdentityService.API.Contracts.Users;
using IdentityService.Application.Models;
using IdentityService.Application.Models.Users;
using Mapster;

namespace IdentityService.API.Mapping;

[GenerateMapper]
public partial class ApiMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<RegisterRequest, CreateUserDto>();
        config.NewConfig<LoginRequest, LoginUserDto>();
        config.NewConfig<AuthResultDto, AuthResponse>();
        config.NewConfig<UserDetailsDto, UserResponse>();
    }
}
