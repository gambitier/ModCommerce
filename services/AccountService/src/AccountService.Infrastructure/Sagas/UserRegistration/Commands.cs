namespace AccountService.Infrastructure.Sagas.UserRegistration;

public class ConfirmUserEmailCommand
{
    public required string UserId { get; set; }
    public required string Email { get; set; }
    public required DateTime ConfirmedAt { get; set; }
}

public class CreateUserProfileCommand
{
    public required string UserId { get; set; }
    public required string Email { get; set; }
    public required string Username { get; set; }
    public required DateTime CreatedAt { get; set; }
}