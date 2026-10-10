# Hangfire Durable Scheduling and Job Safety

> **Classification:** `[PROFILE]`
> **Status:** Draft | **Owner:** Backend / Platform Engineering
> **Last verified:** Not verified | **Evidence:** Optional reference design; Hangfire storage adapter not certified

## Selection and ownership

Hangfire is an optional persistent delayed/recurring job engine, not a required worker or notification framework. Select durable storage independently of the business store. Redis cache selection does not enable Redis job storage; review community/commercial adapter support and licensing separately. Omit packages, storage schemas, dashboard, credentials, probes and hosted processors when unselected. The sample contains a separately compiled SQL Server scheduler adapter, not selected by its default memory host; no worker/dashboard/schema preparation or live job execution is certified.

For selected .NET 10 Clean Architecture/CQRS targets, Application owns scheduling/use-case ports, Infrastructure implements Hangfire adapters and API/optional Worker composition roots configure them. Prefer a separate Worker where lifecycle/scaling requires it; document in-process trade-offs explicitly. Candidate Hangfire Core/ASP.NET Core and storage adapter packages require exact framework/package/server compatibility, maintenance, edition and license evidence. Do not assume all providers, paid features or versions have equivalent support.

## Durable storage and scheduling boundary

Use a reviewed persistent adapter with explicit schema/provisioning authority, isolation, backups, restoration, retention and cleanup. A process-local/in-memory provider cannot establish durable restart recovery. Review storage connection limits and worker concurrency independently of the API business pool; no singleton may capture scoped dependencies.

Enqueue acceptance proves a job was stored under the selected adapter's contract, not that it executed successfully. Business commit and enqueue are not automatically atomic, even if stores share an engine. Required scheduling recovery needs atomic durable intent/outbox or another explicitly verified protocol, with a dispatcher and reconciliation. Unknown enqueue outcomes may create duplicate jobs; retain stable business operation IDs. Define recurring job identity, time zone, misfire/overlap expectations, owner and old/new payload compatibility before rollout; schedules are not exact execution-time guarantees.

## Retry-safe execution

Pass small versioned contracts/stable IDs, not secrets, DbContext, scoped services or serialized aggregates. Resolve a fresh scope per execution and revalidate current business state/authorization policy. Job arguments are persisted and can appear in the dashboard, logs and backups.

Jobs can execute repeatedly after retry, shutdown, lock loss or worker failure. Use authoritative idempotency/uniqueness and transaction boundaries for effects; scheduling locks or concurrency filters do not prove exactly-once execution. External sends can have unknown outcomes and require provider idempotency or reconciliation. Configure bounded attempts/backoff, per-job timeout/cancellation, retryable/permanent classification and failed-job review; do not rely on unspecified defaults. Record terminal failures and controlled manual retry policy. Stop intake and drain cooperatively within the deployment shutdown budget; incomplete work can repeat.

## Dashboard, secrets and readiness

Disable the dashboard unless selected. When enabled, require explicit authentication and privileged authorization, private/restricted network exposure, TLS and appropriate CSRF protections for administrative actions. Never rely on development/local defaults in production. Review read-only versus mutation privileges, audit retries/deletion and redact sensitive job details. Dashboard access denial is a release gate.

Use `Hangfire` and `ConnectionStrings:Hangfire` conventions from [configuration](./configuration.md), backed by [approved secret injection](../security/secrets-management.md). Required storage outages affect Worker processing readiness, not liveness. API readiness depends on whether safe scheduling is required for its assigned endpoints; optional scheduling failure must not automatically remove unrelated traffic. Monitor queue age, active/failed/retried jobs, schedule lag, storage latency and cleanup pressure.

## Acceptance and limitations

Test durable restart/restore, duplicate and concurrent execution, crash after business effect, scheduling split outcomes, bounded retries/permanent failure, cancellation/drain, recurring overlap, payload version coexistence, storage outage and dashboard denial. Record exact adapter/license/version/topology evidence; no integration is certified here. Follow [worker resilience](../standards/resilience-and-workers.md), [persistence standards](../standards/persistence-and-concurrency.md), [deployment](./deployment.md) and [observability](./observability.md).
