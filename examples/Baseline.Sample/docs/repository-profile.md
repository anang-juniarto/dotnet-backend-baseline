# Example repository profile

> Status: Draft
> Classification: `[PROFILE]`
> Scope: `examples/Baseline.Sample` only
> Evidence date: 2026-10-10; historical implementation checks below, optimization checks recorded separately when executed

## Verified structure and default profile

- [global.json](../global.json) sets SDK `10.0.201`, `rollForward=latestPatch`, `allowPrerelease=false`: compatible patches within the same feature band, not an automatic feature-band/major upgrade. Root selected this policy; only installed SDK 10.0.201 has execution evidence, not alternate-patch/cross-machine resolution. Docker SDK stays exact 10.0.201. All projects target net10.0; nullable and XML docs enabled.
- Canonical Core/Infrastructure/Presentation folders with modular provider/capability projects; 3 categorized test assemblies.
- MediatR12.4.1 and FluentValidation12.1.0, feature command/query handlers, validation/logging/tracing pipeline. No event sourcing or separate read database.
- Default WebApi: memory store, owner-isolated Items POST/GET, opt-in Development-only header identity, code-owned loopback5080, reject configured Kestrel endpoints.
- Optional separate gRPC host: actual generated protobuf, same MediatR use cases, isolated memory per process, Development-only loopback5081 HTTP/2, authentication off unless explicitly enabled.
- Optional receive-only SignalR notification transport: authenticated hub at `/hubs/notifications`; no durable history, inbox or external push.
- No OpenAPI generation/UI package or production identity provider selected.

## Compiled adapter evidence (not live integration certification)

| Module | Exact selected dependency evidence | Scope |
|---|---|---|
| Shared EF / SQL Server | EFCore.Relational/SqlServer10.0.9 | Item mapping and scoped store, separate provider context/migration assembly; no migrations generated/applied |
| PostgreSQL | Npgsql EF10.0.3, EF10.0.9 compatible range | Item store; no engine connection |
| MongoDB | MongoDB.Driver3.12.0 | Owner-scoped native store; host supplies client |
| Redis | StackExchange.Redis3.4.0 | Bounded byte cache, host supplies multiplexer |
| Elasticsearch | Elastic.Clients.Elasticsearch9.5.3 | Owner-filtered projection and alias-required index writes; host supplies client/mapping |
| RabbitMQ | MassTransit.RabbitMQ8.3.6 | Publisher/V1 contract only; enabled registration starts bus with host; no consumer/outbox |
| Hangfire |1.8.21; SqlClient6.1.1; Newtonsoft.Json13.0.3 | SQL scheduler returns actual queued ID when invoked; no worker/dashboard/schema creation |
| Seq/Sentry/OTel | See CPM and observability module README | Independent disabled defaults; no live exports |

Package availability and compilation do not prove server compatibility, delivery, end-to-end privacy or production licensing entitlement. MediatR12.4.1/MassTransit8.3.6 pins deliberately avoid assuming current major-version terms; verify licenses for target redistribution/upgrade. Hangfire LGPL/commercial obligations need deployment-owner review. Central package versions are authoritative.

## Selection and configuration

Default host excludes vendor projects/packages. `EnableSqlServer`, `EnablePostgreSql`, `EnableObservability`, `EnableRealtime` build properties selectively compile corresponding references. Runtime provider selection defaults memory and rejects unsupported values. Aggregate solution includes all implemented modules for compilation coverage, not runtime enablement. See [configuration](configuration.md).

## Evidence tiers

| Tier | Meaning here |
|---|---|
| Implemented | Source/registration exists; does not prove successful execution |
| Compile-verified | A recorded build passed for its exact selected dependencies |
| Runtime-tested | Recorded in-process tests exercise only their named behavior; not real sockets/providers |
| Live-verified | Requires isolated real-engine/transport/exporter evidence; none claimed here |
| Deferred | Missing implementation or unexecuted verification, listed below |

Memory Items have historical in-process HTTP coverage. Relational adapters have compilation/model evidence only, not schema/live CRUD verification. Realtime source is implemented; live WebSocket delivery remains unverified. Observability registration is implemented/compiled, not live export certification. Consult exact checks before promoting any tier; `Draft` lifecycle does not erase historical evidence or imply production readiness.

## Historical executed checks

The following implementation-stage results were previously recorded by the coordinating root and module owners, not rerun by this documentation edit. Actor and working directory are distinct: solution commands require `examples/Baseline.Sample`; the earlier record did not preserve a separate actual process-working-directory transcript for each command. No new optimization result is implied.

- Coordinating root: `dotnet build Baseline.Sample.slnx --nologo -warnaserror`: passed18 projects, zero warnings/errors.
- Coordinating root: `dotnet test Baseline.Sample.slnx --no-build --no-restore --verbosity minimal`: passed34 (Unit10, Integration18, Architecture6), zero failed/skipped.
- Coordinating root: WebApi build with all four selection flags true: passed zero warnings/errors, without external calls.
- Module owners: standalone provider/cache/search/messaging/jobs/telemetry/gRPC/realtime builds and scoped formatting checks passed.
- Coordinating root: solution whitespace format verification passed. NuGet vulnerability audit including transitives reported no known vulnerable packages across18projects using current sources.
- Coordinating root: code-only graph refresh completed473nodes/781edges; global.json produced no nodes (tool warning), so graph is not complete source evidence.

## Optimization verification record

Date: 2026-10-10. Actor: coordinating root. SDK: 10.0.201 (installed SDK list also included older SDKs; no alternate .NET 10 patch was executed). Working directory for all commands below: `examples/Baseline.Sample`.

| Check | Exact scope/command | Result |
|---|---|---|
| Default Release build | `dotnet build Baseline.Sample.slnx --configuration Release --artifacts-path artifacts/validation-consistency/default -p:EnableSqlServer=false -p:EnablePostgreSql=false -p:EnableObservability=false -p:EnableRealtime=false -warnaserror` | Build: 0 warnings, 0 errors |
| Default Release tests | `dotnet test Baseline.Sample.slnx --configuration Release --artifacts-path artifacts/validation-consistency/default --no-build --no-restore --verbosity minimal` | Tests: 46 passed, 0 failed/skipped (Unit 10, Integration 29, Architecture 7) |
| Format verification | `dotnet format Baseline.Sample.slnx --verify-no-changes --no-restore` | Passed |
| Optional host compile (SQL Server) | `dotnet build src/Presentation/Baseline.Sample.WebApi/Baseline.Sample.WebApi.csproj --configuration Release --artifacts-path artifacts/optimization/sqlserver -p:EnableSqlServer=true -p:EnablePostgreSql=false -p:EnableObservability=false -p:EnableRealtime=false -warnaserror` | Build: 0 warnings, 0 errors |
| Optional host compile (PostgreSQL) | `dotnet build src/Presentation/Baseline.Sample.WebApi/Baseline.Sample.WebApi.csproj --configuration Release --artifacts-path artifacts/optimization/postgresql -p:EnableSqlServer=false -p:EnablePostgreSql=true -p:EnableObservability=false -p:EnableRealtime=false -warnaserror` | Build: 0 warnings, 0 errors |
| Optional host compile (Observability) | `dotnet build src/Presentation/Baseline.Sample.WebApi/Baseline.Sample.WebApi.csproj --configuration Release --artifacts-path artifacts/optimization/observability -p:EnableSqlServer=false -p:EnablePostgreSql=false -p:EnableObservability=true -p:EnableRealtime=false -warnaserror` | Build: 0 warnings, 0 errors |
| Optional host compile (Realtime) | `dotnet build src/Presentation/Baseline.Sample.WebApi/Baseline.Sample.WebApi.csproj --configuration Release --artifacts-path artifacts/optimization/realtime -p:EnableSqlServer=false -p:EnablePostgreSql=false -p:EnableObservability=false -p:EnableRealtime=true -warnaserror` | Build: 0 warnings, 0 errors |
| Optional host compile (All four) | `dotnet build src/Presentation/Baseline.Sample.WebApi/Baseline.Sample.WebApi.csproj --configuration Release --artifacts-path artifacts/optimization/all -p:EnableSqlServer=true -p:EnablePostgreSql=true -p:EnableObservability=true -p:EnableRealtime=true -warnaserror` | Build: 0 warnings, 0 errors |
| Realtime loopback smoke | `dotnet run --project src/Presentation/Baseline.Sample.WebApi/Baseline.Sample.WebApi.csproj -p:EnableRealtime=true -e ASPNETCORE_ENVIRONMENT=Development -e DemoAuth__Enabled=true -e Realtime__Enabled=true` | Host started on loopback; unauthenticated negotiate returned 401; owner-scoped HTTP create/get returned 200/201 |
| Observability-off loopback smoke | `dotnet run --project src/Presentation/Baseline.Sample.WebApi/Baseline.Sample.WebApi.csproj -p:EnableObservability=true -e ASPNETCORE_ENVIRONMENT=Development -e DemoAuth__Enabled=true -e Observability__ConsoleEnabled=false -e Observability__SeqEnabled=false -e Observability__ErrorReportingEnabled=false -e Observability__TracingEnabled=false -e Observability__MetricsEnabled=false` | Host started on loopback and stopped cleanly; no telemetry destination configured |

`dotnet run` accepted MSBuild property arguments in the observed SDK 10.0.201 invocation even though `dotnet run --help` does not list a dedicated property option. Re-check on SDK changes. SQL Server and PostgreSQL `dotnet run` commands were not executed because no isolated database/schema was provisioned. This does not prove schema creation, CRUD, migrations or provider behavior.

Not executed in this optimization: GitHub Actions (including the new root workflow), YAML schema validation, independent SHA provenance beyond the earlier read-only remote lookup recorded by the CI implementer, Docker build, alternate SDK patch resolution, real WebSocket delivery, live observability exporters and live providers. A reviewer's static approval does not replace those checks. Keep this section as the authority for test totals; update it rather than repeating totals in onboarding pages.

## Deferred/unexecuted

Canonical Pomelo MySQL EF10 unavailable in inspected index; Oracle provider is not substituted. Durable chat/inbox/external web/mobile push, production identity, outbox/workers/consumers, migrations, provider-engine tests, real gRPC/WebSocket/socket/proxy tests, live telemetry/push, load/deployment and Docker/GitHub execution remain unverified or unimplemented. In-memory tests do not certify those capabilities. No external service started or database migration applied.

Reference adoption excludes `examples/**`; this profile does not activate any capability in a consuming existing project.
