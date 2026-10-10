using Baseline.Sample.Application.Items;
using Baseline.Sample.Persistence.InMemory.Items;
#if SAMPLE_SQLSERVER
using Baseline.Sample.Persistence.SqlServer;
#endif
#if SAMPLE_POSTGRESQL
using Baseline.Sample.Persistence.PostgreSql;
#endif
#if SAMPLE_OBSERVABILITY
using Baseline.Sample.Infrastructure.Observability;
#endif
#if SAMPLE_REALTIME
using Baseline.Sample.Realtime;
#endif

namespace Baseline.Sample.WebApi.Extensions;

/// <summary>Wires only build-selected optional modules, with memory and no external telemetry as defaults.</summary>
public static class SelectedStackRegistration
{
    /// <summary>Validates runtime selections against compiled capabilities without connecting or migrating stores.</summary>
    public static WebApplicationBuilder AddSelectedStack(this WebApplicationBuilder builder)
    {
        var provider = builder.Configuration["Persistence:Provider"] ?? "memory";
        switch (provider.ToLowerInvariant())
        {
            case "memory":
                builder.Services.AddSingleton<IItemStore, InMemoryItemStore>();
                break;
#if SAMPLE_SQLSERVER
            case "sqlserver":
                builder.Services.AddSqlServerItemStore(RequiredConnection(builder.Configuration));
                break;
#endif
#if SAMPLE_POSTGRESQL
            case "postgresql":
                builder.Services.AddPostgreSqlItemStore(RequiredConnection(builder.Configuration));
                break;
#endif
            default:
                throw new InvalidOperationException("The selected persistence provider is not compiled into this host.");
        }
#if SAMPLE_OBSERVABILITY
        builder.AddSampleObservability(builder.Configuration.GetSection(SampleObservabilityOptions.SectionName)
            .Get<SampleObservabilityOptions>() ?? new SampleObservabilityOptions());
#else
        if (builder.Configuration.GetSection("Observability").GetChildren().Any())
        {
            throw new InvalidOperationException("Observability configuration requires the EnableObservability build selection.");
        }
#endif
        if (builder.Configuration.GetValue<bool>("Realtime:Enabled"))
        {
#if SAMPLE_REALTIME
            builder.Services.AddSampleRealtime();
#else
            throw new InvalidOperationException("Realtime configuration requires the EnableRealtime build selection.");
#endif
        }
        return builder;
    }

    /// <summary>Maps the optional authenticated receive-only notification hub only when selected at build and runtime.</summary>
    public static WebApplication MapSelectedStack(this WebApplication app)
    {
#if SAMPLE_REALTIME
        if (app.Configuration.GetValue<bool>("Realtime:Enabled"))
        {
            app.MapSampleRealtime();
        }
#endif
        return app;
    }

#if SAMPLE_SQLSERVER || SAMPLE_POSTGRESQL
    private static string RequiredConnection(IConfiguration configuration) =>
        configuration.GetConnectionString("Primary") is { Length: > 0 } value
            ? value
            : throw new InvalidOperationException("The selected store requires ConnectionStrings:Primary.");
#endif
}
