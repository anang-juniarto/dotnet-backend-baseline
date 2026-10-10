# Baseline.Sample — modular .NET 10 Clean Architecture/CQRS

This isolated example follows the user-designated Core/Infrastructure/Presentation schema. MediatR dispatches feature commands/queries through FluentValidation. The default HTTP host uses owner-isolated volatile memory, no external servers and disabled demo authentication. This is not a production-ready deployment or full chat/push product. `examples/**` is excluded from baseline reference transfer.

## Actual structure

```text
Baseline.Sample.slnx
global.json                         # SDK 10.0.201
Directory.Build.props
Directory.Packages.props            # verified package pins, centralized
src/
  Core/
    Baseline.Sample.Domain/
    Baseline.Sample.Application/    # Common/Interfaces, Behaviors, Features/Items
  Infrastructure/
    Baseline.Sample.Persistence.InMemory/
    Baseline.Sample.Persistence.EntityFramework/
    Baseline.Sample.Persistence.SqlServer/
    Baseline.Sample.Persistence.PostgreSql/
    Baseline.Sample.Persistence.MySql/ # deferral evidence only, no fake project
    Baseline.Sample.Persistence.MongoDb/
    Baseline.Sample.Infrastructure.Caching/
    Baseline.Sample.Infrastructure.Search/
    Baseline.Sample.Infrastructure.Messaging/
    Baseline.Sample.Infrastructure.BackgroundJobs/
    Baseline.Sample.Infrastructure.Observability/
  Presentation/
    Baseline.Sample.WebApi/
    Baseline.Sample.Grpc/
    Baseline.Sample.Realtime/
tests/
  Baseline.Sample.UnitTests/
  Baseline.Sample.IntegrationTests/
  Baseline.Sample.ArchitectureTests/
deploy/docker/                       # build/test/publish artifact export, not runtime image
.github/workflows/                   # nested reference workflow, inactive until promoted
```

The schema's `<Project>` names map to `Baseline.Sample`; Items remains the harmless example rather than adding speculative orders/payments. Optional modules are independently compilable projects. The aggregate solution includes them to check compilation, while the default WebApi references only Core and memory. Target generation should omit unselected module projects/packages entirely.

## Build and tests

All commands below use PowerShell with working directory `examples/Baseline.Sample` (not the baseline repository root). Use the SDK selected by [global.json](global.json); SDK policy and execution evidence belong to the [profile](docs/repository-profile.md). From this directory:

```powershell
dotnet build Baseline.Sample.slnx
dotnet test Baseline.Sample.slnx
```

For minimal host-only restore/build, use `src/Presentation/Baseline.Sample.WebApi/Baseline.Sample.WebApi.csproj` instead of the aggregate solution. NuGet access is required; external servers are not needed for compilation or the memory test profile. Exact package versions live in CPM, not inferred from installed runtimes. Final verification results are recorded in the [profile](docs/repository-profile.md).

## Five local profiles

Use a fresh PowerShell session with no inherited sample settings, and run one profile at a time. Build properties select references; environment variables select runtime behavior. Each `dotnet run` builds its selected host: do not add `--no-build` or rely on another profile's output. Commands are source-derived; executed checks and remaining verification are recorded only in the [profile](docs/repository-profile.md).

Common setup for all five profiles:

```powershell
$hostProject = "src/Presentation/Baseline.Sample.WebApi/Baseline.Sample.WebApi.csproj"
$env:ASPNETCORE_ENVIRONMENT = "Development"
$env:DemoAuth__Enabled = "true"
$env:Persistence__Provider = "memory"
$env:Realtime__Enabled = "false"
```

| Profile | Build selection | Runtime selection | Expected scope |
|---|---|---|---|
| memory | none | memory | Local Items HTTP exercise below; volatile state |
| sqlserver | `EnableSqlServer=true` | sqlserver + Primary secret | Compile/configuration only until isolated schema and live tests exist |
| postgresql | `EnablePostgreSql=true` | postgresql + Primary secret | Compile/configuration only until isolated schema and live tests exist |
| realtime | `EnableRealtime=true` | memory + realtime enabled | Authorized receive-only hub; no durable inbox/chat |
| observability-off | `EnableObservability=true` | memory + five signals off | No selected telemetry destination; not exporter certification |

### memory

```powershell
dotnet run --project $hostProject
```

Host is code-bound to `http://127.0.0.1:5080` and rejects configured Kestrel endpoints. Header `X-Demo-User` is NOT authentication against local users: anyone local can impersonate a demo identity. Never expose through tunnels, proxies, container port mappings or a shared server. Enabling demo auth outside Development fails startup. Authentication is disabled by default, yielding 401.

```powershell
$headers = @{ "X-Demo-User" = "alice" }
$item = Invoke-RestMethod -Method Post -Uri "http://127.0.0.1:5080/api/items" -Headers $headers -ContentType "application/json" -Body '{"name":"Example item"}'
Invoke-RestMethod -Uri "http://127.0.0.1:5080/api/items/$($item.id)" -Headers $headers
```

HTTP compatibility remains unchanged: POST creates `{id,name}` with 201/Location; GET returns 200 or 404 for missing/other-owner items. Request contracts live under `src/Presentation/Baseline.Sample.WebApi/Contracts/Items/` (e.g. `CreateItemRequest`), exposing only `Name`. Owner identity is resolved server-side from the authenticated principal, never accepted from request bodies; any client-supplied `ownerId` in JSON payloads cannot override server resolution. The business name invariant (1–100 characters) is enforced after trimming; inputs within 1–100 non-whitespace characters are accepted even when padded with leading/trailing whitespace. Invalid inputs return 400 Problem Details. Codes are sample-local lowercase `validation_failed`, `unauthenticated`, `forbidden`, `not_found`, `unexpected_error` with `traceId`. Create retries can create duplicates. Memory is volatile and unbounded demo storage, not a production datastore.

### sqlserver and postgresql — compile/configuration only

Compile each selected host without starting a database:

```powershell
dotnet build $hostProject -p:EnableSqlServer=true
dotnet build $hostProject -p:EnablePostgreSql=true
```

For a separately authorized isolated provider exercise, inject `ConnectionStrings__Primary` into this session from an approved secret source; do not paste credentials into commands/history or commit them. There is no insecure fallback connection string. Provision and review the provider-specific schema separately: startup does not create a database, apply migrations or retry saves. The following commands show matching build/runtime selection, **not a verified relational CRUD quick-start**; do not run them against an empty or production database:

```powershell
$env:Persistence__Provider = "sqlserver"
dotnet run --project $hostProject -p:EnableSqlServer=true
```

Or, in a fresh session with the PostgreSQL secret and common setup:

```powershell
$env:Persistence__Provider = "postgresql"
dotnet run --project $hostProject -p:EnablePostgreSql=true
```

Registration requires a nonempty Primary connection string; actual CRUD needs an available engine and matching schema. A listening host alone does not prove either. See [EF schema prerequisites](src/Infrastructure/Baseline.Sample.Persistence.EntityFramework/README.md).

### realtime

After common setup in a fresh session:

```powershell
$env:Realtime__Enabled = "true"
dotnet run --project $hostProject -p:EnableRealtime=true
```

Maps authenticated receive-only `/hubs/notifications`; anonymous negotiation must be challenged. This is live hints, not inbox/chat persistence, external push or proof of live WebSocket delivery.

### observability-off

After common setup in a fresh session:

```powershell
$env:Observability__ConsoleEnabled = "false"
$env:Observability__SeqEnabled = "false"
$env:Observability__ErrorReportingEnabled = "false"
$env:Observability__TracingEnabled = "false"
$env:Observability__MetricsEnabled = "false"
dotnet run --project $hostProject -p:EnableObservability=true
```

All five signals are explicitly off; no telemetry endpoint/DSN is needed. Ordinary host logging is not disabled by this profile. See [module instructions](src/Infrastructure/Baseline.Sample.Infrastructure.Observability/README.md) before selecting any signal.

### Cleanup and failure modes

Stop the process with Ctrl+C, then remove only the variables introduced in this fresh session (also after a failed startup):

```powershell
$sampleVariables = @("ASPNETCORE_ENVIRONMENT", "DemoAuth__Enabled", "Persistence__Provider", "ConnectionStrings__Primary", "Realtime__Enabled", "Observability__ConsoleEnabled", "Observability__SeqEnabled", "Observability__ErrorReportingEnabled", "Observability__TracingEnabled", "Observability__MetricsEnabled")
$sampleVariables | ForEach-Object { Remove-Item -LiteralPath "Env:$_" -ErrorAction SilentlyContinue }
Remove-Variable hostProject, sampleVariables -ErrorAction SilentlyContinue
```

Do not clear pre-existing settings in a shared shell; instead use a fresh session or restore its prior values. Configuration keys and common errors are in [configuration](docs/configuration.md).

## Other selected modules
- Separate [gRPC host](src/Presentation/Baseline.Sample.Grpc/README.md): real protobuf Create/Get via the same application pipeline, isolated process-memory state, Development-only loopback HTTP/2 at5081. It does not share Items with a separately running WebApi process.
- MongoDB, Redis, Elasticsearch, RabbitMQ and Hangfire supply callable adapter registration APIs but are not auto-wired into the memory host. Follow module READMEs and explicit trusted-client/worker/storage ownership. Enabled MassTransit starts its bus through host lifecycle; Hangfire scheduler registration does not start a worker/dashboard. No durable business outbox is implemented.

## Coverage and limits

Implemented source modules: EF shared/SQL Server/PostgreSQL item stores; MongoDB item store; bounded Redis byte cache; owner-filtered Elasticsearch projection adapter; MassTransit RabbitMQ publisher; Hangfire SQL Server scheduler; independent Seq/Sentry/OTel registration; protobuf gRPC host; authenticated SignalR notification transport. Compilation is not real-engine or provider-delivery certification.

Deferred: canonical Pomelo EF10 MySQL (no matching published provider verified); durable chat/conversation history, notification inbox, web/mobile push, production identity, outbox/consumers/workers, generated migrations and provider-engine tests, OpenAPI/UI, full business Orders model. Alternative Oracle MySQL provider is not silently substituted because behavior/licensing differs. MediatR12.4.1 and MassTransit8.3.6 are explicit verified pins; future major-version licensing must be reviewed. Hangfire LGPL/commercial choice remains deployment-owner responsibility.

Schema CI/deployment branches are selectable references, not fabricated cloud configuration. See [artifact Docker/reference CI notes](deploy/docker/README.md). Kubernetes/Helm/Terraform, registry push, production release/security services, runtime containers and all-service Compose are absent until a concrete platform and safe production host exist. Raw source schema remains unchanged; maintained [repository map](../../docs/architecture/repository-map.md) uses .NET10/RFC9457.
