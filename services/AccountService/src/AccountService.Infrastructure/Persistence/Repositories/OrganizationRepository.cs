using AccountService.Domain.Interfaces.Repositories;
using AccountService.Domain.Models.Organizations.DomainModels;
using AccountService.Domain.Models.Organizations.Dtos;
using AccountService.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace AccountService.Infrastructure.Persistence.Repositories;

public class OrganizationRepository : IOrganizationRepository
{
    private readonly UserServiceDbContext _dbContext;

    public OrganizationRepository(UserServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Guid> AddAsync(CreateOrganizationDomainModel createOrganizationDomainModel)
    {
        var organization = OrganizationEntity.Create(createOrganizationDomainModel);
        await _dbContext.Organizations.AddAsync(organization);
        return organization.Id;
    }

    public async Task<OrganizationDto> GetByIdAsync(Guid id)
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
            throw new Exception("Organization not found");
        }

        return organizationDto;
    }
}