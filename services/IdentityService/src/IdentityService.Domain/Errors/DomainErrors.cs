namespace IdentityService.Domain.Errors;

public static class DomainErrors
{
    public static class Authentication
    {
        public static UnauthorizedError InvalidCredentials => new("Auth.InvalidCredentials",
            "Invalid email or password");

        public static ConflictError EmailAlreadyExists => new("Auth.EmailExists",
            "Email is already registered");

        public static NotFoundError UserNotFound => new("Auth.UserNotFound",
            "User not found");
    }

    public static class User
    {
        public static ValidationError InvalidEmail => new("User.InvalidEmail",
            "Email is invalid");

        public static ValidationError WeakPassword => new("User.WeakPassword",
            "Password does not meet requirements");

        public static ValidationError CreationFailed(string description) =>
            new("User.CreationFailed", description);
    }
}