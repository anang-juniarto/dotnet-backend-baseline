# Optional Stack and Capability Catalog

> **Classification:** `[PROFILE]`
> **Status:** Draft
> **Owner:** Architecture / Engineering
> **Last verified:** Not verified
> **Evidence:** Approved design targets; exact package/server compatibility and integrations not certified

## 1. Selection policy

All optional capabilities in this catalog default off for generated applications; MediatR belongs to the explicit greenfield Application profile, not this optional stack. Selected providers/capabilities use separate Infrastructure assemblies per the [canonical schema](./repository-map.md). Select only approved modules and omit their packages, containers, required options, schemas, health probes, hosted processes and network calls when unselected. These are candidate dependency families, not approved package versions or implemented adapters. The isolated local in-memory sample is not external-stack certification. Reference scope and adoption mode remain independent of capability selection; see the [greenfield profile](./net10-baseline-profile.md).

| Capability | Purpose / candidate family | Selection constraints | Required verification |
|---|---|---|---|
| Sentry | Error reporting; Sentry .NET SDK | One error capture owner; optional reviewed SDK tracing/OTel interoperability; redact PII | Synthetic error once, disabled traffic absent, redaction and outage |
| Seq | Structured logs; Serilog Seq sink or verified OTLP route | One log delivery path; bounded buffering | Correlated event once and collector outage |
| OpenTelemetry | Vendor-neutral traces/metrics, optional logs | Selected instrumentation/exporter and collector; bounded attributes/sampling | Parentage, propagation, cardinality, exporter outage |
| Elasticsearch | Search/derived projections; Elastic client family | Not authoritative business store; compatible server/client, mappings/aliases/rebuild | Index/query/rebuild, staleness and outage |
| MongoDB | Primary documents or explicit read model; MongoDB.Driver | Collection/index ownership; topology supports selected transactions | CRUD, uniqueness/concurrency; real replica set for transactions |
| SQL Server | Relational authoritative store; EF SQL Server provider | Compatible EF Core 10 set, provider-specific migrations | Real-engine migrations, constraints, concurrency, split outcomes |
| MySQL | Relational authoritative store; verified EF provider or approved native path | EF Core 10 support is a go/no-go gate; no silent downgrade; MariaDB not equivalent | Exact server/provider, charset/collation, transaction/concurrency |
| PostgreSQL | Relational authoritative store; Npgsql EF provider | Provider/EF compatibility and separate migration set | Real engine, migrations, types/time, concurrency |
| Redis cache | Cache-aside; distributed-cache/StackExchange.Redis families | TTL/key ownership, after-commit invalidation, outage bypass; not authoritative storage | Hit/miss/expiry, invalidation, bounded timeout, tenant isolation if applicable |
| RabbitMQ | Integration delivery; MassTransit with RabbitMQ for selected greenfield messaging; preserve verified brownfield stack | Confirms, manual ack, bounded prefetch, retry/DLQ; outbox/inbox where durable recovery required | Duplicate/poison delivery, restart, commit-to-publish recovery |
| gRPC | Typed transport; ASP.NET Core gRPC/Protobuf tooling | HTTP/2/TLS/proxy, auth, deadlines, cancellation, compatible proto evolution | Round trip, denial, deadline, limits and compatibility |
| Hangfire | Persistent scheduling/jobs; selected Hangfire storage adapter | Storage separately selected/licensed; restricted dashboard; repeat-safe jobs | Restart, retries/duplicates, storage outage, dashboard denial |
| SignalR | Authenticated live events; ASP.NET Core SignalR | Single node needs no Redis; multi-node selects reviewed backplane or managed service | Authorization, reconnect/rejoin, revocation, proxy/scale-out behavior |
| Chat | Durable conversations/messages | Identity + authoritative datastore + SignalR; history via authorized paginated queries | Membership denial/revocation, deduplication, stable ordering/catch-up |
| Notification inbox | Recipient history, unread state/preferences | Identity + authoritative datastore; SignalR and push independently optional | Recipient isolation, repeated acknowledgments, cross-device cursors |
| Web push | Browser background notifications | Explicit FCM web, Web Push/VAPID or reviewed provider; HTTPS/service worker/permission | Registration ownership, lifecycle, payload privacy, SSRF for direct Web Push |
| Mobile push | Android/iOS OS notifications | Explicit FCM/APNs route, direct native or reviewed provider; separately scoped client SDKs | Rotation/logout/account switch, retry/permanent failures, authorized real-device tests |

## 2. Dependencies and recovery

- Initial relational choice is `none | sqlserver | mysql | postgresql`; primary storage may instead be MongoDB. Multiple stores require explicit bounded-context/collection ownership and consistency/reconciliation plans, not automatic inclusion.
- Each relational provider has a verified migration set; provider-named folders alone do not ensure isolation. EF InMemory/SQLite does not certify another engine.
- Elasticsearch projections and database commits are not atomic. Required recovery uses durable committed intent; document refresh/staleness and rebuild ownership.
- Business-required RabbitMQ publication uses transactional outbox where supported and idempotent consumers. Confirms are not business exactly-once delivery. No cross-store distributed transaction is promised.
- Hangfire storage is independent of the business store; Redis cache does not select Redis job storage. Verify adapter maintenance, compatibility and license separately.
- Redis is not mandatory for MongoDB, messaging, jobs, search or single-node SignalR. Redis cache and backplane are distinct roles/options. Backplanes are not durable history or arbitrary-worker publishing APIs; multi-node hosting must test affinity and documented exceptions.
- Seq does not select Elastic logging; Elasticsearch means search unless another use case is approved. OTel does not select Sentry/Seq. Choose one route per log/trace/error signal, including handled exception diagnostics, without duplicate capture.
- Readiness reflects critical endpoint/worker dependencies only. Optional cache or noncritical exporters must not automatically remove API traffic; dependency outages are not process liveness failures.
- Worker/job payloads carry small stable IDs/versioned contracts. Resolve scoped dependencies per operation and validate trusted identity/tenant context.

## 3. Chat, inbox and push boundaries

Chat defaults to text, direct/authorized group conversations, bounded cursor history and reconnect catch-up; attachments, moderation, editing, voice/video and end-to-end encryption are separate scope. Derive sender identity server-side and verify current membership on every command/query/hub invocation. Groups are not authorization; revoke live access after membership changes. Treat text as untrusted content and bound invocation frequency, connection/group sizes and message length.

Persist messages before durable acceptance; deduplicate conversation/sender/client-message IDs and define stable authoritative ordering rather than timestamps alone. Durable acceptance, live send, client receipt and read state differ. Required recoverable live publication needs atomic publication intent and a dispatcher; otherwise document best-effort hints and history catch-up. Clients deduplicate and reauthorize/rejoin after reconnect. Presence/typing are ephemeral; read cursors are monotonic with reviewed privacy semantics.

Inbox creation resolves recipients/preferences from trusted policy, deduplicates business event/recipient keys, and persists independently of channel attempts. Only explicit client actions mark read. Push can be selected without inbox or SignalR; do not manufacture an inaccessible inbox. Recoverable push-only delivery still needs intent/attempt storage. A store-backed dispatcher can suffice: RabbitMQ/Hangfire are optional scheduling mechanisms.

Proposed cross-platform provider route is FCM web/Android and configured FCM-to-APNs Apple delivery, subject to exact SDK/platform verification, not a selected/certified adapter. Firebase authentication or history storage is not required. Choose one route per platform; provider failover needs separate duplicate/recovery design.

Authenticate destination registration/refresh/removal, bind to the actor and verify ownership on reassignment. Handle multiple devices, token rotation, logout/account switching, expiration and invalid destinations. Protect credentials/destinations; redact endpoints, keys and tokens. Direct Web Push must constrain HTTPS destinations, ports, redirects and DNS to prevent SSRF. Allow-list deep links and prefer notification IDs over private lock-screen payloads.

Respect consent/preferences, quiet hours, locale, TTL and collapse semantics. Classify permanent/retryable failures and use bounded jittered retries; unknown send outcomes may duplicate. Provider acceptance proves neither device delivery nor display/read. Browser service workers, permission UX, mobile SDKs and device tests need separate exact-file scope and authorization. No exactly-once delivery or background-execution SLA is implied.

## 4. Selected-topic routes and evidence

Read only selected topics, and adapt destination routes to imported files:

- Persistence: [relational providers](../data/relational-providers.md), [MongoDB](../data/mongodb.md), [Elasticsearch](../data/elasticsearch.md).
- Transport/use cases: [gRPC](../api/grpc.md), [SignalR](../api/signalr.md), [chat and notifications](../engineering/chat-and-notifications.md).
- Operations: [Redis](../operations/redis.md), [RabbitMQ](../operations/rabbitmq.md), [Hangfire](../operations/hangfire.md), [observability](../operations/observability.md), [push](../operations/push-notifications.md).

These are approved reference topic candidates, not proof of installed packages. Record each capability as proposed, verified, unavailable or deferred with exact package/server/edition versions, license review, date, tested matrix and limitations. Mark a document Verified only when its complete scope has executable evidence. Cover each adapter and critical pairs rather than all 2^N combinations; live services, restores, containers and devices require separate permission.
