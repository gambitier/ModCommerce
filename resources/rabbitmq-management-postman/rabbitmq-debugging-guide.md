# RabbitMQ Debugging Guide

This guide explains how to use the RabbitMQ Management API via Postman to debug message delivery issues.

## Core RabbitMQ Concepts

### Exchanges
- **What**: Message routing centers that receive messages from producers and route them to queues
- **Types**:
  - **Direct**: Routes messages to queues with matching routing keys
  - **Fanout**: Broadcasts messages to all bound queues
  - **Topic**: Routes based on pattern matching of routing keys
  - **Headers**: Routes based on message header values

### Queues
- **What**: Message storage destinations that consumers connect to
- **Properties**:
  - **Durable**: Survives broker restart
  - **Exclusive**: Used by only one connection
  - **Auto-delete**: Deleted when last consumer unsubscribes

### Bindings
- **What**: Rules that tell exchanges which queues to route messages to
- **Components**:
  - **Exchange name**: Source exchange
  - **Queue name**: Destination queue
  - **Routing key**: Pattern for message routing (for direct and topic exchanges)
  - **Arguments**: Additional binding parameters

### Message Flow
1. **Producer** publishes message to an **Exchange**
2. **Exchange** routes message to **Queue(s)** based on **Bindings**
3. **Consumer** retrieves message from **Queue**

## MassTransit Specifics

MassTransit adds its own conventions on top of RabbitMQ:

- **Message Envelope**: Wraps your message with metadata (messageId, messageType, etc.)
- **Message Types**: Uses URNs to identify message types (e.g., `urn:message:ModCommerce:Events:UserCreated:v1`)
- **Naming Conventions**: Auto-creates exchanges and queues with specific naming patterns

## Debugging with Postman

### Setup
1. Import the `rabbitmq-management.json.postman_collection`
2. Set environment variables:
   - `rabbitmq_host`: Usually `localhost:15672`
   - `vhost`: Default is `%2F` (URL-encoded `/`)
   - `username`: Default is `guest`
   - `password`: Default is `guest`

### Common Debugging Flows

#### 1. Check if Exchange Exists
```
GET {{rabbitmq_host}}/api/exchanges/{{vhost}}/IdentityService.Exchanges.Users:UserCreatedExchange
```
- Confirms the exchange was created correctly
- Shows exchange type and parameters

#### 2. Check if Queue Exists
```
GET {{rabbitmq_host}}/api/queues/{{vhost}}/UserService.Queues.Users:UserCreatedQueue
```
- Confirms the queue was created correctly
- Shows message count and consumer count

#### 3. Verify Bindings
```
GET {{rabbitmq_host}}/api/exchanges/{{vhost}}/IdentityService.Exchanges.Users:UserCreatedExchange/bindings/source
```
- Shows all bindings from the exchange
- Confirms queue is bound correctly with right routing key

#### 4. Inspect Messages in Queue
```
POST {{rabbitmq_host}}/api/queues/{{vhost}}/UserService.Queues.Users:UserCreatedQueue/get
```
Body:
```json
{
  "count": 5,
  "ackmode": "ack_requeue_true",
  "encoding": "auto"
}
```
- Shows messages currently in the queue
- Setting `ackmode` to `ack_requeue_true` keeps messages in the queue

#### 5. Publish Test Message
```
POST {{rabbitmq_host}}/api/exchanges/{{vhost}}/IdentityService.Exchanges.Users:UserCreatedExchange/publish
```
Body:
```json
{
  "properties": {
    "content_type": "application/json",
    "headers": {
      "x-message-type": "urn:message:ModCommerce:Events:UserCreated:v1"
    }
  },
  "routing_key": "",
  "payload": "{\"userId\":\"test-id\",\"email\":\"test@example.com\",\"username\":\"testuser\",\"createdAt\":\"2023-04-01T12:00:00Z\"}",
  "payload_encoding": "string"
}
```
- Tests if exchange routes messages to queue
- Note: This bypasses MassTransit envelope - useful for basic routing tests

## Debugging UserCreatedEvent Issues

### 1. Check MessageType URN

In the message inspection output, look for:
```json
"messageType": [
  "urn:message:ModCommerce:Events:UserCreated:v1" 
]
```
or
```json
"messageType": [
  "urn:message:IdentityService.Contracts.Events.Users:UserCreatedEvent"
]
```

The URN must match exactly between producer and consumer to be handled correctly.

### 2. Check Message Serialization

Look at the `message` property in the JSON payload:
```json
"message": {
  "userId": "1d9a14b6-07e4-438f-a94a-b534067b5079",
  "email": "test@example.com",
  "username": "testuser",
  "createdAt": "2023-04-01T12:00:00Z"
}
```

Property names must match between producer and consumer, including case sensitivity.

### 3. Debug Steps for Common Issues

#### No Message in Queue
1. Check if exchange exists
2. Verify producer is publishing correctly
3. Check binding between exchange and queue

#### Message in Queue but Consumer Not Triggered
1. Verify message URN matches consumer's message type
2. Check if consumer is connected (should show in queue details)
3. Examine message serialization format

#### Message Received but Data Missing
1. Verify property names match between producer and consumer
2. Check serialization settings in consumer definition
3. Ensure data types match (string vs int, format of date, etc.)

## Common Issues & Solutions

1. **URN Mismatch**: Ensure `MessageUrn` attribute matches exactly on both sides
2. **Case Sensitivity**: JSON property names are case sensitive
3. **Binding Issues**: Check if queue is bound to exchange with correct routing key
4. **Serialization**: Use `UseRawJsonDeserializer` with MassTransit to handle non-MassTransit messages
5. **Namespace Changes**: When namespaces differ between producer/consumer, use `MessageUrn` to maintain compatibility

## Advanced Debugging

For more complex issues:
1. Check RabbitMQ Server Logs
2. Enable MassTransit Diagnostics Logging
3. Use Wireshark to capture AMQP traffic
