# RabbitMQ Publication and Consumer Recovery

> **Classification:** `[PROFILE]`
> **Status:** Draft | **Owner:** Backend / Messaging Platform
> **Last verified:** Not verified | **Evidence:** Optional reference design; RabbitMQ adapter not certified

## Purpose and ownership

Select RabbitMQ only for approved asynchronous integration delivery. It is not a database transaction coordinator, mandatory notification dependency or exactly-once business transport. Omit packages, broker topology, credentials, probes and hosted consumers when unselected. The default in-memory Items profile does not publish business events or provide a durable outbox. The [sample README](../../examples/Baseline.Sample/README.md) separately records optional messaging adapter compilation and its runtime limits.

For selected .NET 10 Clean Architecture/CQRS targets, Application owns publication/use-case ports and versioned contracts; Infrastructure implements broker/outbox adapters. API and optional Worker composition roots own wiring. Consumers invoke typed application handlers using a fresh DI scope per delivery; singleton connections/consumers never capture scoped services or DbContext. The canonical selected messaging module is `<Project>.Infrastructure.Messaging` using MassTransit with RabbitMQ; preserve existing RabbitMQ.Client or other verified frameworks in brownfield projects. Exact versions and licensing must be reviewed rather than assuming the newest major release has the same terms. Verify exact client/broker versions, channel concurrency rules, topology features and licenses before implementation.

## Publication and durable intent

When business-required publication must survive crashes, persist business state and outbox intent atomically in the authoritative transactional store. A dispatcher publishes stable event IDs/versioned small contracts and marks intent dispatched only after the reviewed publisher-confirm and routing outcome. If the store cannot support that atomic boundary, define an alternative recoverable design or defer the guarantee; never claim database and broker share a transaction.

Select durable exchanges/queues, persistent messages and a reviewed queue/replication policy appropriate to required recovery; validate bindings and handle unroutable messages through an explicit mandatory-return/alternate-routing policy. Publisher confirms establish broker-side acceptance under the selected topology, not consumer completion or business exactly-once execution. Confirm loss/timeouts can leave an unknown outcome: retain intent and retry using the same event identity. A crash after publish but before marking the outbox can produce duplicates. Bound dispatcher batches, retries/backoff, retention and concurrent claiming; alert on oldest undispatched intent and reconcile stalled records.

## Consumer semantics

- Use manual acknowledgments only after required durable business processing and deduplication commit. Acknowledging early can lose work; ack loss after commit can redeliver it.
- Deduplicate by stable event ID and consumer/operation scope using authoritative uniqueness/inbox state committed atomically with the effect where possible. Broker delivery tags are not business deduplication keys. External effects require their own idempotency/reconciliation design.
- Bound prefetch, concurrency, payload size, deserialization depth and processing time. Validate schema versions and trusted context; never treat message tenant/actor claims as authorization. Propagate approved trace context, not secrets or service instances.
- Classify transient versus permanent failures. Use bounded delayed retries/backoff and delivery limits; avoid immediate requeue loops. Reject/quarantine unsupported or poison payloads to an explicitly provisioned DLQ with reason/attempt metadata and retention/access controls. Verify dead-letter transfer durability for the selected queue policy; it is not an unconditional lossless guarantee.
- DLQ replay is an authorized operation after diagnosis/fix, preserving event identity and deduplication. It must not reset retry budgets endlessly. Drain on shutdown within a bound; uncommitted deliveries remain unacknowledged for recovery.

## Configuration, security and acceptance

Use the proposed `RabbitMq` routing/prefetch/retry settings and `ConnectionStrings:RabbitMq` from [configuration](./configuration.md). Isolate environments/vhosts; grant publishers/consumers only required topology permissions. Use reviewed TLS identities, private listeners and restricted management access. Payloads/outbox/DLQ records can hold sensitive data: minimize, protect and expire them under [security policy](../security/overview.md).

Test commit-to-publish crash recovery, confirms/returns/unknown outcomes, duplicate and concurrent deliveries, commit-before-ack restart, poison messages, retry exhaustion/DLQ/replay, old/new schemas and broker outages. Observe backlog age, unacknowledged deliveries, redelivery, outbox lag and DLQ growth without high-cardinality labels. Required Worker connectivity affects processing readiness, not liveness; API readiness depends on its serving contract, not broker selection alone.

See [distributed-system standards](../standards/distributed-systems.md), [worker resilience](../standards/resilience-and-workers.md) and [observability](./observability.md). Exact versions and real isolated recovery evidence are required; none are certified here.
