namespace IdentityService.Contracts.Constants;

public static class EventConstants
{
    private const string UserEventPrefix = "IdentityService.Exchanges.Users";

    public static class UserCreatedEvent
    {
        public const string Exchange = $"{UserEventPrefix}:UserCreatedExchange";
        public const string Urn = $"ModCommerce:Events:UserCreated:v1";
    }

    public static class UserEmailConfirmedEvent
    {
        public const string Exchange = $"{UserEventPrefix}:UserEmailConfirmedExchange";
        public const string Urn = $"ModCommerce:Events:UserEmailConfirmed:v1";
    }
}