using MassTransit;
using MediatR;

namespace AccountService.Domain.Events.UserProfile;

public static class UserProfileCreatedDomainEventConstants
{
    public const string Exchange = "user-profile-created";
}

[EntityName(UserProfileCreatedDomainEventConstants.Exchange)]
public class UserProfileCreatedDomainEvent : INotification
{
    public Guid UserProfileId { get; private set; }
    public string UserId { get; private set; }


    public UserProfileCreatedDomainEvent(
        Guid userProfileId,
        string userId)
    {
        UserProfileId = userProfileId;
        UserId = userId;
    }
}
