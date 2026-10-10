# Optional observability adapter

This `net10.0` module registers real Serilog console/Seq sinks, Sentry ASP.NET Core error capture, and OpenTelemetry ASP.NET Core/HttpClient traces and metrics. Package versions are owned by the sample's `Directory.Packages.props`; build compatibility is not certification of any remote provider.

## Host integration

Reference this project, then call once before `builder.Build()`:

```csharp
using Baseline.Sample.Infrastructure.Observability;

var telemetry = builder.Configuration
    .GetSection(SampleObservabilityOptions.SectionName)
    .Get<SampleObservabilityOptions>() ?? new SampleObservabilityOptions();
builder.AddSampleObservability(telemetry);
```

No middleware mapping is required by this adapter; `UseSentry` installs the SDK's ASP.NET Core integration. Do not add a Sentry Serilog sink, separately initialize `SentrySdk`, or manually capture exceptions already owned by SDK middleware. Handled exceptions in the host's exception middleware require a deliberate capture policy and isolated tests; this module does not change .NET 10 handled-exception diagnostic suppression.

## `Observability` keys

| Key | Default | Selection contract |
|---|---|---|
| `ConsoleEnabled` | `false` | Serilog JSON console output |
| `SeqEnabled` | `false` | Requires explicit `SeqServerUrl`; optional secret `SeqApiKey` |
| `ErrorReportingEnabled` | `false` | Requires explicit `SentryDsn`; SDK exception capture only |
| `TracingEnabled` | `false` | Requires explicit full `TracesEndpoint` including OTLP path |
| `MetricsEnabled` | `false` | Requires explicit full `MetricsEndpoint` including OTLP path |
| `TraceSampleRatio` | `0.1` | Parent-based trace sampling, finite range zero through one |
| `ServiceName` | `Baseline.Sample` | Nonempty when any signal is enabled |
| `ServiceVersion` | unset | Optional release/resource identity |
| `Environment` | unset | Optional deployment/resource identity |

Every switch is independent. Destinations have no defaults and must be absolute HTTP(S) URLs. OTLP uses HTTP/protobuf with a five-second export timeout; use TLS and an approved collector/network security boundary. OTLP authentication headers are deliberately not part of this minimal adapter. Disabled signal settings are ignored by validation. With all switches off, registration returns without altering the service collection or host logging providers and without constructing SDKs, sinks or exporters.

When console or Seq is selected, the adapter replaces host logging providers with the selected Serilog destinations at Information level; otherwise host logging remains unchanged. It does not read arbitrary Serilog/Sentry sink configuration or register an OTLP logs exporter. Sentry log events, breadcrumbs, request bodies, default PII, session tracking and performance sampling are disabled. Enabled SDKs may export when the host runs: never enable them in local tests with real credentials or destinations.

## Verification boundary

Standalone restore/build verifies the exact published packages and APIs against .NET 10. No endpoint, live service, exporter delivery, privacy scrubbing, collector outage, handled-error capture count or shutdown behavior has been runtime-certified. Host owners must review sensitive exception/log/span attributes, redaction, retention, bounded exporter queues and failure ownership before deployment. Disabled-path and invalid-options regression tests belong in the host's test suite. No service installation, migration, remote provisioning or endpoint configuration is performed here.
