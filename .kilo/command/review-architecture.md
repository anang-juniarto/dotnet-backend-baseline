---
description: Review architectural boundaries, ADR compliance, and structural integrity
agent: architect-reviewer
subtask: true
---
Review the architecture or structural changes for: $ARGUMENTS

Workflow:
1. Inspect proposed changes against `docs/architecture/repository-map.md`.
2. Verify repository-selected dependency boundaries; check inward dependency flow and layer coupling only where the verified architecture requires them.
3. If this introduces an architectural decision, verify compliance with `docs/adr/template.md`.
4. Check contract compatibility per `docs/api/compatibility-and-versioning.md`.
5. Output structured verdict: APPROVED / CHANGES REQUESTED with trade-offs.
