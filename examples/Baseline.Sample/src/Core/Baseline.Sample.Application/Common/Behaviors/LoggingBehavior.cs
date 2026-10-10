using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Baseline.Sample.Application.Common.Behaviors;

/// <summary>Records request type and elapsed time without logging payloads or actor identities.</summary>
public sealed class LoggingBehavior<TRequest, TResponse>(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    /// <summary>Reports success, cancellation and failure without serializing potentially sensitive exceptions.</summary>
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var started = Stopwatch.GetTimestamp();
        var outcome = "failed";
        try
        {
            var response = await next();
            outcome = "completed";
            return response;
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            outcome = "cancelled";
            throw;
        }
        finally
        {
            logger.LogInformation("Application request {RequestType} {Outcome} in {ElapsedMilliseconds} ms",
                typeof(TRequest).Name, outcome, Stopwatch.GetElapsedTime(started).TotalMilliseconds);
        }
    }
}
