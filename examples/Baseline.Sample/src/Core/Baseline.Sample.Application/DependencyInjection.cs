using Baseline.Sample.Application.Common.Behaviors;
using Baseline.Sample.Application.Features.Items.Commands.CreateItem;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Baseline.Sample.Application;

/// <summary>Registers application handlers and transport-independent request policies.</summary>
public static class DependencyInjection
{
    /// <summary>Adds the verified MediatR pipeline and explicit validators without provider coupling.</summary>
    public static IServiceCollection AddSampleApplication(this IServiceCollection services)
    {
        services.AddLogging();
        services.AddTransient<IValidator<CreateItemCommand>, CreateItemCommandValidator>();
        services.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssemblyContaining<CreateItemCommand>();
            configuration.AddOpenBehavior(typeof(LoggingBehavior<,>));
            configuration.AddOpenBehavior(typeof(TracingBehavior<,>));
            configuration.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });
        return services;
    }
}
