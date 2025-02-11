using IdentityService.Application.Models.Users;
using IdentityService.Domain.Models;
using Mapster;

namespace IdentityService.Application.Mapping;

[GenerateMapper]
public partial class ApplicationMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        // register all mappings here
        config.NewConfig<UserDomainModel, UserDetailsDto>();
    }
}