namespace Baseline.Sample.Infrastructure.Observability;

/// <summary>Explicit, independently selected telemetry destinations; every signal is disabled by default.</summary>
public sealed class SampleObservabilityOptions
{
    /// <summary>Configuration section the host may bind before registering this module.</summary>
    public const string SectionName = "Observability";

    /// <summary>Enables structured JSON console output through Serilog.</summary>
    public bool ConsoleEnabled { get; init; }

    /// <summary>Enables the Serilog Seq sink without selecting other destinations.</summary>
    public bool SeqEnabled { get; init; }

    /// <summary>Explicit HTTP(S) Seq ingestion server; no local or remote default is supplied.</summary>
    public string? SeqServerUrl { get; init; }

    /// <summary>Optional Seq ingestion credential supplied through an approved secret source.</summary>
    public string? SeqApiKey { get; init; }

    /// <summary>Enables Sentry ASP.NET Core exception capture, not Sentry log or performance export.</summary>
    public bool ErrorReportingEnabled { get; init; }

    /// <summary>Explicit Sentry DSN supplied through an approved configuration source.</summary>
    public string? SentryDsn { get; init; }

    /// <summary>Enables ASP.NET Core and HttpClient tracing through the OTLP HTTP/protobuf exporter.</summary>
    public bool TracingEnabled { get; init; }

    /// <summary>Explicit full OTLP traces URL, including the collector's traces path.</summary>
    public string? TracesEndpoint { get; init; }

    /// <summary>Parent-based trace sampling probability, applied only when tracing is enabled.</summary>
    public double TraceSampleRatio { get; init; } = 0.1;

    /// <summary>Enables ASP.NET Core and HttpClient metrics through the OTLP HTTP/protobuf exporter.</summary>
    public bool MetricsEnabled { get; init; }

    /// <summary>Explicit full OTLP metrics URL, including the collector's metrics path.</summary>
    public string? MetricsEndpoint { get; init; }

    /// <summary>Service identity used by enabled providers; not a destination or credential.</summary>
    public string ServiceName { get; init; } = "Baseline.Sample";

    /// <summary>Optional deployment release identity used by enabled providers.</summary>
    public string? ServiceVersion { get; init; }

    /// <summary>Optional deployment environment label used by enabled providers.</summary>
    public string? Environment { get; init; }
}
