# Database & Storage Overview

> **Classification:** `[PROFILE]`
> **Status:** Draft | **Owner:** Database Engineering / Architecture
> **Last verified:** Not verified | **Evidence:** Approved design constraints, not provider certification

## Scope and evidence

This baseline describes optional persistence for approved .NET 10 Clean Architecture/CQRS targets.
It does not select a production database or install an adapter.
No `docs/repository-profile.md` currently declares an active persistence provider.
Verify the destination profile, manifests and source before implementing persistence.
The isolated `examples/Baseline.Sample` demonstrates in-memory Items only.
It has no durable database, migrations or certified external provider.
Reference-only brownfield adoption preserves the existing runtime and persistence boundaries.

## Selection rules

- Select one primary authoritative store per bounded context, or `none` for stateless work.
- Choose SQL Server, MySQL, PostgreSQL or MongoDB only with explicit approval.
- CQRS does not require separate databases, event sourcing or a broker.
- Multiple contexts/stores require named ownership and a consistency/reconciliation plan.
- Omit packages, configuration, containers and health checks for unselected capabilities.
- Never infer a second store, Redis or RabbitMQ from the primary-store selection.

| Role | Optional choice | Contract |
|---|---|---|
| Relational authority | SQL Server / MySQL / PostgreSQL | Verified engine constraints and transaction boundary |
| Document authority or read model | MongoDB | Explicit collections, indexes and topology |
| Search projection | Elasticsearch | Rebuildable derived data; not business source of truth |
| Cache | Redis, when separately selected | Disposable data; never authoritative financial state |
| Recovery intent | Primary-store outbox or equivalent | Persist with associated state when atomicity is required |

## Layer and schema ownership

Domain owns business entities, value objects and invariants without vendor types; Infrastructure must not duplicate those entities as persistence models.
Application owns typed use cases and only the persistence ports they need.
Infrastructure owns selected clients, mappings, queries and migration assets. For selected EF adapters, shared `ApplicationDbContext` configuration scans explicitly use `typeof(ApplicationDbContext).Assembly`; provider contexts call the base registration before scanning their own assembly. Order within an assembly scan is undefined; see [mapping ownership](./relational-providers.md#ownership-and-migration-assets).
API and optional Worker are composition roots; scope transaction resources per operation.
Do not introduce speculative repositories or leak driver/ORM types into application contracts.
Declare authority and deployment permissions in [schema ownership](./schema-ownership.md).
Follow [compatible evolution](./schema-change-process.md) for every owned schema change.

## Integrity and recovery

Enforce mutable invariants at the authoritative write boundary, not only in earlier reads.
Use constraints, atomic conditions, concurrency tokens or bounded locks as appropriate.
Use bounded pagination and parameterized SQL; document ordering and query limits.
For money, define currency, precision, rounding and overflow; forbid binary floating point.
Choose identity and UTC/time-zone semantics from actual domain requirements.
Do not invent User, Tenant, ledger or authentication schemas for an Items example.
Distinguish confirmed commit, confirmed rollback and unknown outcome.
Resolve ambiguous outcomes through durable operation state before retrying mutations.
Required external effects need durable recovery intent and idempotent delivery.
No atomic transaction across a database, MongoDB, Elasticsearch or broker is promised.

## Provider and acceptance routes

- [Relational providers](./relational-providers.md): compatibility, engine behavior and separate migrations.
- [MongoDB](./mongodb.md): native driver, collection evolution and replica-set tests.
- [Elasticsearch](./elasticsearch.md): mappings, refresh, aliases and rebuild recovery.
- [Persistence standards](../standards/persistence-and-concurrency.md): authoritative transaction rules.
- [Configuration](../operations/configuration.md): proposed keys and secret injection.

Before certification, record exact runtime/client/server versions and execute isolated real-engine tests.
Cover constraints, concurrency, recovery, bounded reads and migration compatibility.
In-memory tests prove neither provider compatibility nor durable transaction behavior.
No build, database connection or migration is authorized by this reference document.
