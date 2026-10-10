# New Project Adoption Playbook

> **Classification:** `[CORE]`
> **Status:** Draft | **Owner:** Engineering Leads
> **Last verified:** Not verified | **Evidence:** Reference adoption workflow; destination execution required
> **Target Context:** Greenfield / New backend .NET repository
> **Default Scope:** `core-reference` (`full-reference` is explicit opt-in)

This source-only playbook follows the [canonical lifecycle](./ai-implementation-workflow.md) and [scope manifest](../../BASELINE.md). Neither scope copies `docs/adoption/` or the three adoption commands into a new destination. Keep source guidance intact; adapt destination README/index/manifest links and use `AGENTS.md` section 2 for permanent fallback. Existing files in a scaffolded target require collision/drift checks and preservation; legacy cleanup, if any, requires the lifecycle's exact approvals and pre/post verification.

---

## 1. Prerequisites & Preparation

Minimal path: choose `core-reference`, inspect the target, approve an exact ADD/MERGE manifest, apply only that manifest, and verify destination links/configuration. This transfers guidance only. If executable generation is separately approved, start with one WebApi host, Domain/Application and a single small feature; select `none` for persistence when no durable state is needed, or one explicitly approved store. Add only the required test projects and capability assemblies. `full-reference` expands eligible documentation, not the runtime stack; it never imports `examples/**` or enables all modules.

1. Have an empty or newly scaffolded target directory ready. Git initialization requires separate explicit authorization; adoption does not perform it.
2. Determine the target folder's absolute path (e.g. `C:\Projects\OrderService`).
3. Ensure the target path is a source directory and not an active deployment environment.

---

## 2. Adoption Execution (Plan & Implement)

### Step 2.1: Generate Adoption Plan (Read-Only)
Execute the planning command from this baseline workspace:

```text
/plan-new-project-adoption target="C:\Projects\OrderService" [scope=core-reference]
```

The AI agent will inspect the target and emit a structured adoption plan using [`adoption-plan-template.md`](./adoption-plan-template.md).

### Step 2.2: Review & Implement
Review the emitted plan. Once confirmed, authorize implementation:

```text
/implement-approved-adoption target="C:\Projects\OrderService" [scope=core-reference] approved=true
```

### What Happens During Implementation:
1. **Safety & Selection**: Normalize physical source/target roots, reject overlap and block unresolved aliases/junctions. Do not automatically traverse links/reparse points. Filter [canonical exclusions](../../BASELINE.md#universal-exclusions) on both source and destination mappings, including Git metadata and `.kilo/plans/`. Check containment, duplicate destinations, drift and dependency closure before writes.
2. **Exact Core Allowlist**: Add/merge only approved files with need, scope basis and recorded state: reconciled AGENTS, baseline identity, adapted index/manifest, selected permanent config/agents/commands with Graphify dependencies, standards, profile template, change-delivery contract and core blueprints. Directory names are discovery candidates, never recursive copy/mirror permission; unknown `.kilo/` files do not automatically transfer.
3. **Optional References**: Explicit `scope=full-reference` only expands discovery to needed topic files and additional templates. Each requires exact approval. Profile initialization is a separate manifest entry and stays Draft until verified; no automatic ADR/spec/release instances.
4. **Root Preservation & Attribution**: Preserve target README identity, CHANGELOG history, root license and user content; CONTRIBUTING is optional target-compatible guidance. Read LICENSE and approve an exact attribution location retaining its full MIT notice, without relicensing the application. Unresolved attribution is CONFLICT/DEFER before related transfer.
5. **Audit & Verify**: Compare actual changes with the manifest; validate links/anchors and discovery against the selected destination tree, adapting absent topic/adoption routes to permanent fallback. Drift/scope changes/new conflicts require renewed approval. Exclusions prohibit source import, not existing-target retention or separately authorized later generation. Failure never authorizes automatic rollback or destructive cleanup.

---

## 3. Post-Adoption Configuration Steps

Once the baseline files are copied into the target project, complete the following steps in the target repository:

### Step 3.1: Separately Approve Executable Generation
Reference adoption does not scaffold source. Approve a distinct exact generated-file manifest and dependency/execution decisions before creating projects. Default to .NET 10 (`net10.0`), `src/Core/` Domain/Application with MediatR CQRS feature slices, provider/capability assemblies in `src/Infrastructure/`, and `src/Presentation/` WebApi controllers (optional Grpc). Include `tests/<Project>.UnitTests/`, `tests/<Project>.IntegrationTests/`, and `tests/<Project>.ArchitectureTests/` in the exact generated-file plan. Selected hosts own composition-root wiring; controllers dispatch Application requests through MediatR, not persistence adapters. Use the [greenfield profile](../architecture/net10-baseline-profile.md) as a blueprint, omitting unused abstractions and features.

Record a verified stable SDK, supported package/provider versions, license review and test runner before implementation. MediatR is explicit in this greenfield profile; event sourcing, a separate read database or a datastore is not mandatory. `examples/**`, including `examples/Baseline.Sample`, is excluded from reference transfer; separately approved generation may consult its blueprint but must not recursively copy source examples.

### Step 3.2: Populate `docs/repository-profile.md`
Edit `docs/repository-profile.md` with the verified facts established in Step 3.1:
- Record the selected .NET 10 SDK/framework for new generation and distinguish proposed settings from verified executable evidence.
- Record project structure in the approved profile; update `docs/architecture/repository-map.md` only if selected/existing, otherwise use permanent fallback.
- Record exact target-owned build/test/lint commands and executed results, not generic CLI placeholders. For source-example evaluation, the [sample README](../../examples/Baseline.Sample/README.md) owns all five verification profiles and routes; do not copy its commands or paths into the destination.
- Inspect the target execution root and applicable ancestor `global.json` before changing SDK selection. The sample's `10.0.201` / `latestPatch` / stable-only policy permits patches within `10.0.2xx`, not arbitrary newer SDKs, and does not prove other patches tested. Preserve an existing scaffold's policy until a change is approved.

### Step 3.3: Select Capabilities Independently
Keep `core-reference | full-reference` separate from runtime selections. Use the [capability matrix](./capability-matrix.md) for optional Sentry, Seq, OpenTelemetry, Elasticsearch, MongoDB, SQL Server, MySQL, PostgreSQL, Redis, RabbitMQ, gRPC, Hangfire, SignalR, chat, notification inbox, web push and mobile push. Select one primary authoritative store or `none`; additional stores need explicit ownership and consistency semantics.

All modules default off. Unselected modules contribute no packages, schema, containers, required settings, health probes, hosted processes or network calls. SignalR does not imply Redis; inbox and external push are independent; chat requires identity and durable history. Select push provider/platform and durable dispatch explicitly; no implicit Firebase account, browser service worker or mobile project. Capability documentation is design guidance, not proof of compatible tested adapters. Resolve unsupported combinations as `DEFER`, not silent package/runtime downgrades.

---

## 4. Verification

Run the verification checklist:
- Follow [`post-adoption-checklist.md`](./post-adoption-checklist.md) to confirm all links, Kilo agents, and build commands operate cleanly.
