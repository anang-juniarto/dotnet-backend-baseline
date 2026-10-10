# Local Development Setup

> **Classification:** `[PROFILE]`
> `Status: Draft` | `Owner: Engineering Leads` | `Last verified: Not verified` | `Evidence: Target-specific reference workflow; executable commands owned by target/sample README`

## 1. Reference, Example, or Consuming Target

This repository's reference documents are not a root backend application. Do not run placeholder solution commands from the repository root. For the isolated volatile-memory example, use [Baseline.Sample/README.md](../../examples/Baseline.Sample/README.md), including its exact startup and verification instructions. Its gated Development-only demo auth is not production authentication; compiled optional vendor adapters are distinct from live-provider certification, and durable chat history/persistent notification inbox remain unimplemented.

Choose one sample verification profile before execution: default memory host, selected host modules, aggregate compilation, separate gRPC host, or artifact/CI validation. The sample README is the command authority for all five and routes to module/artifact details; this guide intentionally duplicates no sample commands. Aggregate compilation is not all-stack startup or live-provider certification. `examples/**` remains excluded from reference adoption.

For a separately approved consuming target, new projects default to .NET 10 `src/Core/` Domain/Application with MediatR feature slices, modular provider/capability projects in `src/Infrastructure/`, Presentation WebApi (optional Grpc) in `src/Presentation/`, and `tests/<Project>.UnitTests/`, `tests/<Project>.IntegrationTests/`, `tests/<Project>.ArchitectureTests/`. Selected hosts own composition-root wiring. Brownfield targets retain their verified runtime, solution format, startup hosts and tooling unless migration is approved.

Before execution, inspect `global.json`, actual solution/project files, scripts/hooks, package manifests and the target's configuration binding. Record exact paths and selected capabilities. Verify SDK availability using the [prerequisites](./prerequisites.md); neither this guide nor the sample authorizes installs, restores, migrations or external calls.

## 2. Build and Run the Selected Host

Use the target README's verified restore/build/run commands and actual `.sln` or `.slnx` paths. Presentation WebApi is the default host/composition root for a new target; optional Grpc and worker hosts are included only when selected and register only their selected provider/capability assemblies. No universal solution name, startup project, listening port or health route is prescribed here. Discover addresses from actual launch settings and startup output, and keep local targets loopback-bound where appropriate.

Select only required local infrastructure from the [optional catalog](../architecture/optional-stack-catalog.md). Inspect the actual compose/services file before an authorized startup; do not assume `docker-compose.yml` exists or start an all-stack profile. Specify isolated databases/volumes, verified image versions, synthetic credentials and bounded resource needs. No datastore is needed for a stateless target or the memory-only sample.

A required authoritative store can affect readiness; optional telemetry exporters/cache outages must not automatically imply process death. Do not add probes, settings or hosted services for omitted modules. Chat/inbox persistence, SignalR scale-out and push providers need separate selected configurations; none automatically enables Redis, RabbitMQ, Hangfire or Firebase.

## 3. Local Configuration and Secrets

Follow the actual host's configured providers and precedence, not a universal assumption about custom hosts. Under the usual ASP.NET Core host, committed `appsettings.json` and Development defaults contain non-secret values; environment variables or Development user secrets supply local secrets. Verify the selected host loads those providers. Do not initialize user secrets unless that project needs them and the change is approved.

The following is **illustrative consuming-target configuration**, not sample configuration or an implemented baseline binding:

| Selected target convention | Configuration key | Environment-variable equivalent |
|---|---|---|
| Target A binds a relational connection with `GetConnectionString("Primary")` | `ConnectionStrings:Primary` | `ConnectionStrings__Primary` |
| Target B binds a dedicated database options section instead | `Database:ConnectionString` | `Database__ConnectionString` |

Choose the row matching verified target code, not both. For an approved new target selecting Target A's convention, use `ConnectionStrings:Primary` consistently in binding, local secret setup, deployment and operations docs. A brownfield target with `Database:ConnectionString` or another existing key keeps that convention; do not silently rename it. Neither example applies to the sample's volatile store. Placeholder secret values are not real connection strings and must not be committed or logged.

Use only the selected module's documented keys and secret sources. Validate required selected settings at startup; omitted modules require none. Never use production credentials, live databases or chargeable telemetry/push targets by default.

## 4. Verify Before Handoff

Discover the framework/runner and exact build, lint/analyzer/typecheck and test commands from target manifests/scripts before execution; follow the [testing guide](../engineering/testing-guide.md). Sample commands and test results are sample-only evidence, not consuming-target certification. Do not assume `dotnet test` filters work across runners or that every target has a test project.

After authorized startup, use the [first request guide](./first-api-request.md) with discovered routes/contracts. Report executed checks with exact outcomes, unexecuted checks and reasons, configuration limitations and deferred integrations. No command in this reference is evidence that a local build or test has run.
