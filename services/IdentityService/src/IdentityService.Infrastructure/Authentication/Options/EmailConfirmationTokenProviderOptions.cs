using Microsoft.AspNetCore.Identity;

namespace IdentityService.Infrastructure.Authentication.Options;

public class EmailConfirmationTokenProviderOptions : DataProtectionTokenProviderOptions
{
    public EmailConfirmationTokenProviderOptions()
    {
        Name = "EmailDataProtectorTokenProvider";
        TokenLifespan = TimeSpan.FromHours(2);
    }
}
