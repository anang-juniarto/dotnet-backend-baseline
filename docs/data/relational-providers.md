# Optional Relational Providers

> **Classification:** `[PROFILE]`
> **Status:** Draft | **Owner:** Database Engineering
> **Last verified:** Not verified | **Evidence:** Candidate providers; no restore/build/engine certification

## Selection and compatibility

For approved .NET 10 Clean Architecture/CQRS targets, select one primary store per context.
Relational choices are `none`, `sqlserver`, `mysql` or `postgresql`; no engine is implicit.
MongoDB is a separate document-store selection, not a relational provider.
Omit unselected providers, packages, containers, settings and health checks.
The Items sample is volatile memory only and certifies no relational integration.

| Engine | Candidate access family | Required evidence before selection is certified |
|---|---|---|
| SQL Server | EF Core SQL Server provider | Exact EF Core 10/provider/server compatibility and engine tests |
| MySQL | Verified compatible EF provider | EF Core 10 support, maintenance/license and exact server version |
| PostgreSQL | Npgsql EF provider | Exact provider/EF Core 10/server compatibility and engine tests |

These are reference families, not universal installed dependencies. The canonical layout uses a shared `<Project>.Persistence.EntityFramework` assembly plus selected `<Project>.Persistence.SqlServer`, `.PostgreSql` or `.MySql` provider/migration assemblies under `src/Infrastructure`. Check the [sample profile](../../examples/Baseline.Sample/docs/repository-profile.md) for its exact compiled versions; engine compatibility is not proven by restore alone.
The canonical Pomelo MySQL provider was found only through major version 9 in the inspected index, so its EF10 sample adapter is deferred. An Oracle EF10 provider is not an automatic substitute: client behavior, migrations and GPL/FOSS-exception licensing require explicit review and selection.
MySQL and MariaDB require independent support claims and tests.
A supported native-client path requires separate approval when the EF route is unavailable.
Preserve existing brownfield access technology unless its migration is approved.
Do not add Dapper, a migration runner or repository wrappers merely to match an example.

## Ownership and migration assets

Domain/Application remain free of provider types; Infrastructure owns mappings and access.
Application uses only required persistence ports; composition roots select the adapter.
Place shared `IEntityTypeConfiguration<T>` classes in `Persistence.EntityFramework/Configurations/` mapping Domain entities directly. Business entities and value objects belong in `src/Core/<Project>.Domain`; do not duplicate entities as persistence models. DTOs, transport contracts and search projections retain their respective layer ownership; this does not move EF/provider types into Domain or prohibit value objects. Keep `ApplicationDbContext.OnModelCreating` limited to configuration registration, not inline table/property/relationship rules. The shared context explicitly scans `modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly)`; do not use the runtime context type, which could select the provider assembly instead. Provider-specific mappings (collation, SQL defaults/types) belong in the corresponding provider project's `Configurations/` folder. Each provider context calls `base.OnModelCreating(modelBuilder)` first, then scans its own provider assembly. This establishes shared-before-provider phases; configuration order within each assembly scan is undefined, so conflicting configurations within one phase require explicit deterministic registration. See the [sample mapping layout](../../examples/Baseline.Sample/src/Infrastructure/Baseline.Sample.Persistence.EntityFramework/README.md). Adapt this source-example link in adopted destinations where examples are excluded.
Declare [authority](./schema-ownership.md) before generating or applying schema changes.
Keep separate provider-specific migration sets, model snapshots and deployment artifacts.
Name the provider/context unambiguously in assets and release instructions.
Never apply one provider's generated migrations to another engine.
Test both empty-database creation and upgrades from supported deployed versions.
Verify immutable Domain entity constructor binding and round-trip materialization against the actual engine, including owner collation enforcement, constraints, and transaction rollback. Compile-verified model metadata alone is not live engine certification.
Use [expand-migrate-contract](./schema-change-process.md) for rolling compatibility.

## Engine-specific review

- SQL Server: verify isolation/snapshot policy, concurrency tokens and index/DDL locking.
- MySQL: verify transactional engine, charset/collation, SQL modes and DDL behavior.
- PostgreSQL: verify schema/search-path policy, timestamp semantics and provider-specific types.
- All engines: verify identity generation, decimal precision, UTC handling and uniqueness.
- Do not translate SQL Server row-version behavior into a universal provider guarantee.
- Review actual query plans before claiming performance improvement from an index.

## Configuration and security

Use proposed `Persistence:Provider` and `ConnectionStrings:Primary` conventions only in new targets.
See [configuration](../operations/configuration.md) for bounded timeout/pool options.
Preserve verified brownfield keys unless a configuration migration is approved.
Inject credentials from approved local secrets or deployment secret stores, not tracked files.
Validate the selected provider and required options at startup without logging credentials.
Use TLS and least-privilege runtime identities; a separate identity applies approved DDL.
Never default connection strings to production or execute migrations on ordinary startup.
Parameterize values; allowlist dynamic identifiers and sort choices.
Bound collection queries and define deterministic pagination ordering.

## Transaction and recovery contract

Keep transactions short and protect mutable invariants at the write boundary.
Use constraints, atomic conditions, optimistic tokens or bounded locks as justified.
Persist required outbox/idempotency state with the business mutation in the same transaction.
After commit starts, timeout/cancellation can mean unknown outcome, not confirmed rollback.
Resolve authoritative state before replay; required effects resume from durable intent.
No atomic promise spans another database, MongoDB, Elasticsearch or a message broker.
See [persistence standards](../standards/persistence-and-concurrency.md) for the normative rules.

## Certification checklist

Record exact SDK, EF/provider/client and engine versions with executed results.
Test constraints, concurrent conflicting writes, pagination and decimal/time boundaries.
Test migration locks/compatibility, rollback or forward-fix and restore procedures.
Inject commit ambiguity and duplicate requests; verify idempotent recovery/outbox behavior.
Use isolated real-engine targets; in-memory substitutes do not prove provider semantics.
Report unavailable compatibility and skipped checks explicitly; do not label proposed support tested.
