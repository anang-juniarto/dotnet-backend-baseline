# Baseline Adoption Router & Playbooks

> **Classification:** `[CORE]`  
> **Status:** Active  
> **Authority:** Governance guide for applying this baseline to new or existing backend .NET repositories.

This source-only directory provides adoption playbooks and checklists. It and `.kilo/command/plan-new-project-adoption.md`, `.kilo/command/plan-existing-project-adoption.md`, and `.kilo/command/implement-approved-adoption.md` stay in the baseline source and are excluded from both destination scopes. Follow the [canonical lifecycle](./ai-implementation-workflow.md) for exact approved cleanup of verified baseline-owned legacy files only after permanent verification, then final link/discovery verification. Preserve user files, customizations, and ownership/drift conflicts; never recursively delete an unverified directory. Adapt destination gateways without removing source links. Permanent fallback is in [`AGENTS.md`](../../AGENTS.md) section 2.

---

## 1. Playbook Selection

Both new and existing projects default to `core-reference`. `full-reference` is explicit opt-in, not a bulk folder transfer. [BASELINE.md](../../BASELINE.md) defines canonical exclusions and root-document/license preservation. Every imported/adapted file requires an exact approved source/destination mapping with need, scope basis and state; no recursive copy/mirror. Preserve target identity/history/root license and retain the source MIT notice at an approved attribution location. Unresolved path overlap/aliases, drift, duplicate destinations or attribution block dependent writes. Audit actual changes and selected-tree links/discovery; exclusions do not delete existing target files or prohibit separately authorized later generation.

Choose the playbook that matches your adoption target:

| Target Context | Playbook | Planning Command | Implementation Command | Recommended Scope |
|---|---|---|---|---|
| **Greenfield / New Project** | [`new-project.md`](./new-project.md) | `/plan-new-project-adoption` | `/implement-approved-adoption` | `core-reference` |
| **Brownfield / Existing Project** | [`existing-project.md`](./existing-project.md) | `/plan-existing-project-adoption` | `/implement-approved-adoption` | `core-reference` |

---

## 2. Profile, Mode & Capability Selection

Reference scope and executable stack selection are independent approval axes. `full-reference` does not enable integrations; selecting a stack does not authorize importing whole topic folders. Reference adoption alone never generates an application.

- **New target:** separately approved generation defaults to .NET 10, Core Domain/Application MediatR CQRS feature slices, modular Infrastructure provider/capability assemblies, and Presentation WebApi controllers (optional Grpc), with Unit/Integration/Architecture test projects. Selected hosts own composition-root wiring; no separate read database, event sourcing or speculative wrappers are required.
- **Existing target:** choose `existing-reference` (guidance only), `existing-capabilities` (selected adapters within current boundaries), or `existing-migration` (separately approved incremental runtime/architecture migration). Preserve verified runtime, contracts, schemas and tooling unless the exact migration approves changes.
- **Optional stacks:** use the [capability matrix](./capability-matrix.md) and [catalog](../architecture/optional-stack-catalog.md). All integrations default off; include only approved dependencies and their acceptance checks. Document proposed versus verified evidence.
- **Source examples:** `examples/**`, including `examples/Baseline.Sample`, never transfer as reference payload in either scope. A separately approved exact generated-file manifest may use a reviewed blueprint, not recursive source copying. The isolated in-memory sample is not external-integration certification.

## 3. Supporting Adoption Governance

- **AI Implementation Lifecycle**: [`ai-implementation-workflow.md`](./ai-implementation-workflow.md)  
  *5-phase transparent adoption lifecycle (Discovery -> Planning -> Review -> Implementation -> Verification).*
- **Adoption Plan Blueprint**: [`adoption-plan-template.md`](./adoption-plan-template.md)  
  *Standard structured schema for adoption plans emitted by planning commands.*
- **Conflict Resolution Matrix**: [`conflict-resolution.md`](./conflict-resolution.md)  
  *Precedence rules for resolving collisions between baseline artifacts and existing project contracts, configs, or docs.*
- **Capability & Fallback Matrix**: [`capability-matrix.md`](./capability-matrix.md)  
  *Source planning map for capability parity; permanent fallback authority is `AGENTS.md` section 2.*
- **Post-Adoption Verification Checklist**: [`post-adoption-checklist.md`](./post-adoption-checklist.md)  
  *Step-by-step checklist to confirm tool registration, link resolution, and test executions after adoption.*
- **Repository Profile Template**: [`../repository-profile-template.md`](../repository-profile-template.md)  
  *Template copied to `docs/repository-profile.md` in the target repository to anchor verified facts.*

---

## 4. Core Precedence Rules

1. **Target Authority Wins**: Never allow generic baseline templates to overwrite active published contracts, live schemas, or existing build/test scripts.
2. **Plan Before Implementation**: Adoption always starts with a read-only planning phase (`/plan-*`) before any target files are touched. Implementation requires explicit user approval.
3. **No Phantom Elements**: Merged agent prompts must discover active libraries from manifests (`.csproj`) rather than assuming package availability.
