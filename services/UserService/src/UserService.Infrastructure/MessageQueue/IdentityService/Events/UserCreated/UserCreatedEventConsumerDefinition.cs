using MassTransit;
using UserService.Infrastructure.MessageQueue.IdentityService.Constants;

namespace UserService.Infrastructure.MessageQueue.IdentityService.Events.UserCreated;

/// <summary>
/// This is a consumer definition for the <see cref="UserCreatedEventConsumer"/>.
/// It is used to configure the consumer endpoint and the message retry policy.
/// This definition will be automatically discovered by MassTransit when the assembly is scanned for consumers.
/// Convention is to name the definition class as the consumer name with "Definition" suffix - {ConsumerName}Definition (e.g. UserCreatedEventConsumerDefinition).
/// </summary>
public class UserCreatedEventConsumerDefinition : ConsumerDefinition<UserCreatedEventConsumer>
{
    public UserCreatedEventConsumerDefinition()
    {
        // Set endpoint name explicitly
        EndpointName = EventConstants.UserCreatedEvent.Queue;
    }

    protected override void ConfigureConsumer(
        IReceiveEndpointConfigurator endpointConfigurator,
        IConsumerConfigurator<UserCreatedEventConsumer> consumerConfigurator,
        IRegistrationContext context)
    {
        // endpointConfigurator.ConfigureConsumeTopology = false;

        // Configure retry policy
        endpointConfigurator.UseMessageRetry(r =>
            r.Intervals(100, 200, 500, 1000, 2000));

        if (endpointConfigurator is IRabbitMqReceiveEndpointConfigurator rabbitMqConfigurator)
        {
            // Bind to the exchange
            rabbitMqConfigurator.Bind(EventConstants.UserCreatedEvent.Exchange);
        }
    }
}