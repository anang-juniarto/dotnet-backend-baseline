using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Baseline.Sample.Realtime;

/// <summary>Provides explicit opt-in registration and endpoint mapping for notification transport only.</summary>
public static class RealtimeExtensions
{
    /// <summary>Gets the stable authenticated notification hub route.</summary>
    public const string NotificationHubPath = "/hubs/notifications";

    /// <summary>
    /// Registers SignalR and the server-only sender without authentication schemes, external SDKs, or durable storage.
    /// The host owns authentication, authorization services, unique user identifiers, origin restrictions,
    /// and scoped token handling; no query-string token extraction is installed by this module.
    /// </summary>
    /// <param name="services">The host service collection.</param>
    /// <returns>The service collection for composition-root chaining.</returns>
    public static IServiceCollection AddSampleRealtime(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);
        services.AddSignalR();
        services.TryAddSingleton<NotificationSender>();
        return services;
    }

    /// <summary>
    /// Maps the receive-only hub with authorization and disconnects connections when authentication expires.
    /// The host must configure authentication and authorization middleware and enforce allowed origins
    /// for WebSockets as well as HTTP; mapping alone does not implement those policies.
    /// </summary>
    /// <param name="endpoints">The host endpoint route builder.</param>
    /// <returns>The convention builder for additional host-owned endpoint policies.</returns>
    public static HubEndpointConventionBuilder MapSampleRealtime(this IEndpointRouteBuilder endpoints)
    {
        ArgumentNullException.ThrowIfNull(endpoints);
        var hub = endpoints.MapHub<NotificationHub>(NotificationHubPath, options =>
        {
            options.CloseOnAuthenticationExpiration = true;
        });
        hub.RequireAuthorization();
        return hub;
    }
}
