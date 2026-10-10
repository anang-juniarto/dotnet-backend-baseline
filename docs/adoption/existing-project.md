# Existing Project Merge Playbook

> **Classification:** `[CORE]`
> **Status:** Draft | **Owner:** Engineering Leads
> **Last verified:** Not verified | **Evidence:** Reference merge workflow; destination execution required
> **Target Context:** Brownfield / Existing active .NET repository
> **Default Scope:** `core-reference`
> **Protocol:** AI Plan $\rightarrow$ Review $\rightarrow$ Implement

This source-only playbook follows the [canonical lifecycle](./ai-implementation-workflow.md) and [scope manifest](../../BASELINE.md). Both scopes exclude `docs/adoption/` and the three adoption commands from copying. For legacy destination adoption files, list exact cleanup paths with explicit approval and baseline ownership/content/customization evidence. Verify permanent changes first, recheck drift immediately before cleanup, then verify final links/discovery. Preserve user files and uncertain/customized files as conflicts; never recursively delete unverified folders. Adapt destination README/index/manifest links while retaining source links; permanent fallback is in `AGENTS.md` section 2.

---

Both project types default to `core-reference`; `full-reference` is explicit opt-in, still requiring an exact-file allowlist. Follow [BASELINE.md](../../BASELINE.md) for canonical exclusions on source and mapped destination paths, root-document preservation and attribution. Never bulk recursively copy/mirror directories; unselected `.kilo/` content cannot transfer. Exclusion is not deletion, and later separately authorized target generation is not source import.

Normalize physical roots, reject equality/ancestor overlap and block unresolved aliases/junctions; do not automatically follow links/reparse points. Check containment, traversal, duplicate destinations, recorded source/target drift and dependency closure. Record each file's need/scope basis and excluded/deferred inventory. Read LICENSE and approve an exact attribution location retaining its full MIT notice without replacing the target root license. Preserve README identity, CHANGELOG history and optional target-compatible CONTRIBUTING. Unresolved attribution means CONFLICT/DEFER before related transfer.

## Adoption Mode (Independent of Reference Scope)

Minimal path: inventory the target runtime, contracts, schema authority and worktree; choose `existing-reference` with `core-reference`; approve only missing or reconciled guidance; apply the exact manifest and verify links/configuration without changing the application. For a later capability request, add one approved capability at the existing composition seam and run the target's contract regressions. `full-reference` adds eligible reference topics, never an all-stack implementation.

Choose and approve one mode before planning writes:

| Mode | Authorized intent | Preserved boundary |
|---|---|---|
| `existing-reference` (default) | Selected permanent guidance only | No runtime, packages, architecture, schema or deployment conversion |
| `existing-capabilities` | Individually selected adapters/use cases with exact generated/modified-file approval | Reuse verified composition roots, transport, mediator and persistence; no automatic .NET 10/Clean Architecture/CQRS migration |
| `existing-migration` | Separately approved incremental runtime/architecture change | Contract baselines, SDK/provider feasibility, seams, cutover/rollback and consumer migration required |

.NET 10 Core Domain/Application MediatR feature slices, modular Infrastructure provider/capability assemblies, Presentation WebApi (optional Grpc), and Unit/Integration/Architecture tests are the greenfield default, not permission to reorganize brownfield code. Preserve existing Minimal APIs, CRUD/service boundaries, dispatchers, public envelopes, events, schemas and job payloads until explicitly approved migration. Avoid blanket project moves and default dual-write.

Keep reference scope separate from stack selection using the [capability matrix](./capability-matrix.md). Discover existing telemetry, stores, messaging, jobs, identity, SignalR/chat/inbox and push systems before adding alternatives; do not create duplicate systems. Every selected capability requires version/license compatibility and acceptance evidence. Unsupported combinations remain `DEFER`. `examples/**`, including `examples/Baseline.Sample`, never transfers as reference payload; separately approved generation may use a reviewed blueprint, not recursive source copy.

## 1. Safety Principles & Pre-Flight Checks

1. **Clean or Understood Worktree**: Inspect `git status` to understand existing working-tree changes. Adoption commands preserve unrelated changes and do NOT run Git operations automatically.
2. **Human Git Branching**: Human operators SHOULD create an isolated branch (e.g. `git switch -c chore/adopt-ai-baseline`) before adopting. AI agents are prohibited from creating or switching branches without explicit authorization.
3. **No Blind Overwrite**: Existing architecture, contracts, routes, and custom configuration MUST NOT be overwritten without human review.
4. **Mandatory Planning**: Adoption always starts with a read-only plan (`/plan-existing-project-adoption`). No mutations occur until you inspect the plan and explicitly authorize implementation.

---

## 2. Phase 1: Planning Phase (Read-Only)

Execute the planning command from this baseline workspace:

```text
/plan-existing-project-adoption target="C:\Projects\ExistingApi" [scope=core-reference]
```

### What Happens in Phase 1:
1. **Target Discovery**:
   - Inspects target `.csproj`, solution structure, existing `AGENTS.md`, and `.kilo/` configuration.
   - Identifies active contracts, runtime version, and dependencies.
2. **Classification Output**:
   Produces an actionable adoption plan following [`adoption-plan-template.md`](./adoption-plan-template.md):
   - `ADD`: Missing artifact that is safe to add (e.g. `docs/standards/README.md`, `docs/repository-profile-template.md`).
   - `MERGE`: Existing file that requires reconciliation (e.g. `AGENTS.md`, `.kilo/kilo.json`).
   - `SKIP`: Target already has an authoritative equivalent (e.g. active `README.md`, existing CI pipelines).
   - `CONFLICT`: Agent, command, or standard collision with differing content requiring user direction.
   - `DEFER`: Artifact requiring missing evidence or unverified prerequisite.
3. **Halts Without Mutation**: The planning phase stops without making any modifications to the target directory.

---

## 3. Reviewing the Plan & Resolving Decisions

Inspect the plan generated by Phase 1:
1. Consult [`conflict-resolution.md`](./conflict-resolution.md) for standard resolution policies.
2. Ensure existing API error formats, routes, and database migration ownership are not contradicted.
3. If an agent name collides (e.g. target already has `code-reviewer`), designate a renamed identifier or approve overwrite.

---

## 4. Phase 2: Implementation Phase

Once the plan has been inspected and decisions approved, execute the implementation command:

```text
/implement-approved-adoption target="C:\Projects\ExistingApi" [scope=core-reference] approved=true
```

### What Happens in Phase 2:
1. **Drift Check**: Verifies that neither the baseline nor target files have changed since the plan was created.
2. **Selective Merge**:
   - Merges root `AGENTS.md` (injecting universal guardrails while preserving all project-specific rules).
   - Merges `.kilo/kilo.json` without overriding project-specific tools or permissions.
   - Adds approved agents and non-adoption commands only, with collision renames applied; validates approved-effective `default_agent` rather than overriding existing settings.
   - Adds/merges individually approved standards files under `docs/standards/`.
   - Initializes a missing approved `docs/repository-profile.md` from the profile template as `Draft`; preserves and reconciles existing verified profile content.
3. **Verification**:
   - Audits actual changes against the approved manifest; unlisted changes stop dependent cleanup.
   - Verifies approved agent/command discovery and links/anchors against the selected destination tree, adapting absent topic/adoption routes while retaining AGENTS fallback.
   - Reports executed and unexecuted checks accurately. Drift, scope changes or new conflicts require renewed approval; no automatic rollback or destructive cleanup on failure.

---

## 5. Post-Merge Profiling

Following the apply phase, work from the approved target execution root:
1. Open `docs/repository-profile.md` and record verified project facts from manifests. Inspect that root and applicable ancestor `global.json` before proposing SDK changes; reference-only adoption preserves the target policy. The sample's `10.0.201` / `latestPatch` stable policy is not a brownfield upgrade instruction or proof that other patches were tested.
2. Follow [`post-adoption-checklist.md`](./post-adoption-checklist.md) to confirm complete verification.
3. Run only discovered applicable and authorized target README/script commands, preserving runtime, routes, response/event/job contracts and schema authority. The [sample README](../../examples/Baseline.Sample/README.md) separately owns its five verification profiles; neither its commands nor `examples/**` transfer as application instructions.
4. Adoption itself performs no Git mutations. Any later staging/commit requires separate explicit authorization and exact reviewed file paths, not broad directory staging.
