namespace AccountService.Domain.Models.Users.DomainModels;

/// <summary>
/// Represents a request to update a user profile.
/// Note:
/// - Identity fields are not allowed to be updated in this service.
/// - Only include fields that are managed by this service for user profiles.
/// </summary>
public class UpdateUserProfileDomainModel
{
    public string? FirstName { get; init; }
    public string? LastName { get; init; }

    public void Deconstruct(
        out string? firstName,
        out string? lastName)
    {
        firstName = FirstName;
        lastName = LastName;
    }
}