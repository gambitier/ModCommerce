using FluentResults;
using IdentityService.Domain.Models;

namespace IdentityService.Domain.Interfaces.Repositories;

public interface IUserRepository
{
    Task<Result<User>> CreateAsync(string username, string email, string password);
    Task<Result<bool>> IsEmailConfirmedAsync(string usernameOrEmail);
    Task<Result<User>> VerifyUserPasswordAsync(string usernameOrEmail, string password);
    Task<Result<User>> FindByEmailAsync(string email);
    Task<Result<IEnumerable<User>>> GetAllAsync();
    Task<Result<User>> FindByIdAsync(string userId);
    Task<Result<User>> ConfirmEmailAsync(string email, string token);
    Task<Result<string>> GenerateEmailConfirmationTokenAsync(string email);
}
