---
description: Implement an approved adoption plan into a target repository using AI edit tools
agent: team-lead
---
Implement the explicitly approved adoption plan for: $ARGUMENTS

## Objective
Run from the baseline source using `docs/adoption/ai-implementation-workflow.md` and `docs/adoption/adoption-plan-template.md`. This command and both adoption planning commands are source-only, as is `docs/adoption/`; do not copy them into either destination scope or delete them from source.

## Input & Pre-Flight
- Require an absolute existing `target` distinct from baseline source and deployments; no destination means no mutation/deletion.
- Scope defaults to `core-reference` for both new and existing projects; `full-reference` requires explicit choice and exact-file approval under `BASELINE.md`. Re-review older plans under current policy before execution.
- Normalize/verify physical roots; reject equality or ancestor/descendant overlap. Resolve aliases/junctions where verifiable, block uncertainty, and never automatically follow links/reparse points. Recheck each mapping for containment/traversal, duplicate destinations under target filesystem semantics and canonical exclusions on both source/destination paths; renames cannot bypass exclusions.
- Confirm excluded/deferred inventory, scope basis/need and dependency closure for every approved exact file. Git metadata files/directories (including nested) and `.kilo/plans/` never transfer. Preserve pre-existing excluded target files; later separately authorized generation is not import.
- Read LICENSE; require an approved exact attribution location retaining its full MIT notice. Preserve target root license, README identity and CHANGELOG history; CONTRIBUTING merges are optional and target-compatible. Unresolved license decisions block related transfer as `CONFLICT`/`DEFER`.
- Require actual user approval of the concrete plan and decisions; `approved=true` alone does not authorize unspecified cleanup.
- Re-read source and destination file state against recorded plan evidence before editing. Unexpected drift, scope changes or new conflicts mean halt dependent operations and obtain refreshed approval, not overwrite.

## Permanent Operations
- Apply only individually approved exact-file `ADD`/`MERGE` actions using normal tools, one file at a time. No bulk recursive copy, mirror, unlisted writes or automatic sample ADR/spec/release instances. Preserve `SKIP`, unresolved conflicts, unrelated user content, and all source-only exclusions.
- Merge destination `AGENTS.md` without duplicating an existing baseline reference block or erasing project rules. Ensure permanent capability fallback lives in section 2, not in destination adoption files.
- Merge only approved missing configuration settings. Preserve existing `default_agent`, compaction, tools, MCPs, plugins, and permissions unless explicit approval changes a setting. Validate approved-effective identifiers, including renamed agents.
- Initialize an approved missing profile as Draft; reconcile an existing profile without overwriting verified facts.
- Adapt destination README/index/manifest and all permanent references before cleanup: remove missing local adoption links/routes or point to verified accessible source locations. Do not edit source links away.
- No Git mutations, package installation, database migrations, or external tooling without separate authorization. Structural indexing is optional and requires its own authorization; absence falls back to manifests.

## Verify → Authorized Cleanup → Verify Again
1. Audit actual added/modified files against the approved manifest, including adaptations, attribution and profile generation; mismatches stop dependent cleanup. Validate references against the selected core/full destination tree, not merely source; adapt missing optional topic routes to permanent fallback. Validate permanent configuration syntax/approved values, agent and non-adoption command discovery, Markdown paths/anchors, and fallback. Verify no permanent artifact requires a cleanup candidate. Consult `docs/adoption/post-adoption-checklist.md` in the source workspace.
2. Only after successful verification, process the plan's exact explicitly approved existing-destination cleanup list. Re-read each file immediately before deletion and confirm baseline provenance/version, content state, no customization, and no worktree drift. Names/sentinels alone are not ownership evidence. Remove only unchanged, uncustomized, baseline-owned files by exact path; never recursively delete an unverified folder. Already absent approved files are no-ops.
3. Failed verification, uncertain ownership, drift, or customization means preserve files, stop dependent cleanup, and report conflicts/partial completion. Approval never permits erasing user-owned content or automatic rollback/destructive cleanup after failure.
4. Repeat link/anchor, configuration, and discovery checks on the actual final tree after cleanup. Check user-content preservation and absence of permanent local adoption dependencies. Run applicable discovered target build/test/lint only when authorized.
5. Report `Implemented`, `Partially implemented`, or `Blocked` based on evidence; list added/merged/preserved/removed/already-absent/conflicted paths and checks passed/failed/unexecuted with reasons. Do not claim completion on failed verification or unresolved cleanup conflicts.
