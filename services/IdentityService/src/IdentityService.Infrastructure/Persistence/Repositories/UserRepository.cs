using IdentityService.Domain.Entities;
using IdentityService.Domain.Interfaces.Repositories;
using IdentityService.Infrastructure.Persistence.Entities;
using Microsoft.AspNetCore.Identity;
using FluentResults;
using IdentityService.Domain.Errors;

namespace IdentityService.Infrastructure.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly UserManager<User> _userManager;

    public UserRepository(
        UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Result<(string UserId, string Email)>> CreateAsync(string email, string password)
    {
        try
        {
            var existingUser = await _userManager.FindByEmailAsync(email);
            if (existingUser != null)
                return Result.Fail(DomainErrors.Authentication.EmailAlreadyExists);

            var user = new User
            {
                UserName = email,
                Email = email,
            };

            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
            {
                // Map identity framework errors to domain errors
                var errors = result.Errors.Select<IdentityError, IError>(e => e.Code switch
                {
                    "DuplicateEmail" => DomainErrors.Authentication.EmailAlreadyExists,
                    "InvalidEmail" => DomainErrors.User.InvalidEmail,
                    "PasswordTooShort" or "PasswordRequiresDigit" or "PasswordRequiresNonAlphanumeric"
                        => DomainErrors.User.WeakPassword,
                    _ => DomainErrors.User.CreationFailed(e.Description)
                });

                return Result.Fail(errors);
            }

            return Result.Ok((user.Id, user.Email));
        }
        catch (Exception ex)
        {
            return Result.Fail(new InternalError("Database.Error", "An unexpected database error occurred")
                .CausedBy(ex));
        }
    }

    public async Task<Result<IApplicationUser>> FindByEmailAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
            return Result.Fail(DomainErrors.Authentication.UserNotFound);

        return Result.Ok((IApplicationUser)user);
    }

    public async Task<Result<bool>> CheckPasswordAsync(string userId, string password)
    {
        var identityUser = await _userManager.FindByIdAsync(userId);
        if (identityUser == null)
            return Result.Fail(DomainErrors.Authentication.UserNotFound);

        var result = await _userManager.CheckPasswordAsync(identityUser, password);
        return result ? Result.Ok(true) : Result.Fail(DomainErrors.Authentication.InvalidCredentials);
    }
}