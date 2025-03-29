using AccountService.Domain.Models.Organizations.DomainModels;
using FluentResults;
namespace AccountService.Domain.Interfaces.Repositories;

public interface IUserOrganizationMembershipRepository
{
    Task<Result<Guid>> AddMemberAsync(AddOrganizationMemberDomainModel domainModel);
    Task<Result> InviteMemberAsync(string invitedByUserId, InviteOrganizationMemberDomainModel domainModel);
    Task<Result> AcceptOrganizationInvitationAsync(string userId, Guid invitationId);
    Task<Result> UpdateRoleAsync(UpdateOrganizationMembershipRoleDomainModel domainModel);
}
