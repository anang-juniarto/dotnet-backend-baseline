# Example configuration and generation-time selection

> Status: Draft
> Classification: `[PROFILE]`
> Scope: isolated sample only; actual keys below, not generic vendor SDK options

The [sample README](../README.md#five-local-profiles) owns exact PowerShell run commands and cleanup for memory, sqlserver, postgresql, realtime and observability-off. Run them from `examples/Baseline.Sample`, not the baseline repository root. The [profile](repository-profile.md) owns evidence and limitations; these configuration instructions do not assert new execution results.

## Default host

| Key | Default | Effect |
|---|---|---|
| `DemoAuth:Enabled` / `DemoAuth__Enabled` | false | Simulated local identity only in Development; enabled elsewhere fails startup |
| `ASPNETCORE_ENVIRONMENT` | Framework Production default | Explicit Development required for demo identity; gRPC host requires Development even without identity |
| `Persistence:Provider` | memory | sqlserver/postgresql require corresponding build-selected module; other values fail startup |
| `ConnectionStrings:Primary` | absent | Required only for selected SQLServer/PostgreSQL; source no real credentials |
| `Realtime:Enabled` | false | Requires EnableRealtime build selection; maps authenticated receive-only hub |
| `Observability` | absent | Requires EnableObservability build selection; independent module options default off |

No settings/secrets are required for default memory operation. Kestrel listeners are code-owned loopback5080 HTTP/5081 local HTTP2, and configured `Kestrel:Endpoints` is rejected. Do not expose proxies/tunnels/container ports. Demo header identities are not secure against local users. `X-Demo-User` is a single1–64 ASCII letter/digit/underscore/hyphen identifier; owner comparison case-sensitive. The HTTP request contract (`CreateItemRequest`) requires presence, while business name length (1–100 characters) is evaluated after trimming by Application and Domain layers. Server request-body size limits protect the host from oversized transport payloads, distinct from the normalized business name invariant.

## Build properties

WebApi `EnableSqlServer`, `EnablePostgreSql`, `EnableObservability`, `EnableRealtime` default false. They conditionally add module references and compiler symbols; runtime settings cannot activate a module omitted at build time. Selecting both relational compile flags is permitted for compatibility compilation, but runtime picks one store. No automatic schema initialization/migrations/retries are configured. A real provider requires pre-provisioned isolated schema and separately approved tooling.

Carry the same `-p:Enable…=true` selection on `dotnet run --project <exact csproj>`; setting an environment variable alone does not add a project reference. The README deliberately rebuilds each run and does not prescribe a build→`--no-build` shortcut with unverified artifact provenance. Aggregate solution compilation does not enable modules in the default WebApi.

To configure a selected relational provider, set `Persistence__Provider` and inject `ConnectionStrings__Primary` from an approved secret source into a fresh session. Never put actual credentials in source, example commands, logs or shell history; do not use production targets or disable certificate validation to make a demo work. Registration checks a nonempty string, not engine connectivity or schema readiness. SQL Server/PostgreSQL profiles remain compile/configuration exercises until separately approved schema provisioning and live tests; there is no usable empty-database CRUD promise.

Use the README cleanup after Ctrl+C or startup failure; it removes the introduced process environment variables, including the connection secret. It does not delete persisted secrets: remove any secret-source entry through that source's approved procedure. In a shared session restore previous values rather than deleting settings owned by another task.

## Expected failure modes

| Symptom | Check |
|---|---|
| Selected provider is not compiled | Use the matching build property on the current run; provider value must be memory/sqlserver/postgresql |
| Primary connection string required | Inject the selected provider's secret; no fallback credentials exist |
| Database CRUD fails | Verify isolated engine/schema independently; startup alone proves neither; do not blindly retry creates |
| HTTP 401 | Demo identity is disabled by default; explicitly opt in in Development and supply a valid header |
| Demo opt-in outside Development fails | Keep the guard; never weaken it or expose the loopback demo |
| Configured Kestrel endpoints rejected | Remove conflicting endpoint settings; never override listener protection |
| Realtime/observability configuration rejected | Select its build property; even all-off observability keys require a compiled module |

`Observability__ConsoleEnabled`, `Observability__SeqEnabled`, `Observability__ErrorReportingEnabled`, `Observability__TracingEnabled`, `Observability__MetricsEnabled` are all explicitly false in the observability-off profile. Leave the entire `Observability` section absent in the unselected memory/realtime/relational profiles.

## Other module registration boundaries

- MongoDB/Redis/Elasticsearch adapters require explicitly supplied trusted client instances and module options. Default host does not create these clients or call their registration.
- MassTransit messaging options default disabled; enabling registers the bus and starts it with the owning host. No consumer/receive endpoint or transactional outbox exists in this sample.
- Hangfire registration defaults disabled and registers a SQL-backed scheduler only; no worker/dashboard or automatic schema preparation. Enqueue requires durable store availability and may have unknown outcomes.
- Observability options `ConsoleEnabled`, `SeqEnabled`, `ErrorReportingEnabled`, `TracingEnabled`, `MetricsEnabled` are independent. Full endpoints/DSN are required only for selected destinations. See [module README](../src/Infrastructure/Baseline.Sample.Infrastructure.Observability/README.md). No signals enabled in default host.
- Realtime transport has no user-controlled join/broadcast endpoint. Server notifier inputs must come from trusted authorization/state; the transport itself does not implement recipient lookup or durable inbox.

Each separate process owns memory; HTTP/gRPC hosts do not share data. Memory has no capacity policy or cleanup and is for synthetic demo data. Required production identity, durable history/inbox/push storage, preferences, outbox and vendor credentials are absent. Logs must not contain tokens, owner IDs, chat bodies or destination identifiers.
