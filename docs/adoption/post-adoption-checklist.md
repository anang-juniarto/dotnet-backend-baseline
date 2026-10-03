# Post-Adoption Verification Checklist

> **Classification:** `[CORE]`  
> **Status:** Normative Checklist  
> **Target:** Verification steps executed immediately following `/implement-approved-adoption`.

Read this source-only checklist from the baseline workspace, not a required destination copy. Follow the [canonical lifecycle](./ai-implementation-workflow.md): verify permanent changes before any exact approved legacy cleanup, then repeat link/configuration/discovery checks after cleanup. Both scopes exclude adoption docs and the three adoption commands; retain all source assets.

---

## 1. Kilo Configuration & Tooling Verification

- [ ] **Valid Kilo JSON**: Verify `.kilo/kilo.json` parses cleanly without syntax errors:
  ```powershell
  Get-Content ".kilo/kilo.json" | ConvertFrom-Json
  ```
- [ ] **Approved-Effective Default Agent**: Confirm `default_agent` matches the approved effective configuration and resolves to an available agent, including collision renames; do not force `team-lead` over an existing setting.
- [ ] **Approved-Effective Compaction**: Confirm compaction matches approved effective values; preserve destination settings unless a change was explicitly approved.
- [ ] **Agent Discovery**: Verify approved agents load, accounting for preserved destination agents and collision renames. Baseline identifiers are:
  - `team-lead` (primary)
  - `system-analyst` (subagent)
  - `architect-reviewer` (subagent)
  - `backend-feature` (subagent)
  - `database-engineer` (subagent)
  - `test-engineer` (subagent)
  - `code-reviewer` (subagent)
  - `devops-operations` (subagent)
- [ ] **Read-Only Permissions**: Confirm `code-reviewer.md` and `architect-reviewer.md` maintain `permission.edit: deny`.
- [ ] **Command Discovery**: Verify approved permanent commands appear (including `/implement-change`, `/analyze-graph`, and `/refresh-graph` when selected); adoption commands remain source-only. Other baseline commands include:
  - `/analyze-feature`
  - `/implement-feature`
  - `/diagnose`
  - `/review-architecture`
  - `/review-code`

---

## 2. Documentation & Profile Verification

- [ ] **Repository Profile Created**: Confirm `docs/repository-profile.md` exists and is populated with verified facts:
  - Runtime and SDK versions match installed environment.
  - Active solution and project paths match filesystem.
  - Verified architectural pattern is recorded.
  - Active persistence and schema authority are stated.
- [ ] **Document Status Accurate**: Confirm all unverified profile documents remain in `Draft` status until verified against executable code.
- [ ] **Relative Links Valid**: Confirm all internal Markdown links resolve without broken paths.
- [ ] **No Invented Artifacts**: Confirm deferred directories (e.g. `docs/api/endpoints/`) were not generated prematurely.

---

## 3. Cleanup & Final-State Verification

- [ ] **Exact Approval & Evidence**: Every cleanup candidate is an exact existing-destination file explicitly approved in the plan, with baseline provenance/version, unchanged content comparison, and no customization/worktree drift. Uncertain or user-owned files remain preserved conflicts; no recursive directory deletion.
- [ ] **Verification Gate**: Permanent configuration, discovery, fallback, and reference checks passed before cleanup. Recheck each candidate immediately before removal; already absent files are no-ops. Failed checks stop dependent cleanup and prevent completion claims.
- [ ] **Final Tree**: Repeat links/anchors, effective configuration, and agent/command discovery after cleanup. Destination README/index/manifest and other permanent artifacts do not depend on missing adoption docs/commands; permanent capability fallback uses `AGENTS.md` section 2.
- [ ] **Source & User Preservation**: Source adoption docs/commands/links and all destination user-owned/customized files are intact; list conflicts and exact removed/preserved paths. No supplied destination means no deletion.

## 4. Environment & Execution Safety

- [ ] **Clean Worktree**: Check git status to ensure only intended baseline files were staged or committed:
  ```powershell
  git status
  ```
- [ ] **Restore & Build Verification**: Execute the project's authorized build commands:
  ```powershell
  dotnet restore
  dotnet build --no-restore
  ```
- [ ] **Test Execution**: Run automated tests and report exact passed/failed counts:
  ```powershell
  dotnet test --no-build
  ```
- [ ] **No Secret Leaks**: Verify that no connection strings, tokens, or environment passwords were committed in configuration files.
- [ ] **Truthful Reporting**: Record any unexecuted checks or missing tooling with concrete rationale in the initial adoption commit message or PR description.
