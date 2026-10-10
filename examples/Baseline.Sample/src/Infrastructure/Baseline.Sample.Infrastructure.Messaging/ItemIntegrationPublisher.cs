using Baseline.Sample.Infrastructure.Messaging.Contracts.V1;
using MassTransit;

namespace Baseline.Sample.Infrastructure.Messaging;

/// <summary>Publishes explicit integration notifications independently of business persistence.</summary>
/// <remarks>No outbox or atomic business commit is implemented. Retrying an ambiguous publish may duplicate a message.</remarks>
public sealed class ItemIntegrationPublisher
{
    private readonly IPublishEndpoint publishEndpoint;

    /// <summary>Uses the scoped MassTransit publishing endpoint.</summary>
    public ItemIntegrationPublisher(IPublishEndpoint publishEndpoint)
    {
        ArgumentNullException.ThrowIfNull(publishEndpoint);
        this.publishEndpoint = publishEndpoint;
    }

    /// <summary>Publishes a validated version-one event; completion does not prove consumer receipt or business commit.</summary>
    public Task PublishAsync(ItemCreatedV1 message, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(message);
        if (message.EventId == Guid.Empty || message.ItemId == Guid.Empty || message.OccurredAtUtc == default
            || message.OccurredAtUtc.Offset != TimeSpan.Zero)
        {
            throw new ArgumentException("Event and item identifiers and a UTC occurrence timestamp are required.", nameof(message));
        }

        cancellationToken.ThrowIfCancellationRequested();
        return publishEndpoint.Publish(message, context => context.MessageId = message.EventId, cancellationToken);
    }
}
