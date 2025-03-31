using AccountService.Domain.Errors;
using AccountService.Domain.Interfaces.Repositories;
using AccountService.Domain.Models.Organizations.DomainModels;
using AccountService.Infrastructure.Persistence.Entities;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace AccountService.Infrastructure.Persistence.Repositories;

public class UserOrganizationMembershipRepository : IUserOrganizationMembershipRepository
{
    private readonly AccountDbContext _dbContext;

    public UserOrganizationMembershipRepository(AccountDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<Result<Guid>> AddMemberAsync(AddOrganizationMemberDomainModel domainModel)
    {
        // TODO: call authorization service to add member to organization with role
        throw new NotImplementedException();
    }

    public async Task<Result> AcceptOrganizationInvitationAsync(string acceptedByUserId, Guid invitationId)
    {
        var invitation = await _dbContext
            .OrganizationMemberInvitations
            .FirstOrDefaultAsync(x => x.Id == invitationId);

        if (invitation == null)
            return Result.Fail(OrganizationMemberInvitationsDomainErrors.OrganizationInvitationNotFound);

        var acceptResult = invitation.Accept(acceptedByUserId);
        if (acceptResult.IsFailed)
            return acceptResult;

        return Result.Ok();
    }

    public async Task<Result> RejectOrganizationInvitationAsync(string rejectedByUserId, Guid invitationId)
    {
        var invitation = await _dbContext
            .OrganizationMemberInvitations
            .FirstOrDefaultAsync(x => x.Id == invitationId);

        if (invitation == null)
            return Result.Fail(OrganizationMemberInvitationsDomainErrors.OrganizationInvitationNotFound);

        var rejectResult = invitation.Reject(rejectedByUserId);
        if (rejectResult.IsFailed)
            return rejectResult;

        return Result.Ok();
    }

    public async Task<Result> InviteMemberAsync(string invitedByUserId, InviteOrganizationMemberDomainModel domainModel)
    {
        var existingOrganizationMembership = await _dbContext
            .OrganizationMemberInvitations
            .FirstOrDefaultAsync(x =>
                x.UserId == domainModel.UserId
                && x.OrganizationId == domainModel.OrganizationId);

        if (existingOrganizationMembership != null)
            return Result.Fail(OrganizationMemberInvitationsDomainErrors.InvitationExists);

        var organizationMembership = OrganizationMemberInvitation.Create(invitedByUserId, domainModel);
        try
        {
            await _dbContext
                .OrganizationMemberInvitations
                .AddAsync(organizationMembership);

            return Result.Ok();
        }
        catch (Exception ex)
        {
            return Result.Fail(new InternalError("UserOrganizationMembership.AddAsync", ex.Message));
        }
    }

    public Task<Result> UpdateRoleAsync(UpdateOrganizationMembershipRoleDomainModel domainModel)
    {
        // TODO: call authorization service to update role
        throw new NotImplementedException();
    }
}