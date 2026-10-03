---
description: Implement an aligned change spanning feature, API, database, config, and tests
agent: team-lead
subtask: true
---
Coordinate the aligned end-to-end implementation of: $ARGUMENTS

## Objective
Implement only affected concerns while preserving contract and architectural coherence per `docs/engineering/change-delivery-contract.md` and the simple-first safeguards in `AGENTS.md`.

## Workflow
1. **Change Scope & Contract Assessment**:
   - Establish verified capabilities using the permanent fallback in `AGENTS.md` (profile -> manifests/code -> applicable universal standards).
   - For straightforward changes, use an inline scope, invariants, acceptance criteria, and test strategy, including cross-concern changes.
   - Use a separate specification or `system-analyst` only when concrete risk, ambiguity, or coordination justifies it; two concerns alone do not require a spec. Resolve consequential decisions before implementing.

2. **Proportionate Implementation**:
   - Handle straightforward work directly; delegate only where specialization is needed. Apply only relevant steps in the verified architecture.
   - **Schema & Persistence**: Preserve schema authority, compatibility, transaction/concurrency integrity, and migration authorization; use `database-engineer` when specialist work is warranted.
   - **Business Logic & API**: Use the existing minimal structure; do not add CQRS, MediatR, interfaces, repositories, or layers solely for CRUD. Use `backend-feature` when needed. Preserve authorization, tenant isolation, published contracts, and CancellationToken propagation.
   - **Configuration**: Follow verified conventions without hardcoded secrets.
   - **Tests**: Cover affected acceptance criteria and failure paths using existing tooling; use `test-engineer` when needed.

3. **Quality Gates & Review**:
   - Inspect the diff against applicable `docs/standards/anti-patterns.md` rules.
   - Use `code-reviewer` or `architect-reviewer` for concrete risk, ambiguity, or coordination needs, not automatically. Do not bypass required security/repository gates or execution permissions.

4. **Coherence Verification**:
   - Check applicable items in `docs/engineering/change-delivery-contract.md#3-coherence--consistency-checklist`.
   - Synchronize affected maintained documentation and inspect touched C# XML coverage per `docs/standards/csharp-and-documentation.md`; honor generated/vendor and untouched-legacy exclusions.
   - Discover and run authorized verification commands for the actual repository; do not assume an executable .NET project or install tooling.
   - Report outcome, exact checks/results, unexecuted checks with reasons, documentation impact/blockers, and residual risks.
