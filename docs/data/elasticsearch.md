# Optional Elasticsearch Search Projections

> **Classification:** `[PROFILE]`
> **Status:** Draft | **Owner:** Database Engineering / Search Operations
> **Last verified:** Not verified | **Evidence:** Derived-search design target, no client/server certification

## Purpose and non-goals

Select Elasticsearch explicitly for search and derived projections.
It is not the authoritative business store, ledger or durable idempotency authority.
CQRS does not require Elasticsearch or a second database.
Selecting search does not select an Elastic logging stack, Redis or RabbitMQ.
Omit packages, settings, containers and health checks when unselected.
The sample includes a separately compiled owner-filtered Elasticsearch projection adapter, not selected by its default memory host. No live search/index or durable recovery pipeline is certified; see the sample README/profile.

## Compatibility and ownership

The candidate dependency is the current Elastic .NET client family.
Verify exact client/server/.NET 10 compatibility, license and supported APIs before implementation.
No installed dependency, tested version or available managed deployment is claimed here.
Infrastructure owns client access, projection mapping, index settings and alias operations.
Application owns a use-case-specific search port without vendor query/response types.
Declare index/alias ownership and authoritative source in [schema ownership](./schema-ownership.md).
Define projection version, document IDs, deletion semantics and rebuild ownership.
Do not expose arbitrary index names or administrative APIs through application contracts.

## Mapping and index lifecycle

Version mappings/settings and use explicit field/analyzer choices for required queries.
Review dynamic-field limits and prevent user-controlled field explosion.
Plan incompatible mapping changes as a new versioned index, not an in-place assumption.
Build a replacement from the authoritative store using bounded checkpoints.
Capture/replay changes during rebuild so a snapshot copy cannot silently lose new writes.
Validate counts, sample contents, relevance, scope filters and deletion propagation.
Switch the serving alias only after validation and caught-up change processing.
Keep old indexes for a bounded, approved rollback window; delete only with authorization.
Alias switching does not make the source database and search index atomic.
Use [schema change process](./schema-change-process.md) for consumer-compatible rollout.

## Publication and recovery contract

Search refresh is asynchronous; a successful write does not promise immediate search visibility.
Define tolerated lag and what the caller sees while a projection is stale or unavailable.
Use synchronous best-effort updates only when lost updates are acceptable and rebuildable.
Required recovery needs a durable committed-change pipeline or equivalent persisted intent.
Store an outbox with the authoritative mutation atomically where supported.
Publish/project after confirmed commit; a broker is optional, not implied by outbox selection.
Retry in bounded batches; handle partial bulk failures per item rather than retrying blindly.
Use stable source IDs and versions to deduplicate and reject stale updates.
Propagate deletions/tombstones; prevent delayed updates from resurrecting deleted records.
Own checkpoints, poison-record handling, lag alarms and periodic reconciliation.
No atomic transaction spans Elasticsearch and the source store or message broker.
Keep an unknown source commit outcome pending until authoritative reconciliation resolves it.

## Query, configuration and security

Bound page size, query complexity, deadlines and deep-pagination behavior.
Select stable pagination supported by the verified client/server; no unbounded result export.
Validate query inputs; do not pass raw user DSL or scripting into privileged APIs.
Enforce actor/context filters on every search and avoid leaking inaccessible documents/counts.
Use authoritative checks when stale projections could violate authorization or invariants.
Proposed keys are `Elasticsearch:Endpoint`, `IndexPrefix`, request timeout and `Elasticsearch:ApiKey`.
See [configuration](../operations/configuration.md) for conventions, not installed options.
Inject secrets, require TLS and scope credentials to owned indexes and required operations.
Isolate environments; restrict administration and never default to production.
A noncritical search outage should degrade explicitly, not silently change business truth.

## Acceptance and operational gates

Test exact client/server versions against an isolated real cluster.
Cover mapping/query behavior, bounded reads, refresh lag and unavailable search responses.
Test duplicate/out-of-order updates, deletes, partial bulk failures and restart recovery.
Rehearse interrupted rebuild, catch-up, alias cutover, rollback and reconciliation.
Define recovery targets, source retention needs and capacity for concurrent old/new indexes.
Record executed checks and remaining risks; provider certification remains unverified here.
See [persistence standards](../standards/persistence-and-concurrency.md) for transaction boundaries.
