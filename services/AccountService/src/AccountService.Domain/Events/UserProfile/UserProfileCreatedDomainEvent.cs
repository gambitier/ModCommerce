using MediatR;

namespace AccountService.Domain.Events.UserProfile;

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
