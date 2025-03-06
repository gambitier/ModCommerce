using IdentityService.Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Identity;
using FluentResults;
using IdentityService.Domain.Errors;
using IdentityService.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace IdentityService.Infrastructure.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly UserManager<Entities.IdentityUser> _userManager;

    public UserRepository(UserManager<Entities.IdentityUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Result<User>> CreateAsync(string username, string email, string password)
    {
        var existingUser = await _userManager.FindByEmailAsync(email);
        if (existingUser != null)
            return Result.Fail(DomainErrors.User.EmailAlreadyExists);

        var existingUsername = await _userManager.FindByNameAsync(username);
        if (existingUsername != null)
            return Result.Fail(DomainErrors.User.UsernameAlreadyExists);

        var user = Entities.IdentityUser.Create(username, email);
        var result = await _userManager.CreateAsync(user, password);

        if (!result.Succeeded)
        {
            var errors = result.Errors.Select<IdentityError, IError>(e => e.Code switch
            {
                nameof(IdentityErrorDescriber.DuplicateEmail) => DomainErrors.User.EmailAlreadyExists,
                nameof(IdentityErrorDescriber.InvalidEmail) => DomainErrors.User.InvalidEmail,
                nameof(IdentityErrorDescriber.PasswordTooShort) or
                nameof(IdentityErrorDescriber.PasswordRequiresDigit) or
                nameof(IdentityErrorDescriber.PasswordRequiresNonAlphanumeric)
                    => DomainErrors.User.WeakPassword,
                _ => DomainErrors.User.CreationFailed(e.Description)
            });

            return Result.Fail(errors);
        }

        return Result.Ok(new User
        {
            Id = user.Id,
            Email = user.Email!,
            Username = user.UserName!,
            EmailConfirmed = user.EmailConfirmed
        });
    }

    public async Task<Result<User>> VerifyUserPasswordAsync(string usernameOrEmail, string password)
    {
        var user = await _userManager.FindByEmailAsync(usernameOrEmail)
                ?? await _userManager.FindByNameAsync(usernameOrEmail);

        if (user == null)
            return Result.Fail(DomainErrors.User.UserNotFound);

        var isPwdValid = await _userManager.CheckPasswordAsync(user, password);

        return isPwdValid
            ? Result.Ok(new User
            {
                Id = user.Id,
                Email = user.Email!,
                Username = user.UserName!,
                EmailConfirmed = user.EmailConfirmed
            })
            : Result.Fail(DomainErrors.Authentication.InvalidCredentials);
    }

    public async Task<Result<User>> FindByEmailAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
            return Result.Fail(DomainErrors.User.UserNotFound);

        return Result.Ok(new User
        {
            Id = user.Id,
            Email = user.Email!,
            Username = user.UserName!,
            EmailConfirmed = user.EmailConfirmed
        });
    }

    public async Task<Result<User>> FindByIdAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            return Result.Fail(DomainErrors.User.UserNotFound);

        return Result.Ok(new User
        {
            Id = user.Id,
            Email = user.Email!,
            Username = user.UserName!,
            EmailConfirmed = user.EmailConfirmed
        });
    }

    public async Task<Result<IEnumerable<User>>> GetAllAsync()
    {
        var users = await _userManager.Users.ToListAsync();
        return Result.Ok(users.Select(u => new User
        {
            Id = u.Id,
            Email = u.Email!,
            Username = u.UserName!,
            EmailConfirmed = u.EmailConfirmed
        }));
    }

    public async Task<Result<User>> ConfirmEmailAsync(string email, string token)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
            return Result.Fail(DomainErrors.User.UserNotFound);

        var successResponse = new User
        {
            Id = user.Id,
            Email = user.Email!,
            Username = user.UserName!,
            EmailConfirmed = true
        };

        if (user.EmailConfirmed)
            return Result.Ok(successResponse);

        var result = await _userManager.ConfirmEmailAsync(user, token);
        if (!result.Succeeded)
            return Result.Fail(DomainErrors.Authentication.InvalidEmailConfirmationToken);

        return Result.Ok(successResponse);
    }

    public async Task<Result<string>> GenerateEmailConfirmationTokenAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
            return Result.Fail(DomainErrors.User.UserNotFound);

        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        if (string.IsNullOrEmpty(token))
            return Result.Fail(DomainErrors.User.EmailConfirmationTokenGenerationFailed);

        return Result.Ok(token);
    }

    public async Task<Result<bool>> IsEmailConfirmedAsync(string UsernameOrEmail)
    {
        var user = await _userManager.FindByEmailAsync(UsernameOrEmail)
                ?? await _userManager.FindByNameAsync(UsernameOrEmail);

        if (user == null)
            return Result.Fail(DomainErrors.User.UserNotFound);

        return Result.Ok(user.EmailConfirmed);
    }
}
