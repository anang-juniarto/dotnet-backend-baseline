using System.Reflection;
using Baseline.Sample.Realtime;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Baseline.Sample.IntegrationTests;

/// <summary>Verifies receive-only hub and mapped authorization contracts without starting a network listener.</summary>
public sealed class RealtimeBoundaryTests
{
    /// <summary>The hub requires authorization and adds no client-invocable send, broadcast or group operations.</summary>
    [Fact]
    public void NotificationHub_ReceiveOnlyContract_RequiresAuthorizationAndExposesNoClientMethods()
    {
        var hubType = typeof(NotificationHub);

        Assert.True(typeof(Hub).IsAssignableFrom(hubType));
        Assert.NotEmpty(hubType.GetCustomAttributes<AuthorizeAttribute>(inherit: true));
        var methods = hubType.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        var lifecycleMethod = Assert.Single(methods);
        Assert.Equal(nameof(Hub.OnConnectedAsync), lifecycleMethod.Name);
        Assert.NotEqual(hubType, lifecycleMethod.GetBaseDefinition().DeclaringType);
        Assert.Empty(hubType.GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly));
    }

    /// <summary>Both negotiation and hub endpoints carry authorization metadata when the module is explicitly mapped.</summary>
    [Fact]
    public async Task MapSampleRealtime_ExplicitlySelected_RequiresAuthorizationOnEveryEndpoint()
    {
        var builder = WebApplication.CreateBuilder(new WebApplicationOptions { Args = [] });
        builder.Services.AddAuthorization();
        builder.Services.AddSampleRealtime();
        await using var app = builder.Build();

        app.MapSampleRealtime();
        var endpoints = ((IEndpointRouteBuilder)app).DataSources
            .SelectMany(source => source.Endpoints).OfType<RouteEndpoint>().ToArray();

        Assert.Equal(2, endpoints.Length);
        Assert.Contains(endpoints, endpoint => endpoint.RoutePattern.RawText == RealtimeExtensions.NotificationHubPath);
        Assert.Contains(endpoints, endpoint => endpoint.RoutePattern.RawText == RealtimeExtensions.NotificationHubPath + "/negotiate");
        Assert.All(endpoints, endpoint =>
        {
            Assert.NotEmpty(endpoint.Metadata.GetOrderedMetadata<IAuthorizeData>());
            Assert.Null(endpoint.Metadata.GetMetadata<IAllowAnonymous>());
        });
    }
}
