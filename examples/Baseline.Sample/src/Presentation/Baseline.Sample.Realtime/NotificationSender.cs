using Microsoft.AspNetCore.SignalR;

namespace Baseline.Sample.Realtime;

/// <summary>Dispatches transient notifications from trusted server workflows, never directly from hub clients.</summary>
public sealed class NotificationSender
{
    /// <summary>Holds the framework-managed context rather than a connection-scoped hub instance.</summary>
    private readonly IHubContext<NotificationHub> hubContext;

    /// <summary>Creates a sender using the framework-managed hub context.</summary>
    /// <param name="hubContext">The notification hub context.</param>
    public NotificationSender(IHubContext<NotificationHub> hubContext)
    {
        ArgumentNullException.ThrowIfNull(hubContext);
        this.hubContext = hubContext;
    }

    /// <summary>
    /// Sends only when the server-resolved recipient matches the trusted resource owner exactly.
    /// Both identifiers must come from authorized server state, not client-submitted recipient fields.
    /// The host owns globally unique user identifiers and tenant isolation; this check is not an authorization lookup.
    /// Success means only transport dispatch, not delivery, storage, or client acknowledgment.
    /// </summary>
    /// <param name="recipientOwnerId">The authorized recipient resolved by the server in the hub user identifier namespace.</param>
    /// <param name="resourceOwnerId">The owner loaded from trusted resource state by the server.</param>
    /// <param name="notification">A non-sensitive invalidation hint.</param>
    /// <param name="cancellationToken">Cancels the transport send.</param>
    /// <returns>The transient send operation; disconnected users have no replay or inbox.</returns>
    public Task NotifyUserAsync(
        string recipientOwnerId,
        string resourceOwnerId,
        NotificationMessage notification,
        CancellationToken cancellationToken = default)
    {
        if (!NotificationOwner.IsValid(recipientOwnerId))
        {
            throw new ArgumentException("A valid server-resolved recipient is required.", nameof(recipientOwnerId));
        }

        if (!NotificationOwner.IsValid(resourceOwnerId))
        {
            throw new ArgumentException("A valid server-resolved resource owner is required.", nameof(resourceOwnerId));
        }

        if (!string.Equals(recipientOwnerId, resourceOwnerId, StringComparison.Ordinal))
        {
            throw new UnauthorizedAccessException("The recipient must match the resource owner.");
        }

        ArgumentNullException.ThrowIfNull(notification);
        cancellationToken.ThrowIfCancellationRequested();

        return hubContext.Clients.User(recipientOwnerId).SendAsync(
            nameof(INotificationClient.ReceiveNotification), notification, cancellationToken);
    }
}
