using Microsoft.Extensions.Options;

namespace Baseline.Sample.Infrastructure.Observability;

/// <summary>Validates only selected signals without constructing providers or contacting destinations.</summary>
public sealed class SampleObservabilityOptionsValidator : IValidateOptions<SampleObservabilityOptions>
{
    /// <summary>Reports invalid explicit destinations and sampling settings before provider registration.</summary>
    public ValidateOptionsResult Validate(string? name, SampleObservabilityOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        var failures = new List<string>();
        var enabled = options.ConsoleEnabled || options.SeqEnabled || options.ErrorReportingEnabled
            || options.TracingEnabled || options.MetricsEnabled;

        if (enabled && string.IsNullOrWhiteSpace(options.ServiceName))
        {
            failures.Add("Observability:ServiceName is required when any signal is enabled.");
        }

        if (options.SeqEnabled && !IsHttpEndpoint(options.SeqServerUrl))
        {
            failures.Add("Observability:SeqServerUrl must be an explicit absolute HTTP(S) URL without credentials, query or fragment.");
        }

        if (options.ErrorReportingEnabled && (!Uri.TryCreate(options.SentryDsn, UriKind.Absolute, out var dsn)
            || (dsn.Scheme != Uri.UriSchemeHttps && dsn.Scheme != Uri.UriSchemeHttp)
            || string.IsNullOrWhiteSpace(dsn.Host) || string.IsNullOrWhiteSpace(dsn.UserInfo)
            || dsn.AbsolutePath.Trim('/') is not { Length: > 0 }
            || !string.IsNullOrEmpty(dsn.Query) || !string.IsNullOrEmpty(dsn.Fragment)))
        {
            failures.Add("Observability:SentryDsn must be an explicit HTTP(S) DSN containing a public key and project path.");
        }

        if (options.TracingEnabled)
        {
            if (!IsHttpEndpoint(options.TracesEndpoint))
            {
                failures.Add("Observability:TracesEndpoint must be an explicit full HTTP(S) OTLP traces URL without credentials, query or fragment.");
            }

            if (!double.IsFinite(options.TraceSampleRatio) || options.TraceSampleRatio is < 0 or > 1)
            {
                failures.Add("Observability:TraceSampleRatio must be a finite number between zero and one.");
            }
        }

        if (options.MetricsEnabled && !IsHttpEndpoint(options.MetricsEndpoint))
        {
            failures.Add("Observability:MetricsEndpoint must be an explicit full HTTP(S) OTLP metrics URL without credentials, query or fragment.");
        }

        return failures.Count == 0 ? ValidateOptionsResult.Success : ValidateOptionsResult.Fail(failures);
    }

    private static bool IsHttpEndpoint(string? value) =>
        Uri.TryCreate(value, UriKind.Absolute, out var uri)
        && (uri.Scheme == Uri.UriSchemeHttps || uri.Scheme == Uri.UriSchemeHttp)
        && !string.IsNullOrWhiteSpace(uri.Host)
        && string.IsNullOrEmpty(uri.UserInfo)
        && string.IsNullOrEmpty(uri.Query)
        && string.IsNullOrEmpty(uri.Fragment);
}
