using Hangfire;
using Hangfire.Common;
using Hangfire.States;
using Microsoft.Extensions.Logging;

namespace Baseline.Sample.Infrastructure.BackgroundJobs;

/// <summary>Enqueues only a concrete, harmless diagnostic job, not arbitrary types or expressions.</summary>
/// <remarks>Each call creates a new storage-assigned job ID. Retries may duplicate jobs; enqueue is not atomic with business changes.</remarks>
public sealed class SampleJobScheduler
{
    private readonly IBackgroundJobClient client;

    /// <summary>Uses Hangfire's SQL-backed client without starting a processing server.</summary>
    public SampleJobScheduler(IBackgroundJobClient client)
    {
        ArgumentNullException.ThrowIfNull(client);
        this.client = client;
    }

    /// <summary>Persists a diagnostic job and returns its Hangfire ID; no worker is automatically started.</summary>
    /// <remarks>Cancellation is checked before the synchronous storage call; an interrupted call can have an unknown enqueue outcome.</remarks>
    public string EnqueueDiagnostic(Guid operationId, CancellationToken cancellationToken = default)
    {
        if (operationId == Guid.Empty)
        {
            throw new ArgumentException("A nonempty correlation identifier is required.", nameof(operationId));
        }

        cancellationToken.ThrowIfCancellationRequested();
        return client.Create(
            Job.FromExpression<SampleDiagnosticJob>(job => job.ExecuteAsync(operationId, CancellationToken.None)),
            new EnqueuedState("baseline_diagnostics"))
            ?? throw new InvalidOperationException("Hangfire did not create the diagnostic job.");
    }
}

/// <summary>Safe sample job that writes only a correlation identifier and never mutates business data.</summary>
public sealed class SampleDiagnosticJob
{
    private readonly ILogger<SampleDiagnosticJob> logger;

    /// <summary>Uses host-provided logging without capturing request-scoped business state.</summary>
    public SampleDiagnosticJob(ILogger<SampleDiagnosticJob> logger)
    {
        ArgumentNullException.ThrowIfNull(logger);
        this.logger = logger;
    }

    /// <summary>Emits a diagnostic message when a separately configured worker executes this job.</summary>
    [AutomaticRetry(Attempts = 0)]
    public Task ExecuteAsync(Guid operationId, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        if (operationId == Guid.Empty)
        {
            throw new ArgumentException("A nonempty correlation identifier is required.", nameof(operationId));
        }

        logger.LogInformation("Sample diagnostic job executed for operation {OperationId}.", operationId);
        return Task.CompletedTask;
    }
}
