# New Project Adoption Playbook

> **Classification:** `[CORE]`  
> **Target Context:** Greenfield / New backend .NET repository  
> **Default Scope:** `full-reference`  

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
/plan-new-project-adoption target="C:\Projects\OrderService" [scope=full-reference]
```

The AI agent will inspect the target and emit a structured adoption plan using [`adoption-plan-template.md`](./adoption-plan-template.md).

### Step 2.2: Review & Implement
Review the emitted plan. Once confirmed, authorize implementation:

```text
/implement-approved-adoption target="C:\Projects\OrderService" [scope=full-reference] approved=true
```

### What Happens During Implementation:
Apply only the exact files selected in the approved scope manifest; topic directories below describe `full-reference`, not mandatory additions to `core-reference`. Reconcile existing files instead of blindly copying over them.
1. **Core Governance Setup**:
   - Copies `AGENTS.md` (root operational contract).
   - Adds or merges approved `.kilo/kilo.json`, agent definitions, and non-adoption commands only; preserves existing settings and verifies approved-effective `default_agent` and collision renames.
   - Copies universal standards (`docs/standards/`).
2. **Profile Templates Setup**:
   - Copies `docs/repository-profile-template.md` as `docs/repository-profile.md`.
   - Copies topic directories (`architecture/`, `engineering/`, `api/`, `data/`, `security/`, `operations/`, `releases/`, `adr/`, `collaboration/`).
   - All copied profile documents start in `Draft` status with placeholders intact.
3. **Deferred Artifacts Excluded**:
   - Folders like `docs/api/endpoints/` and `docs/assets/diagrams/` are intentionally left uncreated until real endpoints/diagrams exist.

---

## 3. Post-Adoption Configuration Steps

Once the baseline files are copied into the target project, complete the following steps in the target repository:

### Step 3.1: Create Solution & Projects
Create your .NET solution and projects using standard .NET CLI commands:
```powershell
dotnet new sln -n OrderService
dotnet new webapi -n OrderService.Api -o src/OrderService.Api
dotnet sln add src/OrderService.Api/OrderService.Api.csproj
```

### Step 3.2: Populate `docs/repository-profile.md`
Edit `docs/repository-profile.md` with the verified facts established in Step 3.1:
- Fill in Target Framework (e.g., `net10.0`, `net9.0`).
- Record project structure in `docs/architecture/repository-map.md`.
- Record verified build and test commands (`dotnet build`, `dotnet test`).

### Step 3.3: Select Architecture Deliberately
Do not blindly add Clean Architecture or MediatR unless authorized by design:
- For simple services: Use Minimal APIs and straightforward service layers.
- For complex domains: Author an ADR under `docs/adr/` before introducing Clean Architecture, MediatR, or extra project boundaries.

---

## 4. Verification

Run the verification checklist:
- Follow [`post-adoption-checklist.md`](./post-adoption-checklist.md) to confirm all links, Kilo agents, and build commands operate cleanly.
