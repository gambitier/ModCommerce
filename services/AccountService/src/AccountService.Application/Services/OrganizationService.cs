using AccountService.Domain.Interfaces.Persistence;
using AccountService.Domain.Interfaces.Repositories;
using AccountService.Domain.Interfaces.Services;
using AccountService.Domain.Models.Organizations.DomainModels;
using AccountService.Domain.Models.Organizations.Dtos;
using AccountService.Domain.Models.Organizations.Enums;
using FluentResults;

namespace AccountService.Application.Services;

public class OrganizationService : IOrganizationService
{
    private readonly IOrganizationRepository _organizationRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserOrganizationMembershipRepository _userOrganizationMembershipRepository;

    public OrganizationService(
        IOrganizationRepository organizationRepository,
        IUnitOfWork unitOfWork,
        IUserOrganizationMembershipRepository userOrganizationMembershipRepository)
    {
        _organizationRepository = organizationRepository;
        _unitOfWork = unitOfWork;
        _userOrganizationMembershipRepository = userOrganizationMembershipRepository;
    }

    public async Task<Result<Guid>> CreateOrganizationAsync(string userId, CreateOrganizationDomainModel domainModel)
    {
        var createOrgResult = await _organizationRepository.AddAsync(domainModel);
        if (createOrgResult.IsFailed)
            return createOrgResult.ToResult<Guid>();

        var orgId = createOrgResult.Value;

        var orgMembershipResult = await _userOrganizationMembershipRepository
            .AddAsync(new AddToOrganizationMemberDomainModel
            {
                OrganizationId = orgId,
                UserId = userId,
                Role = UserOrganizationMembershipRole.Admin
            });
        if (orgMembershipResult.IsFailed)
            return orgMembershipResult.ToResult<Guid>();

        var saveResult = await _unitOfWork.SaveChangesAsync();
        if (saveResult.IsFailed)
            return saveResult.ToResult<Guid>();

        return orgId;
    }

    public async Task<Result<OrganizationDto>> GetByIdAsync(Guid id)
    {
        var getOrgResult = await _organizationRepository.GetByIdAsync(id);
        if (getOrgResult.IsFailed)
        {
            return getOrgResult.ToResult();
        }

        return getOrgResult.Value;
    }

    public async Task<Result> AddMemberAsync(string addedByUserId, AddToOrganizationMemberDomainModel domainModel)
    {
        var addMemberResult = await _userOrganizationMembershipRepository.AddAsync(domainModel);
        if (addMemberResult.IsFailed)
            return addMemberResult.ToResult();

        var saveResult = await _unitOfWork.SaveChangesAsync();
        if (saveResult.IsFailed)
            return saveResult;

        return Result.Ok();
    }
}
