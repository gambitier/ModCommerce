namespace AccountService.Domain.Models.Users.DomainModels;

public class ConfirmUserEmailDomainModel
{
    public required string UserId { get; init; }
    public required string Email { get; init; }
    public required DateTime ConfirmedAt { get; init; }
}
