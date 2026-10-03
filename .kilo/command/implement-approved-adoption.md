---
description: Implement an approved adoption plan into a target repository using AI edit tools
agent: team-lead
---
Implement the explicitly approved adoption plan for: $ARGUMENTS

## Objective
Run from the baseline source using `docs/adoption/ai-implementation-workflow.md` and `docs/adoption/adoption-plan-template.md`. This command and both adoption planning commands are source-only, as is `docs/adoption/`; do not copy them into either destination scope or delete them from source.

## Input & Pre-Flight
- Require an absolute existing `target` distinct from baseline source and deployments; no destination means no mutation/deletion.
- Scope defaults to `core-reference` for existing projects and `full-reference` for new projects; use the approved scope from `BASELINE.md`.
- Require actual user approval of the concrete plan and decisions; `approved=true` alone does not authorize unspecified cleanup.
- Re-read source and destination file state against recorded plan evidence before editing. Unexpected drift means halt dependent operations and obtain refreshed approval, not overwrite.

## Permanent Operations
- Apply only individually approved `ADD`/`MERGE` actions using normal tools, one file at a time. Preserve `SKIP`, unresolved conflicts, unrelated user content, and all source-only exclusions.
- Merge destination `AGENTS.md` without duplicating an existing baseline reference block or erasing project rules. Ensure permanent capability fallback lives in section 2, not in destination adoption files.
- Merge only approved missing configuration settings. Preserve existing `default_agent`, compaction, tools, MCPs, plugins, and permissions unless explicit approval changes a setting. Validate approved-effective identifiers, including renamed agents.
- Initialize an approved missing profile as Draft; reconcile an existing profile without overwriting verified facts.
- Adapt destination README/index/manifest and all permanent references before cleanup: remove missing local adoption links/routes or point to verified accessible source locations. Do not edit source links away.
- No Git mutations, package installation, database migrations, or external tooling without separate authorization. Structural indexing is optional and requires its own authorization; absence falls back to manifests.

## Verify → Authorized Cleanup → Verify Again
1. Validate permanent configuration syntax/approved values, agent and non-adoption command discovery, Markdown paths/anchors, and fallback. Verify no permanent artifact requires a cleanup candidate. Consult `docs/adoption/post-adoption-checklist.md` in the source workspace.
2. Only after successful verification, process the plan's exact explicitly approved existing-destination cleanup list. Re-read each file immediately before deletion and confirm baseline provenance/version, content state, no customization, and no worktree drift. Names/sentinels alone are not ownership evidence. Remove only unchanged, uncustomized, baseline-owned files by exact path; never recursively delete an unverified folder. Already absent approved files are no-ops.
3. Failed verification, uncertain ownership, drift, or customization means preserve files, stop dependent cleanup, and report conflicts/partial completion. Approval never permits erasing user-owned content.
4. Repeat link/anchor, configuration, and discovery checks on the actual final tree after cleanup. Check user-content preservation and absence of permanent local adoption dependencies. Run applicable discovered target build/test/lint only when authorized.
5. Report `Implemented`, `Partially implemented`, or `Blocked` based on evidence; list added/merged/preserved/removed/already-absent/conflicted paths and checks passed/failed/unexecuted with reasons. Do not claim completion on failed verification or unresolved cleanup conflicts.
