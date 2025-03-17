using AccountService.Domain.Models.Organizations.DomainModels;

namespace AccountService.Infrastructure.Persistence.Entities;

/// <summary>
/// Represents an organization's database entity.
/// </summary>
public class OrganizationEntity
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    private OrganizationEntity(
        string name,
        string? description,
        DateTime createdAt,
        DateTime updatedAt)
    {
        Id = Guid.NewGuid();
        Name = name;
        Description = description;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    /// <summary>
    /// Creates a new organization.
    /// </summary>
    public static OrganizationEntity Create(CreateOrganizationDomainModel createOrganizationDomainModel)
    {
        return new OrganizationEntity(
            createOrganizationDomainModel.Name,
            createOrganizationDomainModel.Description,
            DateTime.UtcNow,
            DateTime.UtcNow);
    }
}