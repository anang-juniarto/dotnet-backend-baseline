using System.Diagnostics;
using MediatR;

namespace Baseline.Sample.Application.Common.Behaviors;

/// <summary>Enriches the existing transport activity instead of duplicating its span.</summary>
public sealed class TracingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    /// <summary>Adds a bounded request-type event without payloads, user identifiers or vendor dependencies.</summary>
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        Activity.Current?.AddEvent(new ActivityEvent("application.request", tags: new ActivityTagsCollection
        {
            { "application.request.type", typeof(TRequest).Name }
        }));
        return await next();
    }
}
