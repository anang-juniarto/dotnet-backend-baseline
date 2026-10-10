# Database Schema Authority & Ownership

> **Classification:** `[STANDARD]`
> **Status:** Draft | **Owner:** Database Engineering / Architecture
> **Last verified:** Not verified | **Evidence:** Governance design, no deployed schema certification

## Declare authority before changing persistence

No active provider or schema authority is declared by this reference baseline.
Discover destination authority from its verified profile, manifests and migration assets.
Use evidence to identify ownership; unresolved authority blocks only affected unsafe changes.
Do not infer installation or approval from the candidate tools below.
Declare one authority for each owned object set within each bounded context.
A hybrid application is valid only with explicit non-overlapping ownership and ordering.

| Authority model | Maintained source | Boundary |
|---|---|---|
| Migration/code-first relational | Reviewed model/configuration and provider migration history | Only selected compatible ORM/provider |
| SQL-first relational | Versioned reviewed SQL and declared runner | Models map to the controlled schema |
| External/platform-managed | Owner contract and approved change procedure | Application is a consumer, not DDL owner |
| Document-store ownership | Collection contracts, validators, index definitions and versioned transforms | Native driver; no implied EF migrations |
| Search projection ownership | Versioned mappings, index settings, aliases and rebuild procedure | Derived data only; authority remains elsewhere |

## Required ownership record

Record in the destination's maintained profile or existing schema registry:

- Bounded context, authoritative store and exact object/collection/index ownership.
- Responsible application/team and schema-review approver.
- Selected runtime, provider/client, server versions and compatibility evidence.
- Authoritative schema assets, migration runner and deployment identity.
- Consumer inventory, backward-compatibility window and rollout order.
- Backup/restore owner, recovery targets and reconciliation ownership.
- Idempotency/outbox ownership, retention and cleanup policy when required.
- Shared-object dependencies and who approves cross-context changes.

Do not add an unapproved migration tool or a duplicate schema authority.
Do not share writable tables/collections between contexts without a declared contract.
Readers must not mutate another context's schema or search projection directly.

## Provider-specific responsibilities

Keep SQL Server, MySQL and PostgreSQL migrations and model snapshots separate.
Domain owns shared business entities and value objects; Infrastructure owns EF configuration and provider-specific migration assets, not duplicate business entities. Shared configuration registration explicitly scans `typeof(ApplicationDbContext).Assembly`; provider contexts register the base model before their own assembly. Intra-assembly scan order is undefined; conflicting mappings need deterministic registration as described in [relational ownership](./relational-providers.md#ownership-and-migration-assets).
A shared entity model does not prove portable types, constraints or generated SQL.
For MongoDB, own collection validation, indexes and resumable document transformations.
For Elasticsearch, own projection versions, aliases, checkpoints and rebuild/delete propagation.
Index names, uniqueness, collation and retention are governed schema contracts.
Review lock/build impact before creating indexes on any populated store.
See [relational](./relational-providers.md), [MongoDB](./mongodb.md) and [search](./elasticsearch.md).

## Deployment authority

| Environment | Execution owner | Required boundary |
|---|---|---|
| Local | Explicit developer action or reviewed opt-in automation | Isolated verified local target |
| CI/test | Approved runner | Disposable database and restricted credentials |
| Staging/production | Dedicated controlled release identity | Approval, compatibility and recovery gates |

Runtime identities use least privilege; production DDL is not a default permission.
Do not apply migrations automatically on normal application startup by default.
Local opt-in automation does not grant staging/production authority.
Do not connect to live databases or execute destructive changes without explicit authorization.
Schema approval and execution permission are separate gates.

## Review handoff

Use [schema change process](./schema-change-process.md) for expand/backfill/contract evidence.
Preserve external authority procedures and report unresolved consumer dependencies.
Treat unknown commit outcomes and multi-store recovery per [persistence standards](../standards/persistence-and-concurrency.md).
The in-memory Items sample establishes no migration, collection or index authority.
