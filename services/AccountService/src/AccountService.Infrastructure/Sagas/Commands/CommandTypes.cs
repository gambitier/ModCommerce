using System.Text.Json.Serialization;

namespace AccountService.Infrastructure.Sagas.Commands;

/// <summary>
/// Command to create a user profile.
/// </summary>
public class CreateUserProfileCommand
{
    [property: JsonPropertyName("userId")]
    public string UserId { get; set; } = null!;

    [property: JsonPropertyName("email")]
    public string Email { get; set; } = null!;

    [property: JsonPropertyName("username")]
    public string Username { get; set; } = null!;

    [property: JsonPropertyName("createdAt")]
    public DateTime CreatedAt { get; set; }
}

/// <summary>
/// Command to confirm a user's email.
/// </summary>
public class ConfirmUserEmailCommand
{
    [property: JsonPropertyName("userId")]
    public string UserId { get; set; } = null!;

    [property: JsonPropertyName("email")]
    public string Email { get; set; } = null!;

    [property: JsonPropertyName("confirmedAt")]
    public DateTime ConfirmedAt { get; set; }
}