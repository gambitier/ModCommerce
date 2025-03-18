using System.Security.Claims;

namespace AccountService.API.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static string GetUserId(this ClaimsPrincipal principal)
    {
        var subject = (principal.FindFirst("sub")?.Value)
            ?? throw new UnauthorizedAccessException("Invalid access token: subject claim not found.");

        return subject;
    }
}
