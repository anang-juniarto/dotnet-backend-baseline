# Post-Adoption Verification Checklist

> **Classification:** `[CORE]`
> **Status:** Draft | **Owner:** Engineering Leads
> **Last verified:** Not verified | **Evidence:** Reference checklist; completed destination checks must be recorded separately
> **Target:** Verification steps executed immediately following `/implement-approved-adoption`.

Read this source-only checklist from the baseline workspace, not a required destination copy. Follow the [canonical lifecycle](./ai-implementation-workflow.md): verify permanent changes before any exact approved legacy cleanup, then repeat link/configuration/discovery checks after cleanup. Both scopes exclude adoption docs and the three adoption commands; retain all source assets.

---

## 0. Scope, Allowlist & Preservation Audit

- [ ] **Default & Approval**: Both new and existing default to `core-reference`; `full-reference` is explicit opt-in for eligible documentation, not all-stack selection. Older plans were re-reviewed under current policy. Every write has an approved exact-file ADD/MERGE entry with need, scope basis and source/target state; no directory/glob authorization, bulk recursive copy or mirror.
- [ ] **Path Safety**: Verified normalized physical roots do not overlap (equal/ancestor/descendant); aliases/junctions are resolved or blocked. No automatic link/reparse traversal. All mappings satisfy containment, reject traversal and duplicate destinations under target filesystem semantics.
- [ ] **Exclusions & Drift**: [Canonical exclusions](../../BASELINE.md#universal-exclusions) were checked against source and mapped destinations before approval and writes, including `.git` file/directory/nested metadata and `.kilo/plans/`. Renames do not bypass them. Source/target drift, scope changes and new conflicts require renewed approval.
- [ ] **Manifest Audit**: Compare actual added/modified files against approved manifest entries, including profile generation, attribution and link adaptations; investigate mismatches before cleanup. Inventory excluded/deferred candidates with reasons. Unlisted `.kilo/` files and secrets/local config were not imported.
- [ ] **Root Documents & License**: README identity, application CHANGELOG history, target workflow and root license are preserved. Optional CONTRIBUTING changes are approved. Source LICENSE was read and its full MIT notice retained at the exact approved attribution location; unresolved decisions block related transfer. No application-wide relicensing is implied.
- [ ] **Import vs Generation**: No excluded source payload was transferred. Existing target caches/plans/endpoint docs are not failures and are not deleted due to exclusion. Later separately authorized target generation is distinct from import; no implicit ADR/spec/release instances.
- [ ] **Dependency Closure**: Simulate the selected core/full destination tree before writes and check actual final links/anchors/discovery afterward. Optional topic/adoption routes are adapted without unnecessary imports; permanent AGENTS fallback and selected Graphify command/helper/docs dependencies remain coherent.

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

- [ ] **Approved Repository Profile**: If initialization/merge was approved, confirm `docs/repository-profile.md` matches that action; unknown facts remain Draft rather than invented. Verify applicable facts:
   - Runtime and SDK versions match recorded executable evidence. Inspect the target execution root and applicable ancestor `global.json` before any SDK change; reference-only adoption preserves its policy. The sample pins `10.0.201`, stable `latestPatch` within `10.0.2xx`; permitted patches are not automatically tested.
  - Active solution and project paths match filesystem.
  - Verified architectural pattern is recorded.
  - Active persistence and schema authority are stated.
- [ ] **Document Status Accurate**: Confirm all unverified profile documents remain in `Draft` status until verified against executable code.
- [ ] **Relative Links Valid**: Confirm all internal Markdown links resolve without broken paths.
- [ ] **No Invented Artifacts**: Confirm deferred directories (e.g. `docs/api/endpoints/`) were not generated prematurely.

---

## 3. Selected Application Verification (Only When Separately Approved)

- [ ] **Mode & Defaults**: Reference scope is separate from stack selection. Reference-only did not change runtime, packages, architecture or deployment. New generation uses approved .NET 10 Core Domain/Application MediatR feature slices, modular Infrastructure provider/capability assemblies, Presentation WebApi controllers (optional Grpc), and Unit/Integration/Architecture tests; brownfield capability work preserves verified boundaries. Migration has separate contract baselines, feasibility, cutover/rollback and approval.
- [ ] **Generation Boundary**: Exact generated/modified files match their separate manifest; no `examples/**` reference transfer or recursive sample source copy. Blueprint use is reviewed; the in-memory sample is not optional-integration certification.
- [ ] **Selection Evidence**: Every capability records proposed/verified/unavailable/deferred status, exact version/license/server/SDK evidence and executed/unexecuted checks. Restore/build alone is not provider certification; unsupported MySQL EF10 combinations are not silently downgraded.
- [ ] **EF Mapping Ownership**: When EF is selected, business entities/value objects remain in Domain, shared mapping registration explicitly scans `typeof(ApplicationDbContext).Assembly`, and provider contexts call base registration before scanning their own assembly. Intra-assembly scan order is undefined; conflicting configurations use deterministic registration. Brownfield mapping changes need separate approval.
- [ ] **Architecture & Contracts**: Verify selected boundaries (greenfield inward references, Core feature slices, provider/capability assembly isolation, Presentation host composition-root wiring and MediatR request dispatch; preserved brownfield patterns otherwise), command invariants/read-only queries where CQRS is selected, cancellation, transport contract compatibility and no forbidden SDK/persistence type leakage. HTTP request DTOs are separated from commands when commands carry server-trusted actor/owner/tenant context.
- [ ] **Stores & Recovery**: Real-provider tests are executed only against target-selected capabilities using isolated test infrastructure; unselected stacks do not imply test prerequisites. Selected relational engines verify constructor round-trip with immutable Domain entities, migration/concurrency and unknown-outcome behavior; MongoDB indexes/topology and Elasticsearch staleness/rebuild are verified where selected. Redis expiry/invalidation/bypass, RabbitMQ confirms/ack/deduplication/outbox/DLQ, Hangfire durable storage/retry/dashboard and gRPC auth/deadline/limits are checked only when selected.
- [ ] **Telemetry**: Sentry/Seq/OpenTelemetry have one capture/export owner per signal, bounded buffers/labels, sampling, privacy/redaction, correlation and outage behavior. Noncritical exporter failures do not automatically fail readiness.
- [ ] **Realtime & Chat**: SignalR authorization/membership revocation, token/query redaction, bounded hub invocations, reconnect/catch-up and selected scale-out/affinity are tested. Single-node has no implicit Redis. Chat has durable acceptance, deduplicated sends and deterministic history cursors; live publication is not atomic with commit or proof of receipt/read.
- [ ] **Inbox & Push**: Test recipient isolation, cross-device unread/read idempotency, preference/consent, durable intent/attempt recovery, destination registration ownership/rotation/logout, bounded retry/expiry/cleanup and provider outages. Reject unsafe Web Push endpoints/deep links and redact credentials/private lock-screen payloads. Provider acceptance is not delivery/read; fake adapters are default and live device/account tests need explicit permission.
- [ ] **Disabled & Client Scope**: Unselected modules add no package, schema, hub, container, required setting/secret, health probe, hosted service or network dependency. Chat/inbox/push are independent; no implicit Firebase/Redis/RabbitMQ/Hangfire. Browser service workers/mobile SDKs/accounts remain separately approved client scope.

## 4. Cleanup & Final-State Verification

- [ ] **Exact Approval & Evidence**: Every cleanup candidate is an exact existing-destination file explicitly approved in the plan, with baseline provenance/version, unchanged content comparison, and no customization/worktree drift. Uncertain or user-owned files remain preserved conflicts; no recursive directory deletion.
- [ ] **Verification Gate**: Permanent configuration, discovery, fallback, and reference checks passed before cleanup. Recheck each candidate immediately before removal; already absent files are no-ops. Failed checks stop dependent cleanup and prevent completion claims.
- [ ] **Final Tree**: Repeat links/anchors, effective configuration, and agent/command discovery after cleanup. Destination README/index/manifest and other permanent artifacts do not depend on missing adoption docs/commands; permanent capability fallback uses `AGENTS.md` section 2.
- [ ] **Source & User Preservation**: Source adoption docs/commands/links and all destination user-owned/customized files are intact; list conflicts and exact removed/preserved paths. No supplied destination means no deletion.

## 5. Environment & Execution Safety

- [ ] **Worktree Audit**: Check read-only git status/diff against the manifest and initial user changes; adoption does not stage or commit files:
  ```powershell
  git status
  ```
- [ ] **Restore & Build Verification**: Execute only discovered, applicable and separately authorized target README/script commands. No reference adoption step authorizes restore, network access or infrastructure startup. For source-example evaluation, the [sample README](../../examples/Baseline.Sample/README.md) owns commands/routes for default memory, selected host modules, aggregate compilation, separate gRPC and artifact/CI profiles; do not duplicate or transfer them.
- [ ] **Test Execution**: Run only authorized target tests and report exact executed passed/failed/skipped counts. Separate historical sample evidence (previously recorded 34 passing tests) from new runs; no new count or production/provider certification follows from documentation review.
- [ ] **No Secret Leaks**: Verify that no connection strings, tokens, or environment passwords were committed in configuration files.
- [ ] **Truthful Reporting**: Report exact changed/preserved/removed/conflicted files, executed results, unexecuted checks and reasons, and remaining concerns. No automatic rollback or destructive cleanup on failure. No commit/PR is required or authorized by this checklist.
