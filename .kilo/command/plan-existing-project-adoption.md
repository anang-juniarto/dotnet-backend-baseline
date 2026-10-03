---
description: Analyze an existing project target and generate a collision-free AI adoption plan
agent: team-lead
subtask: true
---
Analyze the existing project repository and produce a collision-free AI adoption plan for: $ARGUMENTS

## Objective
Perform a read-only assessment of an active repository and generate a structured adoption plan using `docs/adoption/adoption-plan-template.md` without modifying any target files.

## Source-Only Lifecycle & Scope
Follow `docs/adoption/ai-implementation-workflow.md` and `BASELINE.md`; default to `core-reference`, or `full-reference` only when requested. Both exclude `docs/adoption/` and `.kilo/command/plan-new-project-adoption.md`, `.kilo/command/plan-existing-project-adoption.md`, and `.kilo/command/implement-approved-adoption.md` from copying; retain source assets. Enumerate exact permanent destination paths, adapting README/index/manifest and permanent references to avoid missing local adoption links. Permanent fallback is `AGENTS.md` section 2. Record approved-effective settings, preserving existing `default_agent` and approved collision renames. List legacy cleanup separately by exact file path, with explicit approval decisions, baseline provenance/version, content comparison/state, customization/worktree findings, and reference adaptations. Names alone do not prove ownership. Plan permanent verification → approved cleanup after immediate ownership/drift rechecks → final link/discovery verification. Preserve user-owned/customized/uncertain files and report conflicts; never plan recursive deletion of unverified folders. No destination means no cleanup; planning is read-only.

## Workflow
1. **Target Verification & Pre-Flight (Read-Only)**:
   - Verify the target repository root path is an absolute path.
   - Inspect target Git status read-only if Git exists (`git status`). Never branch, stash, reset, or commit.
2. **Staged Inventory (Token Efficient)**:
   - Stage 1: Discover existing root instruction files (`AGENTS.md`, `CLAUDE.md`, `CONTEXT.md`), Kilo configs (`kilo.json`, `.kilo/`), and project manifests (`*.sln`, `*.csproj`).
   - Stage 2: Read target manifests to establish target framework, installed packages, and project references.
   - Stage 3: Read collision files only (e.g. existing `AGENTS.md` or `.kilo/kilo.json`). Do NOT recursively load entire source or documentation trees.
3. **Classify Actions & Identify Collisions**:
   - `ADD`: Missing baseline file safe to introduce (e.g. `docs/standards/`, `docs/repository-profile-template.md`).
   - `MERGE`: Existing file requiring reconciliation (`AGENTS.md` gets baseline reference block; `kilo.json` gets missing compaction/default_agent).
   - `SKIP`: Target has an authoritative equivalent (e.g. active `README.md`, existing CI definitions).
   - `CONFLICT`: Agent, command, or standard collision with differing content requiring user decision.
   - `DEFER`: Artifact requiring unverified prerequisites.
4. **Generate Adoption Plan**:
   - Produce a complete, structured plan following `docs/adoption/adoption-plan-template.md`.
   - List each required decision key and recommended resolution per `docs/adoption/conflict-resolution.md`.
5. **Halt Without Mutation**:
   - Output the complete adoption plan and decision list for human review.
   - **DO NOT modify any files in the target directory.** Instruct the user to review the plan and approve implementation via `/implement-approved-adoption`.
