# Application Configuration & Optional Capability Options

> **Classification:** `[PROFILE]`
> **Status:** Draft | **Owner:** DevOps / Backend Leads
> **Last verified:** Not verified | **Evidence:** Documentation design target; no adapter runtime certification

## Selection and scope

For approved new targets, use .NET 10 Clean Architecture/CQRS with API/optional Worker composition roots. These are proposed conventions, not installed options or vendor SDK property names. Preserve existing brownfield keys unless an explicit migration is approved. Select capabilities at generation time: an unselected module contributes no packages, settings, containers, hosted services, health checks or network calls. The [sample README](../../examples/Baseline.Sample/README.md) records actual selected modules, options and verification. The default Items profile remains volatile in-memory; optional adapter compilation does not certify external servers, durable chat/inbox, or provider delivery.

Application in `src/Core` owns use-case ports and MediatR feature slices; separate assemblies in `src/Infrastructure` implement selected datastore/cache/messaging/job/push adapters. Hosts and optional realtime transport live in `src/Presentation`. Domain/Application contain no vendor options or SDK types. API owns transport wiring and the SignalR hub adapter; Worker owns its composition root. Register request/DbContext dependencies scoped; create a fresh scope per message/job. Long-lived clients may be shared only when their verified SDK supports it; singleton services never capture scoped dependencies.

## Proposed key inventory

No values below are credentials or defaults for an actual application. Each selected target must record types, bounds, defaults, owner and reload/restart behavior in its verified profile. `ConnectionStrings:Primary` is the single primary relational credential convention; do not also require `Database:ConnectionString` or `DefaultConnection` for the same store.

| Selected capability | Proposed configuration sections/keys | Secret sources |
|---|---|---|
| Relational store | `Persistence:Provider` (`none`, `sqlserver`, `mysql`, `postgresql`); `Persistence:CommandTimeoutSeconds`, `Persistence:MaxPoolSize` | `ConnectionStrings:Primary` |
| MongoDB | `MongoDb:Database`, bounded operation timeouts | `ConnectionStrings:MongoDb` |
| Elasticsearch search | `Elasticsearch:Endpoint`, `IndexPrefix`, request timeout | `Elasticsearch:ApiKey` or selected credential |
| Redis cache | `Redis:KeyPrefix`, `DefaultTtlSeconds`, operation timeout | `ConnectionStrings:Redis` |
| RabbitMQ | `RabbitMq:VirtualHost`, routing names, `PrefetchCount`, retry/DLQ limits | `ConnectionStrings:RabbitMq` |
| Hangfire | `Hangfire:StorageProvider`, queues, worker concurrency, dashboard policy | `ConnectionStrings:Hangfire` or selected storage credential |
| gRPC | `Grpc:MaxReceiveMessageBytes`, deadline policy; actual host listener configuration | TLS identity via selected host secret mechanism |
| SignalR | `Realtime:ScaleOut` (`single-node`, `redis-backplane`, `managed-service`), allowed origins, message/connection bounds | `ConnectionStrings:SignalRBackplane` or managed-service identity |
| Notification inbox | `Notifications:RetentionDays`, dispatch batch/retry limits | Selected authoritative store credential |
| Web/mobile push | `Push:WebProvider`, `Push:MobileProvider`, TTL and outbound destination policy | Selected FCM service identity, APNs private key, or VAPID private key via secret store |
| OTel | Target-owned service/environment identity, sampler and OTLP endpoint configuration | Exporter authentication via approved secret injection |
| Seq | `Seq:Endpoint`, selected bounded delivery route | `Seq:ApiKey` |
| Sentry | Environment/release, capture and sampling policy using verified SDK configuration | `Sentry:Dsn` via approved configuration source |

Cache and SignalR backplane credentials/key namespaces are separate roles even when sharing a reviewed deployment; see [Redis](./redis.md). Hangfire storage is independently selected; Redis cache does not imply Redis job storage or its license/support. Inbox, push and live hints are independently selected. FCM is a proposed option, not a mandatory dependency. See [RabbitMQ recovery](./rabbitmq.md), [Hangfire storage](./hangfire.md) and [telemetry signal ownership](./observability.md), including the .NET 10 handled-exception diagnostics policy.

## Validation and isolation

Bind typed options only for included modules and validate required values and cross-field dependencies at startup using the selected framework's verified mechanism. Validate positive bounded timeouts, batch/concurrency limits, supported providers, TLS endpoints, environment isolation, prefixes and required credentials. No credential text in validation errors. Do not silently enable capabilities by finding a connection string. Disabled included modules must perform no outbound work and must not require unused secrets.

ASP.NET Core-style environment overrides use double underscores: `ConnectionStrings__Primary` corresponds to `ConnectionStrings:Primary`. Use approved local secret storage, CI vault injection or workload identity/secret-store integration, not tracked values. Explicitly choose local/test destinations; never default to production. Non-secret endpoint names can still expose topology and need review. See [secret lifecycle](../security/secrets-management.md) and [delivery standards](../standards/delivery-and-supply-chain.md).

## Network reference (not deployment configuration)

Common listener conventions are SQL Server 1433, MySQL 3306, PostgreSQL 5432, MongoDB 27017, Redis 6379, RabbitMQ AMQP 5672/TLS 5671 and Elasticsearch HTTP 9200. Actual TLS/listener mappings must be verified; none of these imply public exposure. RabbitMQ management commonly uses 15672 and is restricted separately. gRPC and SignalR can share the API's approved HTTPS listener; no mandatory extra public port. Hangfire dashboard shares its selected restricted host. Seq, Sentry, OTLP collectors and push providers use explicitly configured endpoints; do not assume a collector or vendor port. Permit only selected egress and isolate administration interfaces.

## Acceptance gates

Check missing/invalid selected settings fail safely, disabled modules send nothing, secrets are redacted, API/Worker namespaces are isolated, and dependency outages obey [readiness policy](./observability.md). Exact SDK/provider compatibility, licensing and actual bound keys require target restore/build/integration evidence before certification; this reference supplies none.
