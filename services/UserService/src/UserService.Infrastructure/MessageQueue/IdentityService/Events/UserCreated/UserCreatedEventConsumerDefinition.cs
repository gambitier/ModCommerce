using MassTransit;
using Microsoft.Extensions.Logging;
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
    private readonly ILogger<UserCreatedEventConsumerDefinition> _logger;

    public UserCreatedEventConsumerDefinition(ILogger<UserCreatedEventConsumerDefinition> logger)
    {
        _logger = logger;
        EndpointName = EventConstants.UserCreatedEvent.Queue;
    }

    protected override void ConfigureConsumer(
        IReceiveEndpointConfigurator endpointConfigurator,
        IConsumerConfigurator<UserCreatedEventConsumer> consumerConfigurator,
        IRegistrationContext context)
    {
        endpointConfigurator.ConfigureConsumeTopology = false;

        // Configure retry policy
        endpointConfigurator.UseMessageRetry(r =>
            r.Intervals(100, 200, 500, 1000, 2000));

        endpointConfigurator.ClearSerialization();
        endpointConfigurator.UseRawJsonSerializer();

        if (endpointConfigurator is IRabbitMqReceiveEndpointConfigurator rabbitMqConfigurator)
        {
            _logger.LogInformation("Binding to exchange: {Exchange}", EventConstants.UserCreatedEvent.Exchange);

            // Bind to the exchange
            // rabbitMqConfigurator.ExchangeType = "fanout"; // Match the exchange type
            // rabbitMqConfigurator.Bind(EventConstants.UserCreatedEvent.Exchange, x =>
            // {
            //     x.ExchangeType = "fanout";
            //     x.RoutingKey = ""; // Empty for fanout exchanges
            // });
            rabbitMqConfigurator.Bind(EventConstants.UserCreatedEvent.Exchange);
        }
    }
}