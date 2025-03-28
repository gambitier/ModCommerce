using Mapster;
using AccountService.Domain.Models.Organizations.DomainModels;
using AccountService.Contracts.API.Organizations.Requests;
using AccountService.Domain.Models.Organizations.Enums;

namespace AccountService.API.Mapping;

[GenerateMapper]
public partial class ApiMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreateOrganizationDomainModel, CreateOrganizationRequest>();

        config.NewConfig<(AddOrganizationMemberRequest request, Guid orgId), AddToOrganizationMemberDomainModel>()
            .Map(dest => dest.UserId, src => src.request.UserId)
            .Map(dest => dest.OrganizationId, src => src.orgId)
            .Map(dest => dest.Role, src => (UserOrganizationMembershipRole)src.request.Role);
    }
}
