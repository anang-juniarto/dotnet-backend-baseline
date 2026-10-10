using Hangfire;
using Hangfire.SqlServer;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;

namespace Baseline.Sample.Infrastructure.BackgroundJobs;

/// <summary>Explicit SQL Server job storage selection; no server or dashboard is started.</summary>
public sealed class HangfireBackgroundJobsOptions
{
    /// <summary>Leaves registration inert unless enabled by an explicit caller.</summary>
    public bool Enabled { get; set; }

    /// <summary>Secret connection string for an independently provisioned Hangfire schema.</summary>
    public string ConnectionString { get; set; } = string.Empty;

    /// <summary>Pre-provisioned schema; this module never creates or upgrades it.</summary>
    public string SchemaName { get; set; } = "HangFire";
}

/// <summary>Opt-in Hangfire SQL Server client registration without processing or automatic scheduling.</summary>
public static class BackgroundJobsRegistration
{
    /// <summary>Registers storage and an explicit scheduler only; schema provisioning and worker hosting remain separate approvals.</summary>
    public static IServiceCollection AddSampleHangfireBackgroundJobs(
        this IServiceCollection services,
        Action<HangfireBackgroundJobsOptions> configure)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configure);
        var options = new HangfireBackgroundJobsOptions();
        configure(options);
        if (!options.Enabled)
        {
            return services;
        }

        ArgumentException.ThrowIfNullOrWhiteSpace(options.ConnectionString);
        ArgumentException.ThrowIfNullOrWhiteSpace(options.SchemaName);
        if (options.SchemaName.Length > 128 || options.SchemaName.Any(character => !char.IsAsciiLetterOrDigit(character) && character != '_'))
        {
            throw new ArgumentException("Schema must be an ASCII identifier of at most 128 characters.", nameof(configure));
        }

        var connectionString = options.ConnectionString;
        var schemaName = options.SchemaName;
        services.AddHangfire(configuration => configuration
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseSqlServerStorage(connectionString, new SqlServerStorageOptions
            {
                SchemaName = schemaName,
                SqlClientFactory = SqlClientFactory.Instance,
                PrepareSchemaIfNecessary = false,
                TryAutoDetectSchemaDependentOptions = false,
                EnableHeavyMigrations = false,
                UseRecommendedIsolationLevel = true,
                SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                CommandBatchMaxTimeout = TimeSpan.FromMinutes(1)
            }));
        services.AddTransient<SampleDiagnosticJob>();
        services.AddTransient<SampleJobScheduler>();
        return services;
    }
}
