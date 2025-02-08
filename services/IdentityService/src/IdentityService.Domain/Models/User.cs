namespace IdentityService.Domain.Models;

public class UserDomainModel
{
    public string Id { get; private set; }
    public string Email { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    private UserDomainModel(string id, string email)
    {
        Id = id;
        Email = email;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public static UserDomainModel Create(string id, string email)
    {
        return new UserDomainModel(id, email);
    }
}
