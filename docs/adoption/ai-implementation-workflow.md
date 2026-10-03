# AI-Assisted Baseline Implementation Workflow

> **Classification:** `[CORE]`  
> **Status:** Normative Workflow (source-only)  
> **Authority:** Canonical adoption lifecycle; retained in the baseline source, not copied to destinations.

## 1. Five-Phase Implementation Lifecycle

### Phase A: Read-Only Discovery
- Verify an absolute destination path distinct from the baseline source and active deployments. No supplied destination means no destination mutation or cleanup.
- Inspect existing instructions, effective Kilo configuration, manifests, architecture, and relevant worktree changes without modifying files.
- Discover baseline version and source state; record target file state for later drift checks.

### Phase B: Adoption Plan Authoring
- Use [the plan template](./adoption-plan-template.md) and scopes from [BASELINE.md](../../BASELINE.md): `full-reference` defaults for new projects; `core-reference` for existing projects.
- Enumerate exact source and destination file paths, not executable directory globs. Classify permanent actions as `ADD`, `MERGE`, `SKIP`, `CONFLICT`, or `DEFER`.
- Mark all `docs/adoption/` files and the three commands `plan-new-project-adoption.md`, `plan-existing-project-adoption.md`, and `implement-approved-adoption.md` under `.kilo/command/` as `SOURCE-ONLY`: retain in source; do not copy to a new destination.
- For legacy adoption assets in an existing destination, propose a separate exact per-file `CLEANUP` list with baseline provenance/version, content comparison, customization/worktree findings, and recorded content state. File names or sentinels alone are insufficient evidence of baseline ownership.
- Preserve customized, user-owned, drifted, or uncertain files as conflicts. Do not propose recursive deletion of unverified folders.
- Specify adaptations to permanent destination README files, documentation index, manifest, agents, and commands so they do not depend on missing local adoption files. Remove destination-only adoption links/routes or replace them with verified accessible source references. Retain source baseline links unchanged.
- Permanent fallback belongs in destination `AGENTS.md` section 2: verified profiles → manifests/source → applicable universal standards. No destination capability-matrix dependency.
- Record approved-effective configuration, including preserved or renamed agents and `default_agent`, without forcing baseline defaults over existing settings. Emit the plan and halt without target mutation.

### Phase C: Human Review & Approval
- Require explicit approval of the concrete plan, destination, scope, merge decisions, and each cleanup path before implementation. An `approved=true` argument alone is not evidence of approval of unspecified deletions.
- Unresolved collisions require preservation and reporting, not inferred permission. Cleanup approval never permits deleting user customizations.

### Phase D: Controlled AI Implementation
- Re-read source and target state against the plan before editing. Unexpected drift requires a refreshed approved plan; do not overwrite it.
- Apply approved permanent additions/merges one file at a time using normal tools, preserving target rules, permissions, custom tools, MCPs, and user changes.
- Initialize a missing approved repository profile as Draft; merge rather than overwrite an existing profile. Adapt destination gateways and references before cleanup.
- Do not copy source-only assets. Do not commit, install packages, migrate databases, or run external tooling without appropriate separate authorization.

### Phase E: Verification, Authorized Cleanup & Final Reporting
1. Verify permanent artifacts first: effective configuration syntax and approved values, agent/command discovery, permanent fallback, and reference integrity. Resolve dependencies on cleanup candidates before removing any candidate. Use [the source checklist](./post-adoption-checklist.md).
2. Only after successful pre-cleanup verification, re-read every explicitly approved cleanup file and compare ownership, content, and worktree evidence to the plan immediately before deletion. Delete only unchanged, uncustomized, baseline-owned files by exact destination path; never delete baseline source assets. An already absent approved file is a no-op, not an error or permission to remove a replacement path.
3. If verification fails, ownership is unclear, or customization/drift is detected, retain affected files, stop dependent cleanup, and report `Partially implemented` or `Blocked` with conflicts. Never claim completion.
4. After cleanup, repeat Markdown path/anchor and permanent discovery/configuration checks against the actual final destination tree. Confirm no permanent instruction requires excluded adoption assets, and user content is preserved. Report any intentional retained conflicts.
5. Report exact added, merged, skipped, removed, already-absent, and conflicted paths; checks passed/failed/unexecuted with reasons; remaining limitations. Run destination build/test/lint only using discovered authorized commands when applicable. Read this workflow/checklist from source, not a missing destination copy.
