using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenTelemetry.Exporter;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Sentry.Extensibility;
using Serilog;
using Serilog.Formatting.Json;

namespace Baseline.Sample.Infrastructure.Observability;

/// <summary>Registers only explicitly selected telemetry providers at the ASP.NET Core composition root.</summary>
public static class ObservabilityRegistration
{
    /// <summary>
    /// Validates a configuration snapshot before registering providers. When every signal is disabled,
    /// leaves all host services and logging providers unchanged. Call once before building the host.
    /// </summary>
    public static WebApplicationBuilder AddSampleObservability(
        this WebApplicationBuilder builder,
        SampleObservabilityOptions options)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(options);
        var result = new SampleObservabilityOptionsValidator().Validate(Options.DefaultName, options);
        if (result.Failed)
        {
            throw new OptionsValidationException(Options.DefaultName, typeof(SampleObservabilityOptions), result.Failures);
        }

        if (!options.ConsoleEnabled && !options.SeqEnabled && !options.ErrorReportingEnabled
            && !options.TracingEnabled && !options.MetricsEnabled)
        {
            return builder;
        }

        if (options.ConsoleEnabled || options.SeqEnabled)
        {
            builder.Logging.ClearProviders();
            builder.Services.AddSerilog((_, logger) =>
            {
                logger.MinimumLevel.Information()
                    .Enrich.FromLogContext()
                    .Enrich.WithProperty("ServiceName", options.ServiceName);
                if (options.ConsoleEnabled)
                {
                    logger.WriteTo.Console(new JsonFormatter(renderMessage: true));
                }

                if (options.SeqEnabled)
                {
                    logger.WriteTo.Seq(options.SeqServerUrl!, apiKey: options.SeqApiKey);
                }
            });
        }

        if (options.ErrorReportingEnabled)
        {
            builder.WebHost.UseSentry(sentry =>
            {
                sentry.Dsn = options.SentryDsn;
                sentry.Environment = options.Environment;
                sentry.Release = options.ServiceVersion;
                sentry.SendDefaultPii = false;
                sentry.MaxRequestBodySize = RequestSize.None;
                sentry.MaxBreadcrumbs = 0;
                sentry.MinimumEventLevel = LogLevel.None;
                sentry.MinimumBreadcrumbLevel = LogLevel.None;
                sentry.TracesSampleRate = 0;
                sentry.EnableLogs = false;
                sentry.AutoSessionTracking = false;
            });
        }

        if (options.TracingEnabled || options.MetricsEnabled)
        {
            var telemetry = builder.Services.AddOpenTelemetry()
                .ConfigureResource(resource =>
                {
                    resource.AddService(options.ServiceName, serviceVersion: options.ServiceVersion);
                    if (!string.IsNullOrWhiteSpace(options.Environment))
                    {
                        resource.AddAttributes(new Dictionary<string, object>
                        {
                            ["deployment.environment.name"] = options.Environment
                        });
                    }
                });

            if (options.TracingEnabled)
            {
                telemetry.WithTracing(tracing => tracing
                    .SetSampler(new ParentBasedSampler(new TraceIdRatioBasedSampler(options.TraceSampleRatio)))
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddOtlpExporter(exporter => ConfigureExporter(exporter, options.TracesEndpoint!)));
            }

            if (options.MetricsEnabled)
            {
                telemetry.WithMetrics(metrics => metrics
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddOtlpExporter(exporter => ConfigureExporter(exporter, options.MetricsEndpoint!)));
            }
        }

        return builder;
    }

    private static void ConfigureExporter(OtlpExporterOptions exporter, string endpoint)
    {
        exporter.Protocol = OtlpExportProtocol.HttpProtobuf;
        exporter.Endpoint = new Uri(endpoint, UriKind.Absolute);
        exporter.TimeoutMilliseconds = 5000;
    }
}
