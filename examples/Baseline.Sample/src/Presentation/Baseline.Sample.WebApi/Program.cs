using Baseline.Sample.Application;
using Baseline.Sample.WebApi.Extensions;
using Baseline.Sample.WebApi.Security;

var builder = WebApplication.CreateBuilder(args);
var demoEnabled = builder.Configuration.GetValue<bool>("DemoAuth:Enabled");
if (demoEnabled && !builder.Environment.IsDevelopment())
{
    throw new InvalidOperationException("Demo authentication is allowed only in Development.");
}

// Reject configured listeners, which would take precedence over hosting URL defaults.
if (builder.Configuration.GetSection("Kestrel:Endpoints").GetChildren().Any())
{
    throw new InvalidOperationException("Configured Kestrel endpoints are not supported by this loopback-only demo.");
}
// Code-owned endpoints override configurable hosting URLs; no external listeners are permitted.
builder.WebHost.ConfigureKestrel(options => options.Listen(System.Net.IPAddress.Loopback, 5080));
builder.WebHost.UseUrls("http://127.0.0.1:5080");
builder.Services.AddSingleton(new DemoAuthenticationSettings(demoEnabled));
builder.Services.AddAuthentication("Demo").AddScheme<Microsoft.AspNetCore.Authentication.AuthenticationSchemeOptions, DemoAuthenticationHandler>("Demo", _ => { });
builder.Services.AddAuthorization();
builder.AddSelectedStack();
builder.Services.AddSampleApplication();
builder.Services.AddExceptionHandler<Baseline.Sample.WebApi.Errors.ValidationExceptionHandler>();
builder.Services.AddControllers();
builder.Services.AddProblemDetails(options => options.CustomizeProblemDetails = context =>
{
    context.ProblemDetails.Extensions["traceId"] = context.HttpContext.TraceIdentifier;
    context.ProblemDetails.Extensions["errorCode"] = context.ProblemDetails.Status switch
    {
        400 => "validation_failed",
        401 => "unauthenticated",
        403 => "forbidden",
        404 => "not_found",
        _ => "unexpected_error"
    };
});
var app = builder.Build();
app.UseExceptionHandler();
app.UseStatusCodePages();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapSelectedStack();
app.Run();

/// <summary>Exposes the educational host to isolated integration tests.</summary>
public partial class Program;
