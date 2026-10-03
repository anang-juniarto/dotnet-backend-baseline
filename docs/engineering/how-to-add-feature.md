# Feature Implementation Playbook

> **Document Metadata**:  
> `Status: Draft` | `Owner: Engineering Leads` | `Last verified: Not verified` | `Evidence: Standard feature workflow`

This playbook guides feature changes using the simplest correct, secure, readable, and compatible design within verified repository boundaries.

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

---

## 2. Phase 2: Implementation Sequence

Apply only relevant steps in the verified architecture. Simple CRUD does not automatically require CQRS, MediatR, interfaces, extra repositories, or new layers. Process flexibility never relaxes security, permissions, compatibility, or transaction integrity.

1. **Business Logic**:
   - Implement invariants and state transitions at existing boundaries. Use Domain entities/value objects only where selected and needed.
2. **Use-Case Implementation**:
   - Define only needed contracts and input validation. Use commands/queries, handlers, and repository ports when the verified architecture requires them, not as mandatory scaffolding.
3. **Persistence & Integrations (when affected)**:
   - Implement queries, mappings, schema changes, or clients under verified technology and schema authority. Migrations/external calls retain explicit authorization gates.
   - Use the selected reliable-publication pattern, such as a transactional outbox, when integration events must be published after commit.
4. **Presentation / API (when affected)**:
   - Expose the endpoint in the existing Controller or Minimal API structure.
   - Apply required authorization/tenant checks and existing versioning conventions; preserve published response contracts.
   - Forward `CancellationToken` through asynchronous I/O.

---

## 3. Phase 3: Testing & Verification

1. **Unit Tests**: Cover affected invariants and business logic; test handlers/ports only where they exist.
2. **Integration Tests**: When persistence is affected, verify transactions, concurrency, and failure behavior using authorized isolated test targets.
3. **Contract Tests**: When API contracts are affected, verify HTTP status codes, headers, authorization, and serialization.
4. **Verification Commands**: Discover existing build, lint/typecheck, and test commands from verified manifests and repository guidance before execution. `dotnet test` applies only to an actual .NET test project; do not install tooling or access external services without authorization.
5. **Risk-Based Review**: Inspect the diff and use targeted independent/specialist review when justified by risk, ambiguity, or coordination. Preserve required security/repository gates.

---

## 4. Phase 4: Documentation & Handoff

1. **Sync Maintained Docs**: Without a separate reminder, update affected repository-owned documentation in the same change per [`csharp-and-documentation.md`](../standards/csharp-and-documentation.md#145-incremental-adoption-and-documentation-validation); preserve existing locations and generated/vendor ownership.
2. **Check Touched XML Docs**: Inspect touched handwritten classes, actions/named endpoint handlers, properties, and fields per the [canonical XML documentation rules](../standards/csharp-and-documentation.md#14-c-and-documentation-standards), including exclusions. Public compiler warnings alone do not prove coverage.
3. **Log Unreleased Changes**: Update the existing release record when applicable; this baseline uses `[Unreleased]` in [`CHANGELOG.md`](../../CHANGELOG.md). Do not create unrelated scaffolding.
4. **Truthful Handoff**: State outcome, executed checks and exact results, unexecuted checks with reasons, documentation impact or blockers, and residual risks.
