using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace Baseline.Sample.WebApi.Security;

/// <summary>Records whether the explicitly opted-in local demo authentication is available.</summary>
/// <param name="Enabled">True only when the host validated the Development-only opt-in.</param>
public sealed record DemoAuthenticationSettings(bool Enabled);

/// <summary>Local educational identity simulation, never a production authentication scheme.</summary>
public sealed class DemoAuthenticationHandler(
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder,
    DemoAuthenticationSettings settings) : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    /// <summary>Rejects disabled authentication and accepts only one bounded ASCII demo identity.</summary>
    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!settings.Enabled)
        {
            return Task.FromResult(AuthenticateResult.NoResult());
        }
        if (!Request.Headers.TryGetValue("X-Demo-User", out var values))
        {
            return Task.FromResult(AuthenticateResult.NoResult());
        }
        var user = values.Count == 1 ? values[0] : null;
        if (user is null || user.Length is < 1 or > 64 ||
            user.Any(c => !(char.IsAsciiLetterOrDigit(c) || c is '_' or '-')))
        {
            return Task.FromResult(AuthenticateResult.Fail("Invalid demo identity."));
        }
        var principal = new ClaimsPrincipal(new ClaimsIdentity(
            [new Claim(ClaimTypes.NameIdentifier, user)], Scheme.Name));
        return Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(principal, Scheme.Name)));
    }
}
