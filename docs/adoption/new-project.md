# New Project Adoption Playbook

> **Classification:** `[CORE]`  
> **Target Context:** Greenfield / New backend .NET repository  
> **Default Scope:** `core-reference` (`full-reference` is explicit opt-in)

This source-only playbook follows the [canonical lifecycle](./ai-implementation-workflow.md) and [scope manifest](../../BASELINE.md). Neither scope copies `docs/adoption/` or the three adoption commands into a new destination. Keep source guidance intact; adapt destination README/index/manifest links and use `AGENTS.md` section 2 for permanent fallback. Existing files in a scaffolded target require collision/drift checks and preservation; legacy cleanup, if any, requires the lifecycle's exact approvals and pre/post verification.

---

## 1. Prerequisites & Preparation

1. Have an empty or newly scaffolded target directory ready (and optionally initialize Git if using version control: `git init`).
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

### Step 3.1: Create Solution & Projects
Only under separate authorization, create solution/projects with verified SDK options. These illustrative commands are not adoption actions:
```powershell
dotnet new sln -n OrderService
dotnet new webapi -n OrderService.Api -o src/OrderService.Api
dotnet sln add src/OrderService.Api/OrderService.Api.csproj
```

### Step 3.2: Populate `docs/repository-profile.md`
Edit `docs/repository-profile.md` with the verified facts established in Step 3.1:
- Fill in Target Framework (e.g., `net10.0`, `net9.0`).
- Record project structure in the approved profile; update `docs/architecture/repository-map.md` only if selected/existing, otherwise use permanent fallback.
- Record verified build and test commands (`dotnet build`, `dotnet test`).

### Step 3.3: Select Architecture Deliberately
Do not blindly add Clean Architecture or MediatR unless authorized by design:
- For simple services: Use Minimal APIs and straightforward service layers.
- For complex domains: Author an ADR under `docs/adr/` before introducing Clean Architecture, MediatR, or extra project boundaries.

---

## 4. Verification

Run the verification checklist:
- Follow [`post-adoption-checklist.md`](./post-adoption-checklist.md) to confirm all links, Kilo agents, and build commands operate cleanly.
