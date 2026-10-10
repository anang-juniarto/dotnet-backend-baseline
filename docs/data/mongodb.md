# Optional MongoDB Persistence

> **Classification:** `[PROFILE]`
> **Status:** Draft | **Owner:** Database Engineering
> **Last verified:** Not verified | **Evidence:** Native-driver design target, no integration certification

## Purpose and selection

Select MongoDB explicitly as primary document storage or a named read model.
Use one authoritative primary store per bounded context; CQRS does not require MongoDB.
Adding MongoDB alongside a relational store needs purpose, ownership and reconciliation rules.
Omit its packages, settings, containers and health checks when unselected.
The sample includes a separately compiled MongoDB item-store adapter, not selected by its default memory host. Live document storage/index/transaction behavior remains unverified; see the sample README/profile.

## Driver and layer boundary

The candidate access family is the native `MongoDB.Driver`, not an assumed EF provider.
No installed dependency, exact version or .NET 10 compatibility is claimed here.
Verify selected driver/runtime/server compatibility, licensing and supported topology first.
Infrastructure owns client configuration, serialization, collections and driver queries.
Domain/Application contain no BSON, collection handles or driver session types.
Application ports express required use cases, not a generic document repository by default.
Share clients only under verified driver lifetime guidance; never share operation sessions blindly.

## Collections, indexes and evolution

Declare collection authority in [schema ownership](./schema-ownership.md).
Record document shape/version, ID strategy, validation, indexes and retention ownership.
Optional document fields must be tolerated during rolling upgrades.
Version transformations and index/validator changes; EF migration commands do not apply.
Backfill with bounded batches, checkpoints and restart-safe conditional updates.
Before tightening validators or creating unique indexes, validate existing documents.
Check index build resource/availability impact and duplicate/collation semantics.
Define required unique indexes for business keys and durable deduplication keys.
A prior existence query alone does not enforce uniqueness under concurrent writes.
TTL cleanup is asynchronous; it is not an exact expiry or authorization guarantee.
Follow [expand-migrate-contract](./schema-change-process.md) for compatible transitions.

## Concurrency and transaction topology

Prefer a single-document atomic update where it preserves the complete invariant.
Use an expected-version predicate or another atomic condition for conflicting updates.
Inspect matched/modified outcomes and map conflicts explicitly.
Use multi-document transactions only for an actual atomicity requirement.
Transactions require a supported replica-set or sharded deployment, not a standalone server.
Verify session, read/write concern, retry and commit semantics for the exact topology.
Do not claim transactions merely because a local standalone CRUD test passes.
Keep transactions short; do not make unrelated external calls while they are open.
Persist required recovery intent with associated state atomically when topology permits.
When atomic persistence is unavailable, explicitly design durable recoverable intent and reconciliation.
No transaction spans MongoDB, a relational store, Elasticsearch or a broker by default.

## Query, configuration and security

Bound result sizes, batch sizes and operation deadlines; use stable pagination ordering.
Build typed/validated predicates; never execute arbitrary client-supplied query operators.
Enforce actor/context scope in queries and projections; return only necessary fields.
Use proposed `MongoDb:Database` and `ConnectionStrings:MongoDb` conventions for new targets.
Verify actual bound options in [configuration](../operations/configuration.md).
Inject credentials through approved secret sources, require TLS and least-privilege roles.
Separate deployment/index administration from runtime access; never default to production.

## Recovery and acceptance

Distinguish confirmed commit, confirmed rollback and unknown outcome after interrupted commit.
Resolve durable operation state before replaying ambiguous mutations.
Required cross-store effects use outbox/equivalent recovery intent, deduplication and reconciliation.
Test against the exact selected server and transaction-capable topology when transactions are used.
Cover duplicate keys, concurrent conditional writes, serialization and mixed document versions.
Test index/validator rollout, resumable backfill, failover and interrupted commit recovery.
Verify backup/restore and outbox replay without duplicating business effects.
Record executed versions/results and skipped checks; standalone CRUD is limited evidence only.
See [persistence standards](../standards/persistence-and-concurrency.md) for authoritative recovery rules.
