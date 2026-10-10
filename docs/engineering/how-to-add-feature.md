# Feature Implementation Playbook

> **Document Metadata**:  
> `Status: Draft` | `Owner: Engineering Leads` | `Last verified: Not verified` | `Evidence: Standard feature workflow`

This reference playbook guides feature changes using the simplest correct, secure, readable, and compatible design. Approved new targets default to .NET 10 Core Domain/Application with MediatR CQRS feature slices, modular Infrastructure provider/capability assemblies, and Presentation WebApi controllers (optional Grpc); existing targets preserve verified boundaries unless migration is explicitly approved. The isolated [Baseline.Sample](../../examples/Baseline.Sample/README.md) demonstrates core boundaries with volatile memory and gated Development auth, not production persistence or certified optional integrations.

---

## 1. Phase 1: Discovery & Specification

1. **Clarify Acceptance Criteria & Change Contract**: Follow [`change-delivery-contract.md`](./change-delivery-contract.md). A concise inline scope, invariants, acceptance criteria, and test strategy suffice for straightforward changes, even across multiple concerns. Use a separate specification or specialist review only when concrete risk, ambiguity, or coordination warrants it; resolve consequential decisions before implementation.
2. **Discover Architecture Profile**: Follow the permanent fallback in [`AGENTS.md`](../../AGENTS.md#2-task-conditional-discovery--context-efficiency): verified profile documents, then relevant manifests/code, then applicable universal standards. Templates are not implementation evidence; missing optional profiles do not authorize new layers or tooling.
3. **Analyze Impact Boundaries**:
   - *Public Contract*: Does this introduce a new route or modify existing responses?
   - *Persistence*: Does this require table migrations, new columns, or indexes?
   - *Security*: Who is authorized to execute this? What tenant scope applies?
   - *Concurrency*: Can multiple clients execute this simultaneously? Is durable idempotency required?
   - *Documentation*: Which maintained API, configuration, operations, schema, or developer guidance is affected?
   - *Capabilities*: Record selected/omitted modules using the [optional stack catalog](../architecture/optional-stack-catalog.md), their dependencies, security, recovery, licensing, and exact package/server verification. No datastore, cache, worker, telemetry exporter, or push provider is implicit.
   - *Evidence Boundary*: Separate reference design, isolated sample behavior, and consuming-target facts. Sample success does not certify external integrations or authorize target generation.

---

## 2. Phase 2: Implementation Sequence

Apply only relevant steps. Approved new targets use `src/Core/` Domain/Application with MediatR feature slices, selected provider/capability projects in `src/Infrastructure/`, and hosts in `src/Presentation/`; repository wrappers and speculative abstractions remain unnecessary. Brownfield CRUD/service boundaries and Minimal APIs remain valid; reference adoption alone does not migrate them. Process flexibility never relaxes security, permissions, compatibility, or transaction integrity.

1. **Business Logic**:
   - Implement invariants and state transitions at existing boundaries. Use Domain entities/value objects only where selected and needed.
2. **Use-Case Implementation**:
   - In new targets, organize MediatR commands/queries, handlers, validators and DTOs under Core Application `Features/<Feature>/Commands/<UseCase>/` or `Queries/<UseCase>/`, with event handlers only when needed. Keep queries read-only and commands responsible for invariants and explicit transaction ownership. Add only ports needed to separate actual adapters; avoid a custom mediator/pipeline framework.
   - In existing targets, use commands/queries and handlers only where selected. Keep transport and vendor types outside business contracts.
3. **Persistence & Integrations (when affected)**:
   - Implement queries, mappings, schema changes, or clients under verified technology and schema authority. Migrations/external calls retain explicit authorization gates.
   - Use the selected reliable-publication pattern, such as a transactional outbox, when integration events must be published after commit. Protect duplicate effects only when required by the invariant; classify ambiguous outcomes before retrying.
   - Add only selected adapters: relational/document storage, search, cache, messaging/jobs, gRPC, telemetry, real-time/chat/inbox/push. Omitted modules contribute no dependencies, required settings, services, or probes.
   - Chat requires authenticated conversation membership, authoritative history and authorized paginated recovery; SignalR alone is not durable storage. Persistent inbox, real-time delivery, and external web/mobile push are independent choices. Verify recipient isolation, unread updates, reconnect recovery, token lifecycle and provider delivery limits only for selected features.
4. **Presentation / API (when affected)**:
   - New targets expose Presentation WebApi REST controllers dispatching Application requests through MediatR; each selected host owns composition-root registration of modular Infrastructure, not controller access to adapters. Optional Grpc maps contracts through the same Application boundary. Preserve existing Controller or Minimal API structure in brownfield targets.
   - Separate HTTP request DTOs from Application commands when the command carries server-trusted context (such as authenticated actor/owner identity, tenant ID, or privileged state) that client payloads must not supply or override. Organize request DTOs under Presentation `Contracts/<Feature>/`. Direct command binding is acceptable only when all command properties are safely client-supplied and unprivileged.
   - Apply required authorization/tenant checks and existing versioning conventions; preserve published response contracts.
   - Forward `CancellationToken` through asynchronous I/O.

---

## 3. Phase 3: Testing & Verification

1. **Unit Tests**: Cover affected invariants and business logic; test handlers/ports only where they exist.
2. **Integration Tests**: When persistence is affected, verify transactions, concurrency, and failure behavior using authorized isolated test targets.
3. **Architecture & Contract Tests**: New targets separate `tests/<Project>.UnitTests/`, `tests/<Project>.IntegrationTests/`, and `tests/<Project>.ArchitectureTests/`; verify inward references, provider/capability isolation and host-only adapter wiring. When API contracts are affected, verify HTTP status codes, headers, authorization, and serialization.
4. **Verification Commands**: Discover existing build, lint/typecheck, and test commands from verified manifests and repository guidance before execution. Verify the actual framework, runner (VSTest or Microsoft.Testing.Platform), SDK selection and runner-specific arguments before publishing or executing test commands; see the [testing guide](./testing-guide.md). Do not install tooling or access external services without authorization.
5. **Risk-Based Review**: Inspect the diff and use targeted independent/specialist review when justified by risk, ambiguity, or coordination. Preserve required security/repository gates.

---

## 4. Phase 4: Documentation & Handoff

1. **Sync Maintained Docs**: Without a separate reminder, update affected repository-owned documentation in the same change per [`csharp-and-documentation.md`](../standards/csharp-and-documentation.md#145-incremental-adoption-and-documentation-validation); preserve existing locations and generated/vendor ownership.
2. **Check Touched XML Docs**: Inspect touched handwritten classes, actions/named endpoint handlers, properties, and fields per the [canonical XML documentation rules](../standards/csharp-and-documentation.md#14-c-and-documentation-standards), including exclusions. Public compiler warnings alone do not prove coverage.
3. **Log Unreleased Changes**: Update the existing release record when applicable; this baseline uses `[Unreleased]` in [`CHANGELOG.md`](../../CHANGELOG.md). Do not create unrelated scaffolding.
4. **Truthful Handoff**: State outcome, executed checks and exact results, unexecuted checks with reasons, documentation impact or blockers, and residual risks.
