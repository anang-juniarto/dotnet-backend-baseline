---
description: Analyze structural dependencies and cycles using Graphify summary or manifest fallback
agent: architect-reviewer
subtask: true
---
Analyze the structural dependencies and modular coupling for: $ARGUMENTS

## Workflow
1. Check if `.kilo/cache/graphify/summary.json` exists:
   - If available: Read summary metadata (node counts, detected cycles, high-coupling hotspots).
   - If absent, stale, or marked `unavailable`: Fall back to discovering dependencies directly from `.sln`/`.slnx` and `<ProjectReference>` tags in `.csproj` files.
2. Focus structural evaluation on:
   - Layer boundary leaks (e.g. Domain referencing Infrastructure or Web).
   - Circular dependency cycles across modules.
   - High fan-in or fan-out coupling risks.
3. Token-Efficiency Constraint:
   - Do NOT load raw `graph.json` or dump full graph outputs into prompt context.
   - Inspect only the specific module neighborhood under evaluation.
4. Output structured architectural assessment:
   - **Verdict**: [APPROVED / STRUCTURAL RISK DETECTED]
   - **Coupling & Cycle Summary**: Observed dependency flow.
   - **Remediation**: Recommended architectural adjustments.
