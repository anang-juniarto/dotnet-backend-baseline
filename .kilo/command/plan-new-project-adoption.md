---
description: Analyze a new project target and generate an AI adoption plan
agent: team-lead
subtask: true
---
Analyze the new project repository and produce an AI adoption plan for: $ARGUMENTS

## Objective
Perform a read-only assessment of the target directory and generate a structured adoption plan using `docs/adoption/adoption-plan-template.md`.

## Source-Only Lifecycle & Scope
Follow `docs/adoption/ai-implementation-workflow.md` and scopes in `BASELINE.md`. In both scopes, exclude all `docs/adoption/` files and `.kilo/command/plan-new-project-adoption.md`, `.kilo/command/plan-existing-project-adoption.md`, and `.kilo/command/implement-approved-adoption.md` from destination copying; retain them in source. Enumerate exact permanent destination paths and README/index/manifest adaptations so no permanent route requires missing adoption files; permanent fallback is `AGENTS.md` section 2. Record approved-effective configuration, preserving existing `default_agent` and agent collision renames. For any existing scaffolded files, classify collisions and preserve user content; legacy cleanup requires an exact per-file approval list with ownership and content/drift/customization evidence. Plan permanent verification → approved cleanup with immediate rechecks → final link/discovery verification; failure or uncertain ownership means retain/report conflict. No destination supplied means no cleanup. Planning never deletes files.

## Exact-File Safety Contract
Apply canonical exclusions and root-document/license rules in `BASELINE.md` to source and mapped destination paths; renames cannot bypass exclusions. Inventory excluded/deferred candidates, including `.git` files/directories at any depth and `.kilo/plans/`. Exclusion is not target deletion; later separately authorized target generation is not import. Read LICENSE and record an exact attribution destination retaining its full MIT notice without replacing the target root license; unresolved attribution is `CONFLICT`/`DEFER`. Preserve README identity, CHANGELOG history and target-compatible CONTRIBUTING.

Normalize source/target roots, reject equality and ancestor/descendant overlap, verify aliases/junctions where possible and block uncertain paths. Do not automatically follow links/reparse points. Check containment/traversal and duplicate mapped destinations. Record each exact file's need, scope basis, source/target state and action; unlisted files cannot transfer. No bulk recursive copy/mirror. Include dependency closure and selected-tree link/anchor checks, pre-write drift/exclusion rechecks and post-write actual-change-to-manifest audit; scope changes or new conflicts require renewed approval. No automatic rollback or destructive cleanup on failure.

## Workflow
1. **Target Verification (Read-Only)**:
   - Verify the target repository root path is an absolute path.
   - Confirm target is not an active deployment environment or this baseline itself.
   - Inspect target directory contents: empty, newly created, or partially scaffolded.
2. **Determine Adoption Scope**:
   - Default: `core-reference` for new and existing projects.
   - Alternative: `full-reference` only when explicitly requested; still an exact-file allowlist, not a folder copy.
3. **Generate Adoption Plan**:
   - Produce a structured plan following `docs/adoption/adoption-plan-template.md`.
   - List every file to be created (`ADD`) or populated (`docs/repository-profile.md` as `Draft`).
   - Note that deferred directories (`docs/api/endpoints/`, `docs/assets/diagrams/`) are intentionally excluded.
   - List possible post-adoption solution/project creation (`dotnet new`) separately, requiring its own authorization; no implicit scaffolding or sample ADR/spec/release instances.
4. **Halt Without Mutation**:
   - Output the complete adoption plan for human review.
   - **DO NOT modify or create any files in the target directory.** Instruct the user to review the plan and authorize implementation with `/implement-approved-adoption`.
