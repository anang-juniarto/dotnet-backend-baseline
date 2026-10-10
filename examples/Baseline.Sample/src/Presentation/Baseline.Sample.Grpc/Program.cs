using System.Net;
using Baseline.Sample.Application;
using Baseline.Sample.Application.Items;
using Baseline.Sample.Grpc.Security;
using Baseline.Sample.Grpc.Services;
using Baseline.Sample.Persistence.InMemory.Items;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Server.Kestrel.Core;

var builder = WebApplication.CreateBuilder(args);
if (!builder.Environment.IsDevelopment())
{
    throw new InvalidOperationException("This educational HTTP/2 gRPC host is Development-only; production requires a separately configured TLS and authentication host.");
}

if (builder.Configuration.GetSection("Kestrel:Endpoints").GetChildren().Any())
{
    throw new InvalidOperationException("Configured Kestrel endpoints are not supported by this loopback-only demo.");
}

builder.WebHost.ConfigureKestrel(options =>
    options.Listen(IPAddress.Loopback, 5081, endpoint => endpoint.Protocols = HttpProtocols.Http2));
builder.WebHost.UseUrls("http://127.0.0.1:5081");
var demoEnabled = builder.Configuration.GetValue<bool>("DemoAuth:Enabled");
builder.Services.AddSingleton(new DemoAuthenticationSettings(demoEnabled));
builder.Services.AddAuthentication("Demo")
    .AddScheme<AuthenticationSchemeOptions, DemoAuthenticationHandler>("Demo", _ => { });
builder.Services.AddAuthorization();
builder.Services.AddSampleApplication();
builder.Services.AddSingleton<IItemStore, InMemoryItemStore>();
builder.Services.AddGrpc(options => options.EnableDetailedErrors = false);

var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();
app.MapGrpcService<ItemsService>().RequireAuthorization();
app.Run();

/// <summary>Exposes the separate educational gRPC host for isolated integration checks.</summary>
public partial class Program;
