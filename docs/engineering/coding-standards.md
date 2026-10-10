# .NET Backend Coding Standards & Clean Engineering Practices

> **Document Metadata**:  
> `Status: Draft` | `Owner: Engineering Leads` | `Last verified: Not verified` | `Evidence: Target coding guidelines`

This is reference guidance, not evidence that this repository implements a production service or any vendor adapter. The isolated [Baseline.Sample README](../../examples/Baseline.Sample/README.md) owns its executable instructions and current contracts; a consuming target owns its actual manifests, configuration, routes, and verification evidence.

For approved new projects, the default is .NET 10 (`net10.0`) Clean Architecture with `src/Core/` Domain and Application assemblies, MediatR CQRS feature slices, provider/capability assemblies under `src/Infrastructure/`, and REST controllers in `src/Presentation/` WebApi (optional Grpc host). Organize tests under `tests/<Project>.UnitTests/`, `tests/<Project>.IntegrationTests/`, and `tests/<Project>.ArchitectureTests/`. MediatR is explicit in this greenfield profile; event sourcing, separate databases, custom dispatchers, speculative ports, wrappers, events, and repositories are not required.

For existing projects, reference-only adoption preserves the verified runtime, architecture, mediator, transport, and published contracts. Capability additions and architecture/runtime migrations require their own approved scope. Follow the permanent capability fallback in [`AGENTS.md`](../../AGENTS.md#2-task-conditional-discovery--context-efficiency); missing profiles do not authorize restructuring. Scale specifications and specialist review to concrete risk per [`change-delivery-contract.md`](./change-delivery-contract.md), never by concern count alone.

Use the selected SDK's compiler defaults, nullable reference types, and implicit usings for new projects; avoid floating `LangVersion=latest`. Preserve compatible brownfield analyzer policies rather than making every legacy warning an adoption blocker.

Synchronize affected maintained documentation in the same change and add concise XML documentation to touched handwritten classes, actions/named endpoint handlers, properties, and fields, with the exclusions and coverage checks in [`csharp-and-documentation.md`](../standards/csharp-and-documentation.md#14-c-and-documentation-standards). Security, permission, compatibility, and transaction safeguards remain mandatory.

---

## 1. Naming & Language Conventions

- **Members & Types**: `PascalCase` for classes, records, structs, interfaces (prefixed with `I`), methods, and properties.
- **Locals & Parameters**: `camelCase` for method arguments and local variables.
- **Fields**: `_camelCase` for private instance fields.
- **Typing**: Use `var` when the type is apparent from assignment; use explicit types when clarity is enhanced.
- **Null Safety**: Prefer `is null`, `is not null`, pattern matching, and `string.IsNullOrWhiteSpace()`.

---

## 2. Asynchronous Programming & Cancellation

1. **Async End-to-End**: Async methods SHOULD end with the `Async` suffix (except framework-mandated methods like MediatR `Handle`).
2. **Propagate `CancellationToken`**: Every I/O-bound method (`HttpClient`, database queries, file access) MUST accept and propagate a `CancellationToken`.
3. **No Async Blocking**: NEVER block asynchronous work using `.Result`, `.Wait()`, or `.GetAwaiter().GetResult()`. This causes thread pool starvation and deadlocks.
4. **Lifecycle-Aware Cancellation**: Distinguish request cancellation from background worker cancellation. Never let request cancellation abort a confirmed transactional commit or durable recovery registration.

---

## 3. Dependency Injection & Resource Ownership

1. **Lifetime Rules**:
   - `Transient`: Lightweight, stateless utilities or algorithms.
   - `Scoped`: Request-bound services, `DbContext`, unit-of-work abstractions.
   - `Singleton`: Thread-safe, long-lived components (telemetry meters, cache providers).
2. **Captive Dependency Prevention**: Singletons MUST NOT capture scoped dependencies or `DbContext`.
3. **Thread Safety**: `DbContext` is NOT thread-safe. Never invoke multiple concurrent operations on a single context instance.
4. **Ownership & Disposal**: Explicitly dispose resources instantiated directly (streams, connections, transactions). Do NOT manually dispose container-injected services.

---

## 4. Validation, Errors & Invariants

1. **Deterministic Input Validation**: Validate incoming contracts (DTOs) for syntax, boundaries, ranges, and required fields before domain execution.
2. **Business Invariant Enforcement**: Invariants depending on persistent state (balances, stock, uniqueness) MUST be enforced at domain/persistence boundaries, not duplicated in static validators.
3. **Zero Swallowed Exceptions**: Never catch exceptions without logging, rethrowing with `throw;`, or returning an explicit domain error contract.
4. **Error Masking**: Clean architecture handlers and controllers MUST NOT expose database queries, stack traces, or internal server errors to clients.

---

## 5. Persistence, SQL & Concurrency

1. **Parameterization**: ALL queries MUST use parameterized SQL or ORM expressions. Never concatenate untrusted strings.
2. **Efficient Projections**: Read-only queries SHOULD project only required columns and use `.AsNoTracking()` where supported.
3. **Bounded Queries**: Avoid unbounded N+1 query patterns. Enforce pagination limits on all collection endpoints.
4. **Concurrency & Repeat Execution**:
   - Protect concurrent state with unique constraints, optimistic row versions, or bounded pessimistic locks.
   - Require durable idempotency keys and transactional deduplication only when retries or duplicate delivery can cause unacceptable repeated effects. Document key scope, retention, payload mismatch, concurrent duplicate behavior, and replay result where selected; do not impose storage on every read or CRUD operation.
   - Distinguish confirmed commit, confirmed rollback, and unknown outcome. Do not blindly retry a non-idempotent operation after an ambiguous commit.

---

## 6. Selected Architecture & Optional Adapters

- Each selected Presentation host is a composition root: it references selected Infrastructure assemblies for registration, while controllers dispatch Application requests through MediatR, not persistence/vendor adapters. Handlers return application results, not `IActionResult`, protobuf messages, or vendor SDK types.
- Commands own state transitions and an explicit transaction boundary when persistence is selected. Queries are read-only, bounded projections; they need no separate read store. Propagate cancellation and map transport contracts at the edge.
- Core Domain has no transport/persistence/vendor dependencies; Core Application depends on Domain and owns feature slices and needed use-case ports. Selected Infrastructure provider/capability assemblies implement adapters; do not collapse them into one universal Infrastructure project. Preserve verified brownfield boundaries unless migration is approved.
- All [optional capabilities](../architecture/optional-stack-catalog.md) default off: SQL Server/MySQL/PostgreSQL, MongoDB, Elasticsearch, Redis, RabbitMQ, gRPC, Hangfire, OpenTelemetry, Seq, Sentry, SignalR, chat, in-app notifications, web/mobile push. Omitted capabilities add no packages, required settings, services, containers, probes, or network calls. Verify exact package/server compatibility and licensing before implementation; no vendor SDK is certified by this guide or the memory-only sample.
- Choose authoritative storage explicitly. Search is a derived projection, cache is not a system of record, and required asynchronous publication needs recoverable durable intent (such as a transactional outbox where supported), not an assumed cross-store transaction. Jobs/consumers may repeat; select duplicate-effect protection by invariant.
- SignalR is transport, not durable history. Chat needs authentication and authoritative storage; persistent notification inboxes need their own recipient authorization and storage. Inbox, SignalR delivery, and external push are independent selections; none implicitly requires Firebase, Redis, RabbitMQ, or Hangfire. Push adapters do not own unread state or guarantee device delivery.
- Define one capture/export owner per telemetry signal, bounded buffering/labels, redaction, and outage behavior; do not duplicate exports or make noncritical telemetry/cache failures process-liveness failures.
