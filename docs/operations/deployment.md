# Deployment and Selected Host Lifecycle

> **Classification:** `[PROFILE]`
> **Status:** Draft | **Owner:** DevOps / Release Management
> **Last verified:** Not verified | **Evidence:** Reference design; no external adapter or deployment certification

## Scope and release gates

Approved greenfield targets use .NET 10 Clean Architecture/CQRS, an API composition root and an optional Worker composition root. Existing targets preserve their verified runtime and deployment contracts unless migration is separately approved. This reference deploys nothing. `examples/Baseline.Sample` demonstrates volatile in-memory Items only; it does not demonstrate external adapters, durable jobs, chat or inbox storage.

Select modules from the [catalog](../architecture/optional-stack-catalog.md). Omit unselected packages, containers, credentials, probes, hosted services and egress. Verify exact SDK, package, provider/server and edition versions, support and licensing before claiming compatibility. Record unavailable/deferred combinations; a successful build alone does not certify an adapter. Apply [delivery standards](../standards/delivery-and-supply-chain.md) and [configuration validation](./configuration.md).

## Deployment sequence

1. Produce one immutable, identified artifact per selected host after authorized build, contract/security tests and selected adapter integration tests. Promote the same artifact across environments; inject reviewed configuration and secrets, never rebuild for credentials.
2. Review rollback and data compatibility. Back up and test restoration for selected durable stores. Apply only required backward-compatible schema expansion through the approved migration authority, not every API replica at startup. Include selected outbox/deduplication/job schemas and broker topology; review provider tooling ownership independently.
3. Deploy compatible API and Worker versions in an explicit order. New and old producers/consumers/job executors must understand contracts during overlap. Keep consumers paused until required schema/topology is ready. Use versioned small payloads and stable job identifiers; do not serialize runtime services.
4. Canary the selected endpoint set and workers; verify [readiness](./observability.md), non-destructive authorized smoke checks, latency/errors, backlog age and recovery progress. Shift traffic only after the release-specific observation window and acceptance thresholds pass.
5. Run bounded backfills/projection rebuilds as separately controlled operations. Contract obsolete schema/message/job formats only after old instances and retained/retry payloads no longer need them. Rolling back binaries does not reverse data migrations or external effects.

A stateless target needs no schema stage. Cache selection does not create a business store; Hangfire storage and RabbitMQ durability are separate decisions. Never promise atomic commits across stores, a broker or a job scheduler. Required publication/scheduling recovery needs durable intent, idempotency and reconciliation; see [RabbitMQ](./rabbitmq.md), [Hangfire](./hangfire.md) and [persistence standards](../standards/persistence-and-concurrency.md).

## Hosting, draining and failure isolation

- Scale API and Worker independently when selected. In-process jobs/consumers require an explicit lifecycle, resource and scaling trade-off; they are not enabled by default.
- On shutdown, stop accepting work, allow bounded completion, cancel cooperatively and release resources. Acknowledge only committed consumer work; abandoned jobs/deliveries can repeat after restart. Never mark an ambiguous external outcome as confirmed rollback.
- For selected SignalR, test proxy upgrades/timeouts, capacity, affinity and rolling-drain reconnect. Single node needs no Redis. Cache and backplane roles are independent; neither backplane nor managed transport is durable history. See [Redis](./redis.md) and [SignalR](../api/signalr.md).
- Restrict administration, dashboard and health listeners to reviewed networks and identities. Grant selected egress only. Rotate credentials through the [secret lifecycle](../security/secrets-management.md).
- Optional cache/exporter/push outages do not automatically fail API readiness. Required worker broker/job storage failures affect processing readiness, not process liveness. Report degraded channels and backlog separately.

## Acceptance evidence

Record release identity, configuration/schema versions, compatibility matrix, migration authority, smoke results, rollback limits and owners. Test restart/drain, old/new contract coexistence, dependency outages, bounded retries, dashboard denial and absence of unselected traffic in isolated authorized environments. No live dependencies, migrations, deployment commands or runtime validation are supplied by this guide.
