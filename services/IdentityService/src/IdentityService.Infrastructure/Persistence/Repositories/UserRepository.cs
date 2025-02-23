using IdentityService.Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Identity;
using FluentResults;
using IdentityService.Domain.Errors;
using IdentityService.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Infrastructure.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly UserManager<Entities.IdentityUser> _userManager;

    public UserRepository(UserManager<Entities.IdentityUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Result<UserDomainModel>> CreateAsync(string email, string password)
    {
        var existingUser = await _userManager.FindByEmailAsync(email);
        if (existingUser != null)
            return Result.Fail(DomainErrors.Authentication.EmailAlreadyExists);

        var user = Entities.IdentityUser.Create(email);
        var result = await _userManager.CreateAsync(user, password);

        if (!result.Succeeded)
        {
            var errors = result.Errors.Select<IdentityError, IError>(e => e.Code switch
            {
                nameof(IdentityErrorDescriber.DuplicateEmail) => DomainErrors.Authentication.EmailAlreadyExists,
                nameof(IdentityErrorDescriber.InvalidEmail) => DomainErrors.User.InvalidEmail,
                nameof(IdentityErrorDescriber.PasswordTooShort) or
                nameof(IdentityErrorDescriber.PasswordRequiresDigit) or
                nameof(IdentityErrorDescriber.PasswordRequiresNonAlphanumeric)
                    => DomainErrors.User.WeakPassword,
                _ => DomainErrors.User.CreationFailed(e.Description)
            });

            return Result.Fail(errors);
        }

        return Result.Ok(UserDomainModel.Create(user.Id, user.Email!));
    }

    public async Task<Result<bool>> CheckPasswordAsync(string userId, string password)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            return Result.Fail(DomainErrors.Authentication.UserNotFound);

        var result = await _userManager.CheckPasswordAsync(user, password);
        return result ? Result.Ok(true) : Result.Fail(DomainErrors.Authentication.InvalidCredentials);
    }

    public async Task<Result<UserDomainModel>> FindByEmailAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
            return Result.Fail(DomainErrors.Authentication.UserNotFound);

        return Result.Ok(UserDomainModel.Create(user.Id, user.Email!));
    }

    public async Task<Result<IEnumerable<UserDomainModel>>> GetAllAsync()
    {
        var users = await _userManager.Users.ToListAsync();
        return Result.Ok(users.Select(u => UserDomainModel.Create(u.Id, u.Email!)));
    }
}