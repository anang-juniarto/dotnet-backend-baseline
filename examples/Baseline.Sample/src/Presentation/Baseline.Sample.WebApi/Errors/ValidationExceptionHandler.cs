using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Baseline.Sample.WebApi.Errors;

/// <summary>Maps application validation failures to the existing public HTTP validation contract.</summary>
public sealed class ValidationExceptionHandler(IProblemDetailsService problemDetails) : IExceptionHandler
{
    /// <summary>Handles only validator failures and leaves unexpected exceptions to the standard handler.</summary>
    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not ValidationException validation)
        {
            return false;
        }

        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        var errors = validation.Errors.GroupBy(failure => failure.PropertyName)
            .ToDictionary(group => group.Key, group => group.Select(failure => failure.ErrorMessage).Distinct().ToArray());
        var details = new ValidationProblemDetails(errors)
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "One or more validation errors occurred."
        };
        await problemDetails.WriteAsync(new ProblemDetailsContext { HttpContext = context, ProblemDetails = details });
        return true;
    }
}
