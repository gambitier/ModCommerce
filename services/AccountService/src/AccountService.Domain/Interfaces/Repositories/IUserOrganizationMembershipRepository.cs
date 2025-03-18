using AccountService.Domain.Models.Organizations.DomainModels;
using FluentResults;
namespace AccountService.Domain.Interfaces.Repositories;

public interface IUserOrganizationMembershipRepository
{
    Task<Result<Guid>> AddAsync(CreateOrganizationMembershipRoleDomainModel domainModel);
    Task<Result> UpdateRoleAsync(UpdateOrganizationMembershipRoleDomainModel domainModel);
}
