# Message Queue

This directory contains the message queue components organized by service boundaries and message types.

## Directory Structure

```
MessageQueue/
└── IdentityService/
    ├── Constants/              # All constants related to IdentityService integration
    │   └── EventConstants.cs
    ├── Events/                # Event messages and their consumers
    │   └── UserCreated/
    │       ├── UserCreatedIntegrationEvent.cs
    │       ├── UserCreatedEventConsumer.cs
    │       └── UserCreatedEventConsumerDefinition.cs
    ├── Commands/              # Command messages and their consumers
    │   └── UpdateUser/        # Example structure for future commands
    ├── Requests/             # Request-response patterns
    │   └── GetUserDetails/   # Example structure for future requests
    └── Contracts/            # Shared DTOs and interfaces
```

## Message Types

1. **Events**: Represent something that has happened in the past. They are facts that have occurred.
   - Example: `UserCreatedIntegrationEvent`

2. **Commands**: Represent actions that should be performed. They are instructions to do something.
   - Example: `UpdateUserCommand` (future implementation)

3. **Requests**: Represent request-response patterns where a response is expected.
   - Example: `GetUserDetailsRequest` (future implementation)

## Best Practices

1. Each message type (Event/Command/Request) should have its own directory
2. Related files (Consumer, Definition, Message) should be kept together
3. Constants are kept at the service level for easy access
4. Shared contracts/DTOs should be placed in the Contracts directory
5. Each consumer should have its corresponding definition file 