namespace IdentityService.Infrastructure.Communication.Templates;

public static class EmailTemplates
{
    public static class ConfirmEmail
    {
        public const string Subject = "Confirm your email";

        public static string GetHtmlBody(string username, string confirmationLink) =>
            $"""
            <!DOCTYPE html>
            <html>
            <body>
                <h2>Welcome {username}!</h2>
                <p>Please confirm your email by clicking the link below:</p>
                <p><a href='{confirmationLink}'>Confirm Email</a></p>
                <p>If you didn't create an account, you can ignore this email.</p>
            </body>
            </html>
            """;

        public static string GetTextBody(string username, string confirmationLink) =>
            $"""
            Welcome {username}!
            Please confirm your email by clicking the link below:
            {confirmationLink}
            If you didn't create an account, you can ignore this email.
            """;
    }
}