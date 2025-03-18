using AccountService.Domain.Errors;
using AccountService.Domain.Interfaces.Repositories;
using AccountService.Domain.Models.Organizations.DomainModels;
using AccountService.Domain.Models.Organizations.Dtos;
using AccountService.Infrastructure.Persistence.Entities;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace AccountService.Infrastructure.Persistence.Repositories;

public class OrganizationRepository : IOrganizationRepository
{
    private readonly UserServiceDbContext _dbContext;

    public OrganizationRepository(UserServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<Guid>> AddAsync(CreateOrganizationDomainModel createOrganizationDomainModel)
    {
        var organization = OrganizationEntity.Create(createOrganizationDomainModel);
        await _dbContext.Organizations.AddAsync(organization);
        return Result.Ok(organization.Id);
    }

    public async Task<Result<OrganizationDto>> GetByIdAsync(Guid id)
    {
        var organizationDto = await _dbContext
            .Organizations
            .Where(o => o.Id == id)
            .Select(o => new OrganizationDto
            {
                Id = o.Id,
                Name = o.Name,
                Description = o.Description,
                CreatedAt = o.CreatedAt,
            })
            .FirstOrDefaultAsync();

        if (organizationDto == null)
        {
            return Result.Fail(OrganizationDomainErrors.OrganizationNotFound);
        }

        return Result.Ok(organizationDto);
    }
}