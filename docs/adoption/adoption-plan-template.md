# AI Baseline Adoption Plan Template

> **Classification:** `[TEMPLATE]`  
> **Status:** Reusable Blueprint (source-only)  
> **Usage:** Planning output for both adoption commands; follow the [canonical lifecycle](./ai-implementation-workflow.md).

## 1. Adoption Context & Target Discovery

- **Destination root**: `<verified absolute path distinct from source/deployment; none means no mutation>`
- **Baseline source root/version/state**: `<absolute path, version, recorded revision/content evidence>`
- **Adoption mode**: `<new-reference | new-generation | existing-reference (brownfield default) | existing-capabilities | existing-migration; generation/migration requires separate explicit approval>`
- **Reference scope**: `<core-reference by default; full-reference only by explicit choice; independent of application stack selection>`
- **Path safety evidence**: `<normalized physical roots; no equality/ancestor overlap; alias/junction resolution; no automatic link/reparse traversal; uncertain paths block writes>`
- **Mapping audit**: `<source/destination containment, traversal rejection, canonical exclusions on both sides, duplicate destination checks using target filesystem semantics>`
- **Verified profile**: `<runtime, manifests, solution/projects, architecture, schema ownership; unknowns remain explicit>`
- **Existing instructions/configuration/worktree**: `<paths, effective settings, unrelated changes to preserve>`
- **Approved-effective settings**: `<default_agent, compaction, agent/command identifiers including collision renames; preserve existing settings unless explicitly approved>`

### Application Selection & Evidence (Not Runtime Configuration)

Record all choices, including `none/off`, separately from reference imports:

- **Greenfield defaults:** `<.NET 10 stable SDK/net10.0; src/Core Domain/Application; MediatR CQRS Features/<Feature>/Commands|Queries/<UseCase>; src/Infrastructure provider/capability assemblies; src/Presentation WebApi controllers/optional Grpc; host composition roots; tests/<Project>.UnitTests, <Project>.IntegrationTests, <Project>.ArchitectureTests; exact generated projects and approved deviations>`
- **Brownfield invariants:** `<verified runtime, boundaries, mediator, contracts, schema/job ownership retained; migration scope/contract baselines/cutover/rollback if separately approved>`
- **Persistence:** `<primary none | sqlserver | mysql | postgresql | mongodb; additional MongoDB role/consistency; Elasticsearch projection/rebuild ownership>`
- **Infrastructure:** `<Redis cache; RabbitMQ publication recovery/outbox/consumer deduplication; gRPC contract; Hangfire engine and independently selected durable storage; worker separate/in-process/none>`
- **Observability:** `<Sentry error capture; Seq log route; OpenTelemetry traces/metrics/logs; one delivery/export owner per signal and redaction/outage strategy>`
- **Realtime:** `<SignalR off/single-node/approved Redis backplane/managed scale-out; affinity/auth/reconnect; chat identity + authoritative durable history>`
- **Notifications:** `<persistent inbox independently selected; realtime inbox requires inbox + SignalR; web/mobile push separately selected provider/platform; durable intent/attempt store and dispatcher; no implicit RabbitMQ/Hangfire/Firebase>`
- **Client boundaries:** `<browser service worker/mobile SDK/device/account setup excluded unless separately approved exact artifacts and platform scope>`
- **Compatibility/evidence:** `<exact SDK/package/provider/server versions, licenses, research date, proposed | verified | unavailable | deferred, executed checks and limitations; no build-only integration certification>`
- **Disabled-module proof:** `<unselected packages/schema/config/containers/hosted services/health probes/network calls absent>`

## 2. Permanent Action Manifest

Expand every row to exact source and destination file paths before approval. Filter [canonical exclusions](../../BASELINE.md#universal-exclusions) first on both mappings; renaming cannot bypass them. Directory names/globs are discovery candidates, never bulk recursive copy/mirror authorization. Unlisted files cannot transfer or be changed.

| Action | Artifact kind / source or blueprint | Exact destination file | Need / scope basis | Recorded source/target state | Merge/adaptation | Approval/decision |
|---|---|---|---|---|---|---|
| `ADD / MERGE / SKIP / CONFLICT / DEFER` | `<reference-import: exact source file; target-generated/modified: reviewed blueprint and selected capability, not copied example>` | `<absolute file>` | `<reference scope or separately approved generation/migration basis>` | `<content comparison/hash/revision, absent if new>` | `<preserve custom content; proposed change>` | `<explicit decision>` |

Keep reference-import rows distinct from executable generation/modification rows, even in one plan. Each generated project, package manifest, configuration, test, local-infrastructure or migration artifact needs its own exact row and execution approval. `examples/**`, including `examples/Baseline.Sample`, is excluded source payload in both scopes; blueprint consultation never authorizes recursive source copying. Do not invent generated migrations or assume optional adapters are tested.

### Root Documents & License Decisions

| Artifact | Target preservation / approved adaptation | Exact manifest destination / decision |
|---|---|---|
| README | Preserve target identity; never raw baseline identity | `<SKIP or approved target-specific merge>` |
| CHANGELOG | Preserve application history; no baseline release-history import | `<SKIP; baseline identity via .baseline-version/manifest>` |
| CONTRIBUTING | Optional target-compatible workflow only | `<SKIP or approved merge>` |
| BASELINE / indexes | Adapt to selected tree, removing missing adoption/topic routes | `<exact files and adaptations>` |
| LICENSE / attribution | Read source MIT terms; preserve full copyright/permission notice and disclaimer; preserve target root license without relicensing application | `<exact approved attribution file; unresolved = CONFLICT/DEFER before related transfer>` |

Include selected permanent config, agents, non-adoption commands, standards, scope-specific documents, and profile initialization/merge. Specify destination README/index/BASELINE adaptations and permanent agent/command routing changes to remove links and dependencies on excluded adoption assets. Preserve authoritative destination gateways; do not mark a needed adaptation `SKIP` and then edit it. Use `AGENTS.md` section 2 for permanent capability fallback.

## 3. Excluded/Deferred Inventory & Exact Cleanup List

| Exact candidate source / mapped destination | Classification | Reason / canonical exclusion | Existing target disposition |
|---|---|---|---|
| `<file; excluded tree inventory may group non-traversed paths with reason>` | `<SOURCE-ONLY / EXCLUDED / DEFER>` | `<BASELINE rule or missing need/evidence>` | `<preserve; not imported, not deletion authorization>` |

Record `.git` file/directory/nested metadata, `.kilo/plans/`, state/cache/build outputs, unselected `.kilo/` files, secrets/local config and deferred fixtures under the canonical policy. Later separately authorized target generation is not source import. Keep core blueprints as selected references; do not auto-create ADR/spec/release instances.

**Never copy to a new destination:** every file under source `docs/adoption/`, plus source `.kilo/command/plan-new-project-adoption.md`, `.kilo/command/plan-existing-project-adoption.md`, and `.kilo/command/implement-approved-adoption.md`. Inventory these files as `SOURCE-ONLY`, retaining all source assets and source links. Apply universal exclusions from [BASELINE.md](../../BASELINE.md) in both scopes.

**Existing destination cleanup:** `<None, or exact individually approved files below; no folder/glob deletion>`.

| Exact destination file | Baseline ownership evidence | Baseline version/source comparison | Recorded target content state | Customization/worktree evidence | References to adapt first | Explicit cleanup approval | Pre-delete recheck/result |
|---|---|---|---|---|---|---|---|
| `<absolute file, not folder>` | `<provenance/history, not name alone>` | `<known baseline content matches>` | `<hash or equivalent reproducible comparison>` | `<unmodified and no user-owned content, or CONFLICT>` | `<exact permanent files>` | `<decision key and user approval; pending means retain>` | `<unchanged / absent no-op / retain conflict>` |

Uncertain ownership, customization, or drift means preserve and report conflict. Approval does not authorize deletion of customized/user-owned files. No actual destination supplied means cleanup list `None` and no deletion. Repeated adoption treats already absent approved files as no-ops.

## 4. Decisions & Approval Record

Record user approval of the exact plan, source/destination, scope, permanent merges/adaptations, effective configuration, and each cleanup decision. Record unresolved conflicts and preserved files separately. Do not treat the command argument `approved=true` as approval for unspecified cleanup.

## 5. Ordered Verification & Reporting Plan

1. Re-review older plans; recheck roots/overlap, containment, exclusions on both mappings, duplicate destinations, attribution, dependency closure and recorded source/target state before edits. Drift, scope changes or new conflicts require refreshed approval.
2. Apply only exact approved permanent changes (no recursive copy/mirror), audit actual changes against the manifest, then verify effective configuration syntax/values, approved agent and non-adoption command discovery, Markdown paths/anchors, and AGENTS section 2 fallback. Inspect all references to cleanup candidates before deletion.
3. Only on successful pre-cleanup verification, recheck each approved cleanup file's ownership/content/customization evidence immediately before exact-file removal. Verification failure or uncertainty means retain and report partial/conflict, not recursive deletion or automatic rollback. Existing excluded target files are not a failure if adoption did not import/change them.
4. Repeat link/anchor, discovery, and configuration verification against the actual post-cleanup tree. Ensure README/index/manifest and other permanent artifacts do not require missing local adoption assets. Keep baseline source assets intact.
5. Execute discovered destination build/test/lint commands only where applicable and authorized; report unexecuted checks and reasons. Use the [source checklist](./post-adoption-checklist.md), not a destination adoption dependency.
6. Report `Implemented / Partially implemented / Blocked`, exact files added/merged/preserved/removed/already absent/conflicted, validation results, and remaining risks.
