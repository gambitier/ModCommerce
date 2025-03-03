using Mapster;

namespace UserService.Application.Mapping;

[GenerateMapper]
public partial class ApplicationMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        // config.NewConfig<UserDomainModel, UserDetailsDto>();
        // config.NewConfig<AuthTokenInfo, AuthResultDto>();
    }
}
