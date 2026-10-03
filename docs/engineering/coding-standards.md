# .NET Backend Coding Standards & Clean Engineering Practices

> **Document Metadata**:  
> `Status: Draft` | `Owner: Engineering Leads` | `Last verified: Not verified` | `Evidence: Target coding guidelines`

This document defines implementation standards for writing clean, resilient, and maintainable C# code in verified .NET backend services. Follow the permanent capability fallback in [`AGENTS.md`](../../AGENTS.md#2-task-conditional-discovery--context-efficiency); examples and templates do not select an architecture.

Prefer the simplest correct, secure, readable, and compatible implementation. Do not introduce CQRS, MediatR, interfaces, extra repositories, or layers for simple CRUD without a verified architectural need. Avoid speculative abstractions and unnecessary boilerplate. Scale specifications and specialist review to concrete risk per [`change-delivery-contract.md`](./change-delivery-contract.md), never by concern count alone.

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
   - Protect repeat requests with durable idempotency keys and transactional deduplication.
