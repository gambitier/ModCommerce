namespace IdentityService.Domain.Constants;

/// <summary>
/// Authentication scopes
/// Scopes are used to define the permissions of the token.
/// According to OAuth 2.0 spec (RFC 6749), scopes are space-delimited strings that can be combined (e.g., "openid profile email")
/// </summary>
public static class Scopes
{
    public const string ApiAccess = "api_access";
    public const string OpenId = "openid";
    public const string Profile = "profile";
    public const string Email = "email";
}
