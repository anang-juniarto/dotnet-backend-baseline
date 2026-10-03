---
description: Implement a backend feature through coordinated specialist workflow
agent: team-lead
subtask: true
---
Plan and coordinate the implementation of: $ARGUMENTS

Workflow:
1. Discover verified capabilities using the permanent fallback in `AGENTS.md`; choose the simplest compatible design. Use an inline scope, invariants, acceptance criteria, and test strategy for straightforward work, even across concerns. A separate specification or specialist review requires concrete risk, ambiguity, or coordination needs per `docs/engineering/change-delivery-contract.md`.
2. Follow applicable steps in `docs/engineering/how-to-add-feature.md` within the verified architecture. Do not impose CQRS, MediatR, handlers, interfaces, extra repositories, or new layers for simple CRUD. Preserve security, permission, compatibility, schema-authority, and transaction gates.
3. Implement directly where straightforward; delegate to `backend-feature`, `database-engineer`, or `test-engineer` only where specialization is needed. Verify affected acceptance criteria and failure paths with existing authorized tooling.
4. Inspect the diff; use `code-reviewer` or `architect-reviewer` when concrete risk warrants independent review, preserving required repository/security gates.
5. Before completion, synchronize affected maintained documentation and inspect touched C# XML docs per `docs/standards/csharp-and-documentation.md`, including generated/vendor and untouched-legacy exclusions. Report outcome, exact executed checks, unexecuted checks with reasons, documentation impact/blockers, and residual risks.
