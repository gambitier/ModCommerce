namespace IdentityService.Domain.Errors;

public static class DomainErrors
{
    public static class Authentication
    {
        public static UnauthorizedError InvalidCredentials => new("Auth.InvalidCredentials",
            "Invalid email or password");

        public static UnauthorizedError InvalidRefreshToken => new("Auth.InvalidRefreshToken",
            "Invalid refresh token");

        public static UnauthorizedError InvalidEmailConfirmationToken => new("Auth.InvalidEmailConfirmationToken",
            "Invalid email confirmation token");
    }

    public static class User
    {
        public static ValidationError InvalidEmail => new("User.InvalidEmail",
            "Email is invalid");

        public static ValidationError WeakPassword => new("User.WeakPassword",
            "Password does not meet requirements");

        public static ValidationError CreationFailed(string description) =>
            new("User.CreationFailed", description);

        public static ConflictError EmailAlreadyExists => new("User.EmailExists",
            "Email is already registered");

        public static ConflictError UsernameAlreadyExists => new("User.UsernameExists",
            "Username is already registered");

        public static NotFoundError UserNotFound => new("User.UserNotFound",
            "User not found");

        public static ConflictError EmailAlreadyConfirmed => new("User.EmailAlreadyConfirmed",
            "Email is already confirmed");

        public static ValidationError EmailNotConfirmed => new("User.EmailNotConfirmed",
            "Email is not confirmed");

        public static InternalError EmailConfirmationTokenGenerationFailed => new("User.EmailConfirmationTokenGenerationFailed",
            "Email confirmation token generation failed");
    }
}