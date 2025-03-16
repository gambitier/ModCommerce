namespace UserService.Infrastructure.MessageQueue.IdentityService.Constants;

public static class EventConstants
{
    public static class UserCreatedEvent
    {
        private const string Prefix = "IdentityService.Exchanges.Users";
        public const string Exchange = $"{Prefix}:UserCreatedExchange";
        public const string Urn = $"ModCommerce:Events:UserCreated:v1";

        private const string QueuePrefix = "UserService.Queues.Users";
        public const string Queue = $"{QueuePrefix}:UserCreatedQueue";
    }
}