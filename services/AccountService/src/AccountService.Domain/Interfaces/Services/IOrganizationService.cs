using AccountService.Domain.Models.Organizations.DomainModels;
using AccountService.Domain.Models.Organizations.Dtos;
using FluentResults;
namespace AccountService.Domain.Interfaces.Services;

public interface IOrganizationService
{
    Task<Result<Guid>> CreateOrganizationAsync(CreateOrganizationDomainModel createOrganizationDomainModel);
    Task<OrganizationDto> GetByIdAsync(Guid id);
}
