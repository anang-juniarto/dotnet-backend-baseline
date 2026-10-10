# AI-Assisted Baseline Implementation Workflow

> **Classification:** `[CORE]`  
> **Status:** Normative Workflow (source-only)  
> **Authority:** Canonical adoption lifecycle; retained in the baseline source, not copied to destinations.

## 1. Five-Phase Implementation Lifecycle

### Phase A: Read-Only Discovery
- Normalize absolute source and destination roots; reject equal or ancestor/descendant roots and active deployments. Resolve aliases/junctions when verifiable; uncertainty blocks writes. Do not automatically follow symlinks/reparse points in enumeration. Check mapped file containment and traversal. No supplied destination means no destination mutation and cleanup `None`.
- Inspect existing instructions, effective Kilo configuration, manifests, architecture, and relevant worktree changes without modifying files.
- Discover baseline version and source state; record target file state for later drift checks.

### Phase B: Adoption Plan Authoring
- Use [the plan template](./adoption-plan-template.md) and canonical scope, exclusion, root-document and attribution rules from [BASELINE.md](../../BASELINE.md): `core-reference` defaults for both new and existing projects; `full-reference` is explicit opt-in.
- Discover candidates, filter canonical exclusions on both source and destination mappings, then enumerate exact file paths with concrete need, scope basis and recorded source/target state. Classify actions as `ADD`, `MERGE`, `SKIP`, `CONFLICT`, or `DEFER`. Directory names/globs are discovery only; no bulk recursive copy or mirror. Unlisted files cannot transfer.
- Inventory excluded/deferred candidates with reasons, including Git metadata files/directories at any depth, plans, state, secrets and deferred source fixtures. Exclusions never authorize deleting existing target artifacts; later authorized target generation is distinct from importing payload.
- Record decisions for README, CHANGELOG, CONTRIBUTING and LICENSE. Preserve target identity/history/workflows/root license. Read the source MIT license and approve an exact attribution location retaining its full notice; unresolved attribution blocks related transfer as `CONFLICT`/`DEFER`. Keep baseline version identity separate from application history.
- Mark all `docs/adoption/` files and the three commands `plan-new-project-adoption.md`, `plan-existing-project-adoption.md`, and `implement-approved-adoption.md` under `.kilo/command/` as `SOURCE-ONLY`: retain in source; do not copy to a new destination.
- For legacy adoption assets in an existing destination, propose a separate exact per-file `CLEANUP` list with baseline provenance/version, content comparison, customization/worktree findings, and recorded content state. File names or sentinels alone are insufficient evidence of baseline ownership.
- Preserve customized, user-owned, drifted, or uncertain files as conflicts. Do not propose recursive deletion of unverified folders.
- Specify adaptations to permanent destination README files, documentation index, manifest, agents, and commands so they do not depend on missing local adoption files. Remove destination-only adoption links/routes or replace them with verified accessible source references. Retain source baseline links unchanged.
- Permanent fallback belongs in destination `AGENTS.md` section 2: verified profiles → manifests/source → applicable universal standards. No destination capability-matrix dependency.
- Record approved-effective configuration, including preserved or renamed agents and `default_agent`, without forcing baseline defaults over existing settings.
- Separate reference scope from application selection and choose new reference/generation or existing reference/capability/migration mode. Reference-only never converts runtime/architecture. New separately approved generation defaults to .NET 10 Core Domain/Application MediatR feature slices, modular Infrastructure provider/capability assemblies, Presentation WebApi controllers (optional Grpc), and Unit/Integration/Architecture tests; preserve verified brownfield boundaries until a separate migration approves change.
- Record every optional selection and dependency using the [capability matrix](./capability-matrix.md), including independent SignalR/chat/inbox/web/mobile push choices. All integrations default off; record versions/licenses and proposed versus tested evidence. `full-reference` does not select runtime stacks.
- Separate exact reference-import rows from target-generated/modified rows and their execution approvals. Source `examples/**`, including `examples/Baseline.Sample`, is excluded in both scopes; reviewed blueprints may inform separately approved generation, never recursive source copy. Client/device/account work is separately scoped.
- Emit the plan and halt without target mutation.

### Phase C: Human Review & Approval
- Require explicit approval of the concrete plan, destination, scope, merge decisions, and each cleanup path before implementation. An `approved=true` argument alone is not evidence of approval of unspecified deletions.
- Unresolved collisions require preservation and reporting, not inferred permission. Cleanup approval never permits deleting user customizations.

### Phase D: Controlled AI Implementation
- Re-review older plans under current policy. Before editing, recheck source/target state, normalized roots and containment, exclusions on both mappings, duplicate destinations (including case/alias equivalence), attribution and document/command dependencies. Unexpected drift, scope changes or new conflicts require refreshed approval; do not overwrite.
- Approval covers exact files only, including adaptations and profile initialization. Never perform recursive copy, mirror, or unlisted writes.
- Apply approved permanent additions/merges one file at a time using normal tools, preserving target rules, permissions, custom tools, MCPs, and user changes.
- Initialize a missing approved repository profile as Draft; merge rather than overwrite an existing profile. Adapt destination gateways and references before cleanup.
- Do not copy source-only assets or examples. Generate executable artifacts only under their separately approved exact manifest; no automatic runtime/architecture conversion. Implement selected store/identity before dependent chat/inbox, then realtime/push dispatch and approved scale-out; omit unselected modules entirely. Preserve existing integrations rather than silently replacing them.
- Do not commit, install packages, migrate databases, start services or run external tooling without appropriate separate authorization.

### Phase E: Verification, Authorized Cleanup & Final Reporting
1. Compare actual added/modified files against approved `ADD`/`MERGE` entries, including attribution and generated profiles; report any mismatch and stop dependent cleanup. Preserve unlisted files and pre-existing excluded artifacts. Verify links/anchors and dependency closure against the selected core/full destination tree, adapting optional topic routes to AGENTS fallback rather than importing unnecessary files. Verify permanent artifacts first: effective configuration syntax and approved values, agent/command discovery, permanent fallback, and reference integrity. Resolve dependencies on cleanup candidates before removing any candidate. Use [the source checklist](./post-adoption-checklist.md).
2. Only after successful pre-cleanup verification, re-read every explicitly approved cleanup file and compare ownership, content, and worktree evidence to the plan immediately before deletion. Delete only unchanged, uncustomized, baseline-owned files by exact destination path; never delete baseline source assets. An already absent approved file is a no-op, not an error or permission to remove a replacement path.
3. If verification fails, ownership is unclear, or customization/drift is detected, retain affected files, stop dependent cleanup, and report `Partially implemented` or `Blocked` with conflicts. Never claim completion or automatically roll back/destructively clean up failed adoption.
4. After cleanup, repeat Markdown path/anchor and permanent discovery/configuration checks against the actual final destination tree. Confirm no permanent instruction requires excluded adoption assets, and user content is preserved. Report any intentional retained conflicts.
5. Report exact added, merged, skipped, removed, already-absent, and conflicted paths; checks passed/failed/unexecuted with reasons; remaining limitations. Run destination build/test/lint only using discovered authorized commands when applicable. Read this workflow/checklist from source, not a missing destination copy.
