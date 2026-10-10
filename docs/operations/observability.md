# Observability, Signal Ownership and Health

> **Classification:** `[PROFILE]`
> **Status:** Draft | **Owner:** SRE / Platform Engineering
> **Last verified:** Not verified | **Evidence:** Reference policy; no telemetry exporter or external adapter certification

## Selection and ownership

For selected .NET 10 Clean Architecture/CQRS targets, configure telemetry at the API/optional Worker composition roots; Domain/Application do not depend on vendor SDK types. All products are optional and independently selected. The volatile in-memory Items sample is not Seq/Sentry/OTel integration evidence. Follow [telemetry standards](../standards/observability-and-operations.md) and record exact package/backend compatibility before enabling an adapter.

| Signal | Selected owner/path | Guardrail |
|---|---|---|
| Structured logs | Microsoft logging abstractions to configured provider, console JSON and optionally Seq | Choose one Seq route: reviewed Serilog sink or verified OTLP log path; no duplicate sink plus exporter delivery |
| Traces | Activity/OTel instrumentation to OTLP collector and approved backend | One sampling/export strategy; avoid overlapping auto/custom instrumentation |
| Metrics | Selected meters/OTel to approved collector/backend | Stable names/units and bounded dimensions; no user, tenant, resource or message IDs |
| Error events | Sentry SDK when selected | One capture owner per failure across middleware, handlers and background processing |
| Sentry performance | Separately selected supported SDK tracing or verified OTel interoperability | Verify exact bridge/version/backend support; do not assume generic OTLP ingestion or enable two trace routes |
| Search projections | Selected Elasticsearch adapter | Search does not select a logging stack; Seq does not require Elasticsearch |

Define service/environment/release identity, instrumentation, exporter destination/authentication, sampling, log levels, retention and owners in the target profile. Use named structured properties, not interpolated payloads. Correlate logs with trace/span IDs and propagate reviewed W3C trace context through HTTP/gRPC, RabbitMQ headers and job metadata. Trace context is not authorization; validate identity/tenant context separately. Create a processing span per delivery/attempt with the selected parent/link policy; bound baggage and discard unsafe attributes.

## .NET 10 handled-exception diagnostics policy

.NET 10 exception-handler middleware suppresses diagnostics by default for exceptions reported handled by an exception handler. Each target must explicitly choose and verify whether to retain that suppression with one deliberate error/log capture owner, or restore diagnostics under a reviewed policy. Do not silently lose unexpected handled failures, or restore framework diagnostics while also manually capturing the same Sentry event. Expected validation/conflict outcomes should not become duplicate unexpected-error reports.

Test a handled unexpected exception, an unhandled exception and expected client errors through the actual selected middleware/SDK pipeline. Assert intended log/metric/trace visibility and exactly one intended Sentry capture per unexpected failure; suppression of framework diagnostics alone is not proof that SDK instrumentation suppresses duplicate events. Verify the selected .NET 10 configuration API before implementation; this guide supplies no executable configuration.

## Privacy, budgets and degradation

Scrub credentials, authorization headers, cookies, connection strings, push destinations, query-string tokens and private chat/inbox content before logs, spans or error events leave the process. Disable body capture by default and review SQL statements/parameters, URLs, breadcrumbs, exception data, local variables and baggage. User/tenant identifiers in logs/traces require purpose/access/retention review; never use them as metric labels. See [security](../security/overview.md) and [secrets](../security/secrets-management.md).

Bound queue sizes, batches, export timeouts, retries and disk buffering; define drop behavior, shutdown flush budget, cost/volume alerts and collector-outage behavior. An optional exporter outage must not block business requests or cause unbounded memory/disk growth. Exclude or sample health probes. Operational telemetry is not durable audit, financial ledger, message deduplication or delivery confirmation.

## Health and operational signals

The following are proposed internal routes, not installed endpoints:

| Probe | Meaning | Dependency policy |
|---|---|---|
| `/health/live` | Process responsiveness | No remote calls; downstream outage is not a liveness failure |
| `/health/ready` | Ability to serve the instance's selected endpoint/work set | Only critical dependencies; set bounded timeouts and reveal no secrets/topology |
| `/health/startup` | Completion of required initialization, when needed | Do not turn optional cache warming/exporter connectivity into a mandatory startup gate |

Define criticality per host and endpoint set. Optional Redis cache bypass, Seq/Sentry/OTel export, search-only features or push channel failures must not automatically remove all API traffic. A dependency can be critical only under an explicit serving contract. Worker readiness reflects required broker/job-storage availability and ability to process safely; readiness failure must not create liveness restart loops. Alert on outages and stalled processing even when API readiness remains healthy.

Monitor request latency/error/saturation, cache hit/miss/timeouts, outbox age, broker backlog/unacknowledged/retries/DLQ, job queue age/failures, projection freshness and push attempt failures only when selected. Set owned SLO-based thresholds and runbooks; provider acceptance, live publication and user read state are distinct outcomes.

## Acceptance gates

Verify one export/capture per selected signal, cross-host trace parentage, bounded cardinality/buffers, redaction and probe privacy. Exercise collector/dependency outages and disabled modules with zero outbound traffic. Record exact versions and isolated test results; none of these runtime checks are certified by this reference.
