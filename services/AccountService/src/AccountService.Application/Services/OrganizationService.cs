using AccountService.Domain.Interfaces.Persistence;
using AccountService.Domain.Interfaces.Repositories;
using AccountService.Domain.Interfaces.Services;
using AccountService.Domain.Models.Organizations.DomainModels;
using AccountService.Domain.Models.Organizations.Dtos;
using FluentResults;

namespace AccountService.Application.Services;

public class OrganizationService : IOrganizationService
{
    private readonly IOrganizationRepository _organizationRepository;
    private readonly IUnitOfWork _unitOfWork;

    public OrganizationService(
        IOrganizationRepository organizationRepository,
        IUnitOfWork unitOfWork)
    {
        _organizationRepository = organizationRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> CreateOrganizationAsync(CreateOrganizationDomainModel createOrganizationDomainModel)
    {
        var organizationId = await _organizationRepository.AddAsync(createOrganizationDomainModel);
        var saveResult = await _unitOfWork.SaveChangesAsync();
        if (saveResult.IsFailed)
        {
            return Result.Fail(saveResult.Errors);
        }
        return organizationId;
    }

    public async Task<OrganizationDto> GetByIdAsync(Guid id)
    {
        return await _organizationRepository.GetByIdAsync(id);
    }
}
