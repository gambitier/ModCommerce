using System.Text;
using IdentityService.API.Constants;
using IdentityService.Application.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace IdentityService.API.Extensions;

public static class JwtAuthenticationExtensions
{
    public static void AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtOptions = configuration.GetSection(ConfigurationConstants.JwtSection).Get<JwtOptions>()
            ?? throw new InvalidOperationException("JWT options are not configured");

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtOptions.Issuer,
                    ValidAudience = jwtOptions.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtOptions.Secret)
                    ),
                    ClockSkew = TimeSpan.Zero
                };
            });
    }
}
