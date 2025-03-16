using MassTransit;
using Microsoft.Extensions.Logging;
using UserService.Infrastructure.MessageQueue.IdentityService.Constants;

namespace UserService.Infrastructure.MessageQueue.IdentityService.Events.UserEmailConfirmed;

public class UserEmailConfirmedEventConsumerDefinition : ConsumerDefinition<UserEmailConfirmedEventConsumer>
{
    private readonly ILogger<UserEmailConfirmedEventConsumerDefinition> _logger;

    public UserEmailConfirmedEventConsumerDefinition(ILogger<UserEmailConfirmedEventConsumerDefinition> logger)
    {
        _logger = logger;
        EndpointName = EventConstants.UserEmailConfirmedEvent.Queue;
    }

    protected override void ConfigureConsumer(
        IReceiveEndpointConfigurator endpointConfigurator,
        IConsumerConfigurator<UserEmailConfirmedEventConsumer> consumerConfigurator,
        IRegistrationContext context)
    {
        endpointConfigurator.ConfigureConsumeTopology = false;

        // Configure retry policy
        endpointConfigurator.UseMessageRetry(r =>
            r.Intervals(100, 200));

        endpointConfigurator.ClearSerialization();
        endpointConfigurator.UseRawJsonSerializer();
        endpointConfigurator.UseRawJsonDeserializer(isDefault: true);

        if (endpointConfigurator is IRabbitMqReceiveEndpointConfigurator rabbitMqConfigurator)
        {
            _logger.LogInformation("Binding to exchange: {Exchange}", EventConstants.UserEmailConfirmedEvent.Exchange);

            // Bind to the exchange
            rabbitMqConfigurator.Bind(EventConstants.UserEmailConfirmedEvent.Exchange);
        }
    }
}
