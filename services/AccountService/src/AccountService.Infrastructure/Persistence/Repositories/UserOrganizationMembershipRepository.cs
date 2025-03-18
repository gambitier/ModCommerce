using AccountService.Domain.Errors;
using AccountService.Domain.Interfaces.Repositories;
using AccountService.Domain.Models.Organizations.DomainModels;
using AccountService.Infrastructure.Persistence.Entities;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace AccountService.Infrastructure.Persistence.Repositories;

public class UserOrganizationMembershipRepository : IUserOrganizationMembershipRepository
{
    private readonly UserServiceDbContext _dbContext;

    public UserOrganizationMembershipRepository(UserServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<Guid>> AddAsync(CreateOrganizationMembershipRoleDomainModel domainModel)
    {
        var existingOrganizationMembership = await _dbContext
            .UserOrganizationMemberships
            .FirstOrDefaultAsync(x =>
                x.UserId == domainModel.UserId
                && x.OrganizationId == domainModel.OrganizationId);

        if (existingOrganizationMembership != null)
        {
            return Result.Fail(UserOrganizationMembershipDomainErrors.UserAlreadyMemberOfOrganization);
        }

        var organizationMembership = UserOrganizationMembership.Create(domainModel);
        try
        {
            await _dbContext
                .UserOrganizationMemberships
                .AddAsync(organizationMembership);
            return Result.Ok(organizationMembership.Id);
        }
        catch (Exception ex)
        {
            return Result.Fail(new InternalError("UserOrganizationMembership.AddAsync", ex.Message));
        }
    }

    public async Task<Result> UpdateRoleAsync(UpdateOrganizationMembershipRoleDomainModel domainModel)
    {
        var organizationMembership = await _dbContext
            .UserOrganizationMemberships
            .Where(x =>
                x.UserId == domainModel.UserId
                && x.OrganizationId == domainModel.OrganizationId)
            .FirstOrDefaultAsync();

        if (organizationMembership == null)
        {
            return Result.Fail(UserOrganizationMembershipDomainErrors.OrganizationMembershipNotFound);
        }

        organizationMembership.UpdateRole(domainModel.Role);
        return Result.Ok();
    }
}