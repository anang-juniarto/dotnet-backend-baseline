using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace Baseline.Sample.Grpc.Security;

/// <summary>Records the host-validated Development-only authentication opt-in.</summary>
/// <param name="Enabled">Whether local educational identity simulation is enabled.</param>
public sealed record DemoAuthenticationSettings(bool Enabled);

/// <summary>Fails closed unless the host explicitly enables local demo identities.</summary>
public sealed class DemoAuthenticationHandler(
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder,
    DemoAuthenticationSettings settings) : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    /// <summary>Accepts one bounded ASCII identity only for the explicitly enabled demo.</summary>
    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!settings.Enabled || !Request.Headers.TryGetValue("X-Demo-User", out var values))
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
