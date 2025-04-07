using MassTransit;
using MediatR;

namespace AccountService.Domain.Events.UserProfile;

public static class UserProfileEmailConfirmedDomainEventConstants
{
    public const string Exchange = "user-profile-email-confirmed";
}

[EntityName(UserProfileEmailConfirmedDomainEventConstants.Exchange)]
public class UserProfileEmailConfirmedDomainEvent : INotification
{
    public Guid UserProfileId { get; private set; }
    public string UserId { get; private set; }


    public UserProfileEmailConfirmedDomainEvent(
        Guid userProfileId,
        string userId)
    {
        UserProfileId = userProfileId;
        UserId = userId;
    }
}
