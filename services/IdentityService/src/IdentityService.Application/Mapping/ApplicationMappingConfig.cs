using IdentityService.Application.Models.Users;
using IdentityService.Domain.Models;
using Mapster;

namespace IdentityService.Application.Mapping;

[GenerateMapper]
public partial class ApplicationMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<User, UserDetailsDto>();
        config.NewConfig<AuthToken, AuthResultDto>();
    }
}
