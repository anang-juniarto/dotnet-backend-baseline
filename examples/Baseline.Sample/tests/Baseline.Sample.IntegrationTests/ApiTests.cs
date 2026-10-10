using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Baseline.Sample.IntegrationTests;

/// <summary>Checks transport validation and fail-closed educational authentication using an isolated in-process server.</summary>
public sealed class ApiTests
{
    /// <summary>Disabled demo authentication cannot be bypassed by sending a header.</summary>
    [Fact]
    public async Task AuthenticationIsDisabledByDefault()
    {
        await using var factory = new DemoFactory(false);
        using var client = factory.CreateClient();
        client.DefaultRequestHeaders.Add("X-Demo-User", "alice");
        var response = await client.GetAsync("/api/items/" + Guid.NewGuid(), TestContext.Current.CancellationToken);
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        var problem = await response.Content.ReadFromJsonAsync<JsonElement>(TestContext.Current.CancellationToken);
        Assert.Equal("unauthenticated", problem.GetProperty("errorCode").GetString());
        Assert.False(string.IsNullOrWhiteSpace(problem.GetProperty("traceId").GetString()));
    }

    /// <summary>The owner can retrieve a created DTO and other identities cannot discover it.</summary>
    [Fact]
    public async Task CreateGetAndOwnerIsolation()
    {
        await using var factory = new DemoFactory(true);
        using var client = factory.CreateClient();
        client.DefaultRequestHeaders.Add("X-Demo-User", "alice");
        var created = await client.PostAsJsonAsync("/api/items", new { name = " example " }, TestContext.Current.CancellationToken);
        Assert.Equal(HttpStatusCode.Created, created.StatusCode);
        Assert.NotNull(created.Headers.Location);
        var dto = await created.Content.ReadFromJsonAsync<JsonElement>(TestContext.Current.CancellationToken);
        Assert.Equal("example", dto.GetProperty("name").GetString());
        Assert.False(dto.TryGetProperty("ownerId", out _));
        var own = await client.GetAsync(created.Headers.Location, TestContext.Current.CancellationToken);
        Assert.Equal(HttpStatusCode.OK, own.StatusCode);
        client.DefaultRequestHeaders.Remove("X-Demo-User");
        client.DefaultRequestHeaders.Add("X-Demo-User", "bob");
        var other = await client.GetAsync(created.Headers.Location, TestContext.Current.CancellationToken);
        Assert.Equal(HttpStatusCode.NotFound, other.StatusCode);
        var problem = await other.Content.ReadFromJsonAsync<JsonElement>(TestContext.Current.CancellationToken);
        Assert.Equal("not_found", problem.GetProperty("errorCode").GetString());
        Assert.False(string.IsNullOrWhiteSpace(problem.GetProperty("traceId").GetString()));
    }

    /// <summary>Invalid display names become validation errors rather than stored records.</summary>
    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public async Task InvalidNamesReturnBadRequest(string name)
    {
        await using var factory = new DemoFactory(true);
        using var client = factory.CreateClient();
        client.DefaultRequestHeaders.Add("X-Demo-User", "alice");
        var response = await client.PostAsJsonAsync("/api/items", new { name }, TestContext.Current.CancellationToken);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType!.MediaType);
        var problem = await response.Content.ReadFromJsonAsync<JsonElement>(TestContext.Current.CancellationToken);
        Assert.Equal("validation_failed", problem.GetProperty("errorCode").GetString());
        Assert.False(string.IsNullOrWhiteSpace(problem.GetProperty("traceId").GetString()));
    }

    /// <summary>Names with 100 characters plus surrounding whitespace succeed after trimming.</summary>
    [Fact]
    public async Task NameWith100CharactersAndWhitespaceSucceedsWithTrimmedName()
    {
        await using var factory = new DemoFactory(true);
        using var client = factory.CreateClient();
        client.DefaultRequestHeaders.Add("X-Demo-User", "alice");
        var expectedName = new string('a', 100);
        var inputName = $"   {expectedName}   ";
        var response = await client.PostAsJsonAsync("/api/items", new { name = inputName }, TestContext.Current.CancellationToken);
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.NotNull(response.Headers.Location);
        var dto = await response.Content.ReadFromJsonAsync<JsonElement>(TestContext.Current.CancellationToken);
        Assert.Equal(expectedName, dto.GetProperty("name").GetString());
        var getResponse = await client.GetAsync(response.Headers.Location, TestContext.Current.CancellationToken);
        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);
        var fetchedDto = await getResponse.Content.ReadFromJsonAsync<JsonElement>(TestContext.Current.CancellationToken);
        Assert.Equal(expectedName, fetchedDto.GetProperty("name").GetString());
    }

    /// <summary>Names exceeding 100 characters after trimming fail validation.</summary>
    [Fact]
    public async Task NameExceeding100CharactersAfterTrimmingReturnsBadRequest()
    {
        await using var factory = new DemoFactory(true);
        using var client = factory.CreateClient();
        client.DefaultRequestHeaders.Add("X-Demo-User", "alice");
        var inputName = $"  {new string('a', 101)}  ";
        var response = await client.PostAsJsonAsync("/api/items", new { name = inputName }, TestContext.Current.CancellationToken);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType!.MediaType);
        var problem = await response.Content.ReadFromJsonAsync<JsonElement>(TestContext.Current.CancellationToken);
        Assert.Equal("validation_failed", problem.GetProperty("errorCode").GetString());
        Assert.False(string.IsNullOrWhiteSpace(problem.GetProperty("traceId").GetString()));
    }

    /// <summary>Missing or null name payloads fail transport validation.</summary>
    [Fact]
    public async Task MissingOrNullNamePayloadReturnsBadRequest()
    {
        await using var factory = new DemoFactory(true);
        using var client = factory.CreateClient();
        client.DefaultRequestHeaders.Add("X-Demo-User", "alice");
        var missingResponse = await client.PostAsJsonAsync("/api/items", new { }, TestContext.Current.CancellationToken);
        Assert.Equal(HttpStatusCode.BadRequest, missingResponse.StatusCode);
        Assert.Equal("application/problem+json", missingResponse.Content.Headers.ContentType!.MediaType);
        var missingProblem = await missingResponse.Content.ReadFromJsonAsync<JsonElement>(TestContext.Current.CancellationToken);
        Assert.Equal("validation_failed", missingProblem.GetProperty("errorCode").GetString());
        Assert.False(string.IsNullOrWhiteSpace(missingProblem.GetProperty("traceId").GetString()));

        var nullResponse = await client.PostAsJsonAsync("/api/items", new { name = (string?)null }, TestContext.Current.CancellationToken);
        Assert.Equal(HttpStatusCode.BadRequest, nullResponse.StatusCode);
        Assert.Equal("application/problem+json", nullResponse.Content.Headers.ContentType!.MediaType);
        var nullProblem = await nullResponse.Content.ReadFromJsonAsync<JsonElement>(TestContext.Current.CancellationToken);
        Assert.Equal("validation_failed", nullProblem.GetProperty("errorCode").GetString());
        Assert.False(string.IsNullOrWhiteSpace(nullProblem.GetProperty("traceId").GetString()));
    }

    /// <summary>Client-supplied owner values are ignored and ownership is bound to the authenticated principal.</summary>
    [Fact]
    public async Task OwnerSpoofingAttemptIsIgnoredAndItemIsOwnedByPrincipal()
    {
        await using var factory = new DemoFactory(true);
        using var client = factory.CreateClient();
        client.DefaultRequestHeaders.Add("X-Demo-User", "alice");
        var response = await client.PostAsJsonAsync("/api/items", new { name = "spoof", ownerId = "bob" }, TestContext.Current.CancellationToken);
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.NotNull(response.Headers.Location);
        var dto = await response.Content.ReadFromJsonAsync<JsonElement>(TestContext.Current.CancellationToken);
        Assert.Equal("spoof", dto.GetProperty("name").GetString());
        Assert.False(dto.TryGetProperty("ownerId", out _));

        var aliceResponse = await client.GetAsync(response.Headers.Location, TestContext.Current.CancellationToken);
        Assert.Equal(HttpStatusCode.OK, aliceResponse.StatusCode);

        client.DefaultRequestHeaders.Remove("X-Demo-User");
        client.DefaultRequestHeaders.Add("X-Demo-User", "bob");
        var bobResponse = await client.GetAsync(response.Headers.Location, TestContext.Current.CancellationToken);
        Assert.Equal(HttpStatusCode.NotFound, bobResponse.StatusCode);
        var problem = await bobResponse.Content.ReadFromJsonAsync<JsonElement>(TestContext.Current.CancellationToken);
        Assert.Equal("not_found", problem.GetProperty("errorCode").GetString());
        Assert.False(string.IsNullOrWhiteSpace(problem.GetProperty("traceId").GetString()));
    }

    /// <summary>Invalid simulated identity syntax fails authentication.</summary>
    [Theory]
    [InlineData("alice,bob")]
    [InlineData("alice bob")]
    public async Task InvalidDemoIdentityIsRejected(string user)
    {
        await using var factory = new DemoFactory(true);
        using var client = factory.CreateClient();
        client.DefaultRequestHeaders.Add("X-Demo-User", user);
        var response = await client.GetAsync("/api/items/" + Guid.NewGuid(), TestContext.Current.CancellationToken);
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    /// <summary>The educational scheme cannot start when enabled in a production environment.</summary>
    [Fact]
    public void ProductionOptInFailsStartup()
    {
        using var factory = new DemoFactory(true, "Production");
        var exception = Assert.Throws<InvalidOperationException>(() => factory.CreateClient());
        Assert.Contains("Development", exception.Message);
    }

    /// <summary>Configured Kestrel listeners cannot override the educational host's loopback restriction.</summary>
    [Fact]
    public void ExternalKestrelEndpointFailsStartup()
    {
        using var factory = new DemoFactory(true).WithWebHostBuilder(builder =>
            builder.UseSetting("Kestrel:Endpoints:Public:Url", "http://0.0.0.0:5090"));
        var exception = Assert.Throws<InvalidOperationException>(() => factory.CreateClient());
        Assert.Contains("loopback", exception.Message);
    }

    /// <summary>Runtime configuration cannot select a module omitted from the default host build.</summary>
    [Fact]
    public void UnsupportedStoreFailsStartup()
    {
        using var factory = new DemoFactory(true).WithWebHostBuilder(builder =>
            builder.UseSetting("Persistence:Provider", "unselected-provider"));
        var exception = Assert.Throws<InvalidOperationException>(() => factory.CreateClient());
        Assert.Contains("not compiled", exception.Message);
    }

    /// <summary>Realtime runtime opt-in is rejected when its project was not build-selected.</summary>
    [Fact]
    public void UncompiledRealtimeFailsStartup()
    {
        using var factory = new DemoFactory(true).WithWebHostBuilder(builder =>
            builder.UseSetting("Realtime:Enabled", "true"));
        var exception = Assert.Throws<InvalidOperationException>(() => factory.CreateClient());
        Assert.Contains("EnableRealtime", exception.Message);
    }

    /// <summary>Vendor telemetry settings cannot silently activate dependencies omitted at build time.</summary>
    [Fact]
    public void UncompiledObservabilityFailsStartup()
    {
        using var factory = new DemoFactory(true).WithWebHostBuilder(builder =>
            builder.UseSetting("Observability:ConsoleEnabled", "true"));
        var exception = Assert.Throws<InvalidOperationException>(() => factory.CreateClient());
        Assert.Contains("EnableObservability", exception.Message);
    }

    /// <summary>Sets host inputs before application configuration; each factory owns independent volatile storage.</summary>
    private sealed class DemoFactory(bool enabled, string environment = "Development") : WebApplicationFactory<Program>
    {
        /// <summary>Applies test-local settings without modifying process environment variables.</summary>
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment(environment);
            builder.UseSetting("DemoAuth:Enabled", enabled.ToString());
        }
    }
}
