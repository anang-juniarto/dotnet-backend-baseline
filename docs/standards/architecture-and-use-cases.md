# Architecture, Clean Dependency Boundaries, and Use-Case Standards

> **Classification:** `[CORE STANDARD]`  
> **Status:** Normative Standard  
> **Applicability:** Active when Clean Architecture, CQRS, or DDD feature profiles are selected by repository configuration, manifests, or an accepted ADR.  
> **Source Migration:** Formed from Section 5 and Sections 6.1–6.2 of the Enterprise .NET Backend AI Engineering Guide. (Section 6.3 is located in [`persistence-and-concurrency.md`](./persistence-and-concurrency.md#63-command-lifecycle)).

---

## 5. Clean Architecture and Dependency Boundaries

**Activation:** Sections 5.1–5.3 define the Clean Architecture profile only where repository evidence or an accepted scoped architecture decision selects it. They do not mandate new layers, projects, repository abstractions, or tactical DDD for every backend. A simpler safe CRUD design may use existing endpoint/service/persistence boundaries. Preserve authorization, invariants, compatibility, testability, and resource safety regardless of architecture; do not migrate a legacy fix merely to activate this profile. Section 5.4 applies wherever its runtime/resource conditions exist.

### 5.1 Dependency Topology

```text
Presentation ──────> Application ──────> Domain
Infrastructure ────> Application ──────> Domain
Composition Root wires Presentation and Infrastructure
```

Presentation and Infrastructure are outer adapters. Presentation does not architecturally depend on Infrastructure except through an explicitly documented bootstrap/composition project.

### 5.2 Responsibilities

| Layer | Permitted in This Profile | Prohibited in This Profile ([core-governance.md Section 3.3](./core-governance.md#33-formal-waiver-policy) Applies) |
|---|---|---|
| Domain | Entities, value objects, invariants, domain services/events/errors | HTTP, ORM, SQL, DI, cache, broker, vendor SDK concerns |
| Application | Use cases, commands/queries, validators, DTOs, persistence-neutral ports, specifications, query objects, projections, and policies | HTTP envelopes, controller results, concrete infrastructure, SQL execution, `DbContext`, ORM/provider types |
| Infrastructure/Persistence | Port implementations, SQL/ORM execution, provider-specific optimization, cache, broker, external clients, migrations | Business invariants, transport response mapping, use-case orchestration |
| Presentation | Binding, authentication context, authorization policy invocation, dispatch, HTTP mapping, composition | Direct persistence, transaction control, domain mutation, business calculations |

### 5.3 Domain Standards

- Domain state MUST preserve invariants regardless of caller.
- New aggregate behavior SHOULD use intention-revealing methods rather than unrestricted public setters.
- ORM constructors MAY be private or protected when supported.
- A mandatory base `Entity` or `AggregateRoot` is a repository profile choice, not a universal requirement.
- Domain events represent facts observed within a domain transaction and MAY be raised before persistence commits; they are not externally committed facts or transport messages.
- Integration events represent facts safe for external consumers and MUST be released only after successful commit or through a transactional outbox. Rolled-back transactions MUST NOT publish integration events.

### 5.4 Dependency Injection and Resource Ownership

Where DI and managed resources apply:
- Match lifetimes to actual ownership. Singletons MUST NOT capture scoped services, request state, or a scoped `DbContext`; create bounded scopes using verified container APIs when a worker needs scoped dependencies.
- Treat `DbContext` and other non-thread-safe scoped resources as non-concurrent. Await operations before reuse; parallel work needs independently owned scopes/contexts and explicit transaction semantics.
- Dispose resources the code creates and owns, including asynchronous disposal where supported. Do not dispose container-owned injected services; follow verified ownership rules for factories, streams, connections, transactions, and clients.
- Do not retain `HttpContext`, request streams, scoped services, or mutable request objects beyond their lifetime. Durable jobs carry validated minimal data and establish their own scope, authorization context, and resources.

---

## 6. DDD, CQRS, Feature Profiles, and Handler Lifecycle

**Activation:** Sections 6.1–6.2 apply to selected request/validator/handler or DDD/CQRS profiles, not every use case. CQRS does not require a mediator, separate read/write databases, event sourcing, or extra projects. Do not add MediatR or tactical DDD just to comply. Existing simpler CRUD/service designs remain valid when they preserve the universal safety and compatibility properties. [Section 6.3's mutation and commit protections](./persistence-and-concurrency.md#63-command-lifecycle) apply independently of handler architecture; adapt orchestration names to the verified design.

### 6.1 Feature Organization Profiles

Within a selected request/handler profile, these are common organization options; follow verified repository conventions:

1. **Single-file cohesive slice**: request, nested validator, and nested handler in one file.
2. **Multi-file vertical slice**: request, validator, handler, DTO, and result separated under one use-case directory.

A bounded context/module SHOULD follow its verified repository convention. Mixed organization MAY be appropriate for generated code, visibility boundaries, complexity, or incremental migration. A repository-wide or cross-module reorganization requires explicit scope and the repository's architecture decision process; MUST NOT reorganize unrelated code merely for uniformity. Keep each changed use case coherent.

### 6.2 Requests, Validators, and Handlers

- Requests contain only use-case input and MUST NOT inherit persistence entities.
- Contract validation is deterministic and I/O-free unless an explicitly selected validation architecture says otherwise.
- Existence, authorization state, uniqueness under concurrency, and business invariants belong in the use case/domain, not duplicated validator queries.
- New clean handlers return primitives, clean DTOs, a void-equivalent result such as `Unit` when supported by the selected mediator, or another transport-neutral result type.
- Existing handler envelopes remain until an approved migration; adapters SHOULD prevent new transport coupling.
- Handler-to-handler dispatch requires explicit transaction ownership, authorization/pipeline equivalence, recursion prevention, and failure semantics. Cyclic dispatch and substituting nested in-process dispatch for required durable work are FORBIDDEN. Prefer reusable domain/application services when composition would otherwise obscure these properties.

*(For Command Lifecycle details including write boundaries, transactional commits, and explicit outcomes, see [persistence-and-concurrency.md Section 6.3](./persistence-and-concurrency.md#63-command-lifecycle)).*
