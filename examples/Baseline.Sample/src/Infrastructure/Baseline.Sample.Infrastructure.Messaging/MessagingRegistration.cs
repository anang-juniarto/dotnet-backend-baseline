using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace Baseline.Sample.Infrastructure.Messaging;

/// <summary>Explicit RabbitMQ connection selection; credentials must come from an external secret provider.</summary>
public sealed class RabbitMqMessagingOptions
{
    /// <summary>Enables this module only when its registration extension is explicitly invoked.</summary>
    public bool Enabled { get; set; }

    /// <summary>Broker DNS name without credentials or a URI scheme.</summary>
    public string Host { get; set; } = string.Empty;

    /// <summary>Broker port; the default is the TLS listener.</summary>
    public ushort Port { get; set; } = 5671;

    /// <summary>Pre-provisioned virtual host accessible to the publishing identity.</summary>
    public string VirtualHost { get; set; } = "/";

    /// <summary>Least-privilege broker identity.</summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>Secret broker password, never included in published messages.</summary>
    public string Password { get; set; } = string.Empty;

    /// <summary>Requires TLS by default; plaintext is limited to loopback development brokers.</summary>
    public bool UseTls { get; set; } = true;
}

/// <summary>Opt-in publisher-only MassTransit registration without consumers or an outbox.</summary>
public static class MessagingRegistration
{
    /// <summary>Registers a bus only when enabled; an explicitly composed host owns its start/stop lifecycle.</summary>
    /// <remarks>No consumers, receive endpoints, business transaction coupling or durable delivery from the memory sample are provided.</remarks>
    public static IServiceCollection AddSampleRabbitMqMessaging(
        this IServiceCollection services,
        Action<RabbitMqMessagingOptions> configure)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configure);
        var options = new RabbitMqMessagingOptions();
        configure(options);
        if (!options.Enabled)
        {
            return services;
        }

        ArgumentException.ThrowIfNullOrWhiteSpace(options.Host);
        ArgumentException.ThrowIfNullOrWhiteSpace(options.VirtualHost);
        ArgumentException.ThrowIfNullOrWhiteSpace(options.Username);
        ArgumentException.ThrowIfNullOrWhiteSpace(options.Password);
        if (Uri.CheckHostName(options.Host) == UriHostNameType.Unknown || options.Port == 0)
        {
            throw new ArgumentException("A valid broker hostname and nonzero port are required.", nameof(configure));
        }

        var loopback = string.Equals(options.Host, "localhost", StringComparison.OrdinalIgnoreCase)
            || (System.Net.IPAddress.TryParse(options.Host, out var address) && System.Net.IPAddress.IsLoopback(address));
        if (!options.UseTls && !loopback)
        {
            throw new ArgumentException("Plaintext messaging is limited to loopback development brokers.", nameof(configure));
        }

        var host = options.Host;
        var port = options.Port;
        var virtualHost = options.VirtualHost;
        var username = options.Username;
        var password = options.Password;
        var useTls = options.UseTls;
        services.AddMassTransit(bus => bus.UsingRabbitMq((_, transport) =>
        {
            transport.Host(host, port, virtualHost, connection =>
            {
                connection.Username(username);
                connection.Password(password);
                if (useTls)
                {
                    connection.UseSsl(ssl => ssl.ServerName = host);
                }
            });
            transport.Message<Contracts.V1.ItemCreatedV1>(message => message.SetEntityName("baseline.sample.item-created.v1"));
        }));
        services.AddScoped<ItemIntegrationPublisher>();
        return services;
    }
}
