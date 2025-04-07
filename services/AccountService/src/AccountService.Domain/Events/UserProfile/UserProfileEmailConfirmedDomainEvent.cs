using MediatR;

namespace AccountService.Domain.Events.UserProfile;

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
