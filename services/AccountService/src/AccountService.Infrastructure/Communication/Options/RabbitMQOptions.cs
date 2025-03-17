using System.ComponentModel.DataAnnotations;

namespace AccountService.Infrastructure.Communication.Options;

/// <summary>
/// Configuration options for RabbitMQ message broker connection.
/// </summary>
public class RabbitMQOptions
{
    /// <summary>
    /// hostname or IP address of the RabbitMQ server.
    /// </summary>
    /// <remarks>
    /// Examples: "localhost", "rabbitmq.company.com", "127.0.0.1"
    /// </remarks>
    [Required(ErrorMessage = "Host is required")]
    public required string Host { get; init; }

    /// <summary>
    /// virtual host to use for the RabbitMQ connection.
    /// </summary>
    /// <remarks>
    /// A virtual host provides a way to segregate applications using the same RabbitMQ instance.
    /// Each virtual host has its own separate exchanges, queues, bindings, users, and permissions.
    /// 
    /// Default value: "/"
    /// Examples: "/development", "/production", "/myapp"
    /// </remarks>
    [Required(ErrorMessage = "Virtual host is required")]
    public required string VirtualHost { get; init; }

    /// <summary>
    /// username for authenticating with the RabbitMQ server.
    /// </summary>
    /// <remarks>
    /// Default 'guest' user can only connect from localhost. Use dedicated users for production.
    /// </remarks>
    [Required(ErrorMessage = "Username is required")]
    public required string Username { get; init; }

    /// <summary>
    /// password for authenticating with the RabbitMQ server.
    /// </summary>
    [Required(ErrorMessage = "Password is required")]
    public required string Password { get; init; }
}
