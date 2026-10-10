namespace Baseline.Sample.Realtime;

/// <summary>
/// Carries a non-sensitive invalidation hint; clients must reload authorized data through the API.
/// This transport does not persist notifications, inbox entries, or chat history.
/// </summary>
public sealed class NotificationMessage
{
    /// <summary>Creates a bounded hint without resource contents, credentials, or recipient identifiers.</summary>
    /// <param name="eventId">A server-generated identifier for client-side duplicate suppression, not a delivery receipt.</param>
    /// <param name="kind">A server-defined ASCII event label containing letters, digits, dots, underscores, or hyphens.</param>
    public NotificationMessage(Guid eventId, string kind)
    {
        if (eventId == Guid.Empty)
        {
            throw new ArgumentException("An event identifier is required.", nameof(eventId));
        }

        ArgumentException.ThrowIfNullOrWhiteSpace(kind);
        if (kind.Length > 64 || kind.Any(character =>
                !char.IsAsciiLetterOrDigit(character) && character is not '.' and not '_' and not '-'))
        {
            throw new ArgumentException("The event kind must be a bounded ASCII label.", nameof(kind));
        }

        EventId = eventId;
        Kind = kind;
    }

    /// <summary>Gets the event identifier; it does not prove delivery or durable processing.</summary>
    public Guid EventId { get; }

    /// <summary>Gets the non-sensitive event label used to trigger an authorized API refresh.</summary>
    public string Kind { get; }
}

/// <summary>Defines the server-to-client wire contract without depending on Application types.</summary>
public interface INotificationClient
{
    /// <summary>Receives a transient hint; clients must tolerate duplicates, loss, and reconnect gaps.</summary>
    /// <param name="notification">The bounded invalidation hint.</param>
    /// <returns>The transport send operation, not an acknowledgment of client processing.</returns>
    Task ReceiveNotification(NotificationMessage notification);
}
