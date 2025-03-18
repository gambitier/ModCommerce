using AccountService.Domain.Models.Organizations.DomainModels;
using AccountService.Domain.Models.Organizations.Dtos;
using FluentResults;
namespace AccountService.Domain.Interfaces.Repositories;

public interface IOrganizationRepository
{
    Task<Result<Guid>> AddAsync(CreateOrganizationDomainModel domainModel);
    Task<Result<OrganizationDto>> GetByIdAsync(Guid id);
}
