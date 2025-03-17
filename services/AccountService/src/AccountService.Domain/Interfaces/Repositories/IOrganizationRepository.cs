using AccountService.Domain.Models.Organizations.DomainModels;
using AccountService.Domain.Models.Organizations.Dtos;
namespace AccountService.Domain.Interfaces.Repositories;

public interface IOrganizationRepository
{
    Task<Guid> AddAsync(CreateOrganizationDomainModel createOrganizationDomainModel);
    Task<OrganizationDto> GetByIdAsync(Guid id);
}
