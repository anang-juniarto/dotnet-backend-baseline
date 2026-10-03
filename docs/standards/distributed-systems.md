# Scalability, Caching, Messaging, Backpressure, and Consistency Standards

> **Classification:** `[CORE STANDARD]`  
> **Status:** Normative Standard  
> **Applicability:** Active when distributed caching, messaging/outbox patterns, read replicas, backpressure, large payload streaming, distributed sagas, or multi-resource reconciliation are employed.  
> **Source Migration:** Formed from Section 10 of the Enterprise .NET Backend AI Engineering Guide.

---

## 10. Scalability, Caching, Messaging, Backpressure, and Consistency

### 10.1 Evidence-Driven Caching

Introduce caching only when measurements show sufficient benefit and consistency risk is accepted. Define:
- exactly one bounded context/service as owner of each cache namespace and invalidation contract;
- tenant partitioning, TTL and eviction;
- serialization/schema versioning;
- invalidation/update strategy after commit;
- cache penetration and stampede protection when load justifies it;
- cache outage behavior and database protection;
- maximum stale age and whether stale responses are contractually permitted.

Consumers MUST NOT mutate cache entries owned by another authority except through an approved interface or event. Cache update or invalidation failure after confirmed commit MUST be handled through bounded retry, durable invalidation intent, expiry, safe bypass/purge, or reconciliation. It cannot roll back the completed transaction. Compensation is valid only for an independent business invariant and is a new audited business operation; cache invalidation failure alone is never grounds for compensation.

Redis, Valkey, or another distributed cache is an implementation choice requiring an existing approved dependency or ADR.

### 10.2 Messaging and Outbox

Use asynchronous messaging when decoupling, load leveling, or durable delivery is required.
- Use a transactional outbox when database state and message intent must commit atomically.
- Design consumers for the broker's delivery semantics; at-least-once delivery requires idempotency/deduplication.
- Version message contracts compatibly and include message ID, type/version, timestamp, correlation ID, causation ID, and tenant context where applicable.
- Consumers SHOULD tolerate additive unknown fields when the serialization format permits it. Never change the semantic meaning of an existing field; remove or rename fields only through a compatible multi-release deprecation.
- Enforce schema compatibility in CI when a registry or compatibility tool is part of the approved platform.
- Define ordering scope and partition key explicitly. Event timestamps do not guarantee delivery or processing order.
- Define replay behavior, retention, retry, dead-letter handling, and reconciliation.
- Define archival and purge policies for processed outbox/inbox records; cleanup MUST preserve records still required for deduplication, audit, reconciliation, legal hold, or incident investigation.
- Broker/product selection requires an ADR and verified dependency.

### 10.3 Read Replicas and Consistency

Use replicas only when measured read load justifies operational complexity. Document lag expectations and route read-your-own-writes, authorization-critical, financial, and invariant-sensitive reads to an appropriately consistent source.

### 10.4 Backpressure and Capacity

Bound queues, channels, worker concurrency, connection pools, and buffers. Define overload behavior: reject, shed, degrade, delay, or persist. Unbounded in-memory queues are forbidden.

### 10.5 Payload and Streaming Safety

For large payloads or files:
- prefer bounded streaming over full buffering;
- enforce size, type, filename, decompression, and archive expansion limits;
- scan untrusted files where the threat model requires it;
- protect against compression bombs and unsafe content handling;
- propagate cancellation and clean up partial data;
- avoid response compression for secret-bearing content where side-channel risk applies.

### 10.6 Multi-Resource Workflows, Sagas, and Compensation

When one business workflow spans multiple authoritative systems:
- select orchestration or choreography through an ADR that defines ownership, observability, ordering, and failure handling;
- never claim ACID atomicity across independent services unless a verified distributed transaction protocol actually provides it;
- persist workflow state and make each step idempotent, resumable, and safe to replay;
- define compensating actions for committed steps, recognizing that compensation is a new business action rather than a perfect rollback;
- handle partial completion, duplicate messages, timeout ambiguity, manual intervention, and irreversible steps explicitly;
- expose workflow age, state, retries, compensation failures, and stuck instances operationally.

### 10.7 Reconciliation and Invariant Repair

When state spans authoritative storage, caches, brokers, external providers, or financial ledgers:
- define scheduled reconciliation between authoritative and derived/external state;
- detect stuck outbox/inbox records and orphaned messages or jobs;
- make replay and repair operations idempotent and auditable;
- reconcile financial balances against ledger entries and external settlement records;
- assign operational ownership, alerts, runbooks, escalation, and repair authorization.
