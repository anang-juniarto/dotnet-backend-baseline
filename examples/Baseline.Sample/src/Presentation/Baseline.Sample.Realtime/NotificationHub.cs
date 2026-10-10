using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Baseline.Sample.Realtime;

/// <summary>
/// Provides an authenticated, receive-only notification transport without client-callable send or group methods.
/// Chat membership, durable history, and notification inbox persistence are intentionally deferred.
/// </summary>
[Authorize]
public sealed class NotificationHub : Hub<INotificationClient>
{
    /// <summary>Rejects connections without an authenticated, usable server-resolved user identifier.</summary>
    /// <returns>The connection initialization operation.</returns>
    public override async Task OnConnectedAsync()
    {
        if (Context.User?.Identity?.IsAuthenticated != true || !NotificationOwner.IsValid(Context.UserIdentifier))
        {
            Context.Abort();
            throw new HubException("An authenticated user identifier is required.");
        }

        await base.OnConnectedAsync();
    }
}

/// <summary>Applies the same bounded user identifier checks at connection and dispatch boundaries.</summary>
internal static class NotificationOwner
{
    internal static bool IsValid(string? ownerId) =>
        !string.IsNullOrWhiteSpace(ownerId) &&
        ownerId.Length <= 256 &&
        string.Equals(ownerId, ownerId.Trim(), StringComparison.Ordinal) &&
        !ownerId.Any(char.IsControl);
}
