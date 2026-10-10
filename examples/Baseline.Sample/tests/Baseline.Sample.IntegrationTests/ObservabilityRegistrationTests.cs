using Baseline.Sample.Infrastructure.Observability;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;

namespace Baseline.Sample.IntegrationTests;

/// <summary>Checks telemetry opt-in boundaries without constructing exporters or contacting destinations.</summary>
public sealed class ObservabilityRegistrationTests
{
    /// <summary>Disabled telemetry preserves every existing host descriptor, including logging providers.</summary>
    [Fact]
    public void AddSampleObservability_AllSignalsDisabled_LeavesServicesUnchanged()
    {
        var builder = WebApplication.CreateBuilder(new WebApplicationOptions { Args = [] });
        var descriptors = builder.Services.ToArray();

        var result = builder.AddSampleObservability(new SampleObservabilityOptions());

        Assert.Same(builder, result);
        AssertDescriptorsUnchanged(descriptors, builder.Services);
    }

    /// <summary>Each selected destination is validated before any provider can modify host registrations.</summary>
    [Theory]
    [InlineData("Seq")]
    [InlineData("Sentry")]
    [InlineData("Traces")]
    [InlineData("Metrics")]
    public void AddSampleObservability_InvalidEnabledDestination_RejectsBeforeRegistration(string destination)
    {
        var builder = WebApplication.CreateBuilder(new WebApplicationOptions { Args = [] });
        var descriptors = builder.Services.ToArray();
        var options = new SampleObservabilityOptions
        {
            SeqEnabled = destination == "Seq",
            SeqServerUrl = "not-an-absolute-url",
            ErrorReportingEnabled = destination == "Sentry",
            SentryDsn = "not-an-absolute-url",
            TracingEnabled = destination == "Traces",
            TracesEndpoint = "not-an-absolute-url",
            MetricsEnabled = destination == "Metrics",
            MetricsEndpoint = "not-an-absolute-url"
        };

        var exception = Assert.Throws<OptionsValidationException>(() => builder.AddSampleObservability(options));

        Assert.Equal(typeof(SampleObservabilityOptions), exception.OptionsType);
        Assert.Contains(exception.Failures, failure => failure.Contains(destination, StringComparison.Ordinal));
        AssertDescriptorsUnchanged(descriptors, builder.Services);
    }

    /// <summary>Compares descriptor identity and ordering rather than only the number of registrations.</summary>
    private static void AssertDescriptorsUnchanged(ServiceDescriptor[] expected, IServiceCollection actual)
    {
        Assert.Equal(expected.Length, actual.Count);
        for (var index = 0; index < expected.Length; index++)
            Assert.Same(expected[index], actual[index]);
    }
}
