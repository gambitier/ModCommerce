using System.Text.Json.Serialization;
using IdentityService.Contracts.Constants;
using MassTransit;

namespace IdentityService.Contracts.Events.Users;

/// <summary>
/// Event published when a user's email is confirmed in the Identity service.
/// </summary>
/// <param name="UserId">The unique identifier of the user.</param>
/// <param name="Email">The email address of the user.</param>
/// <param name="ConfirmedAt">The UTC timestamp when the user's email was confirmed.</param>
[EntityName(EventConstants.UserEmailConfirmedEvent.Exchange)]
[MessageUrn(EventConstants.UserEmailConfirmedEvent.Urn)]
public record UserEmailConfirmedEvent(
    /// <summary>
    /// The unique identifier of the user.
    /// </summary>
    [property: JsonPropertyName("userId")]
    string UserId,

    /// <summary>
    /// The email address of the user.
    /// </summary>
    [property: JsonPropertyName("email")]
    string Email,

    /// <summary>
    /// The UTC timestamp when the user's email was confirmed.
    /// </summary>
    [property: JsonPropertyName("confirmedAt")]
    DateTime ConfirmedAt
);
