using AccountService.Domain.Models.Organizations.DomainModels;
using AccountService.Domain.Models.Organizations.Dtos;
using FluentResults;
namespace AccountService.Domain.Interfaces.Services;

public interface IOrganizationService
{
    Task<Result<Guid>> CreateOrganizationAsync(string userId, CreateOrganizationDomainModel domainModel);
    Task<Result<OrganizationDto>> GetByIdAsync(Guid id);
    Task<Result> InviteMemberAsync(string invitedByUserId, InviteOrganizationMemberDomainModel domainModel);
    Task<Result> AcceptOrganizationInvitationAsync(string userId, Guid invitationId);
}
