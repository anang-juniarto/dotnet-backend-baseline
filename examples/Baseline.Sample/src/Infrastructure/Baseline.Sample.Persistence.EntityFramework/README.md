# Optional EF Core Items adapter

> Status: Draft
> Classification: `[PROFILE]`
> Verification scope: compilation/model checks only; no live relational certification

`Baseline.Sample.Persistence.EntityFramework` contains the common Items mapping and actual `IItemStore` adapter. SQL Server and PostgreSQL projects supply separate concrete contexts and registrations. They are optional; the default host remains in-memory unless explicitly wired by its composition root.

## Mapping ownership

```text
Core/Baseline.Sample.Domain/Items/Item.cs       # the sole business entity
Persistence.EntityFramework/
  Context/ApplicationDbContext.cs              # registration only
  Configurations/ItemConfiguration.cs          # maps Domain Item directly
  Repositories/EntityFrameworkItemStore.cs
Persistence.SqlServer/
  Configurations/SqlServerItemConfiguration.cs
Persistence.PostgreSql/
  Configurations/PostgreSqlItemConfiguration.cs
```

Each mapping implements `IEntityTypeConfiguration<T>`. Contexts call `ApplyConfigurationsFromAssembly`; entity rules do not live inline in `OnModelCreating`. The base context scans `typeof(ApplicationDbContext).Assembly` for shared mappings. Provider contexts first call `base.OnModelCreating`, then scan their own context assembly for provider overrides. This establishes shared-before-provider ordering across assemblies. Ordering within each assembly scan is undefined: keep mappings independent within an assembly, and do not rely on class naming or discovery order to resolve conflicts. Scanned configurations must have a parameterless constructor. Keep EF mapping classes out of Domain/Application. All business entities live in Domain; no duplicate persistence entity is retained. Domain may also contain needed value objects, events and exceptions. Transport DTOs/protobuf contracts and search projections remain in their owning Application/Presentation/Infrastructure layer; they are not automatically Domain entities. `Item` stays immutable and EF-free: explicitly mapped getter-only properties bind to its existing invariant-enforcing constructor. Repository inserts/reads Domain Item directly. Table/key/column rules remain unchanged; changing the CLR entity type can affect future snapshots, so review generated migration diffs before any deployment. No migration was generated or applied.

The shared marker is deliberately [ApplicationDbContext](Context/ApplicationDbContext.cs), not `GetType().Assembly`: a provider-derived runtime type would scan the provider assembly instead of shared configurations. See provider overrides in [SQL Server](../Baseline.Sample.Persistence.SqlServer/SqlServerDbContext.cs) and [PostgreSQL](../Baseline.Sample.Persistence.PostgreSql/PostgreSqlDbContext.cs). Reflection-based discovery is not Native AOT/trimming certification; if that capability is selected later, verify preservation/discovery explicitly.

## Provider selection

Select exactly one relational provider registration after application services:

```csharp
services.AddSqlServerItemStore(connectionString);
```

Or select PostgreSQL instead:

```csharp
services.AddPostgreSqlItemStore(connectionString);
```

The registration replaces `IItemStore` with a scoped adapter. Resolve it within a DI scope; do not capture it in singletons. Obtain connection strings from approved secret configuration, not source code. Registration does not connect, create databases, apply migrations, or enable automatic retries. A failed or canceled save may have an unknown commit outcome; do not blindly retry creates or claim rollback.

Reads are untracked, include both item ID and owner in the database predicate, and check ordinal owner equality before returning a domain object. SQL Server uses `Latin1_General_100_BIN2` and PostgreSQL uses `C` for the owner column. The application supplies owner identity from the authenticated principal, never from client-controlled request ownership. Cancellation is propagated to queries and saves and checked before tracking a new insert.

Provider contexts configure migrations in their **own provider assembly**. The [sample README](../../../README.md#sqlserver-and-postgresql--compileconfiguration-only) owns matching build/run selections; these relational profiles are compile/configuration-only, not end-to-end runnable database demos. No migration files are supplied or schema changes applied. Generate each provider's migrations later using its concrete context and explicitly selected safe design-time configuration, review generated SQL, and execute deployment through the repository schema-change process. Never mix provider migration histories or run `EnsureCreated`/`Migrate` automatically at host startup.

Package evidence inspected from NuGet on 2026-10-10:
- `Microsoft.EntityFrameworkCore.Relational` **10.0.9**.
- `Microsoft.EntityFrameworkCore.SqlServer` **10.0.9**, requiring Relational **10.0.9**.
- `Npgsql.EntityFrameworkCore.PostgreSQL` **10.0.3**, requiring EF Core/Relational **[10.0.4, 11.0.0)**, compatible with **10.0.9**.

Compile/restore checks do not certify database execution, collation behavior on a deployed server, migrations, or transaction-failure recovery. These require separately authorized isolated provider integration tests. The sibling MySql directory records the canonical Pomelo compatibility deferral.
