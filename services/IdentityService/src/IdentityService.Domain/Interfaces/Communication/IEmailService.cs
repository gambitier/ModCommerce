namespace IdentityService.Domain.Interfaces.Communication;

public interface IEmailService
{
    Task SendConfirmationEmailAsync(string email, string username, string confirmationLink);
}