using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using IdentityService.Domain.Interfaces.Communication;
using IdentityService.Infrastructure.Communication.Options;
using IdentityService.Infrastructure.Communication.Templates;

namespace IdentityService.Infrastructure.Communication;

public class EmailService : IEmailService, IDisposable
{
    private readonly EmailOptions _options;
    private readonly SmtpClient _client;
    private bool _disposed;

    public EmailService(IOptions<EmailOptions> options)
    {
        _options = options.Value;
        _client = new SmtpClient();
    }

    public async Task SendConfirmationEmailAsync(string email, string username, string confirmationLink)
    {
        var message = CreateConfirmationEmail(email, username, confirmationLink);
        await SendEmailAsync(message);
    }

    private MimeMessage CreateConfirmationEmail(string email, string username, string confirmationLink)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(_options.SenderName, _options.SenderEmail));
        message.To.Add(new MailboxAddress(username, email));
        message.Subject = EmailTemplates.ConfirmEmail.Subject;

        var builder = new BodyBuilder
        {
            HtmlBody = EmailTemplates.ConfirmEmail.GetHtmlBody(username, confirmationLink),
            TextBody = EmailTemplates.ConfirmEmail.GetTextBody(username, confirmationLink)
        };

        message.Body = builder.ToMessageBody();
        return message;
    }

    private async Task SendEmailAsync(MimeMessage message)
    {
        try
        {
            if (!_client.IsConnected)
            {
                await _client.ConnectAsync(_options.SmtpServer, _options.SmtpPort, SecureSocketOptions.StartTls);
                await _client.AuthenticateAsync(_options.SmtpUsername, _options.SmtpPassword);
            }

            await _client.SendAsync(message);
        }
        catch (Exception)
        {
            // Consider logging the error
            throw;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
            return;

        if (disposing && _client.IsConnected)
        {
            _client.Disconnect(true);
            _client.Dispose();
        }

        _disposed = true;
    }
}