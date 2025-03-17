using Mapster;
using AccountService.Domain.Models.Organizations.DomainModels;
using AccountService.Contracts.API.Organizations.Requests;

namespace AccountService.API.Mapping;

[GenerateMapper]
public partial class ApiMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreateOrganizationDomainModel, CreateOrganizationRequest>();
    }
}
