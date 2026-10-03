# Universal Backend .NET Engineering Baseline

> **Baseline Version:** `2.0.0`  
> **Repository Type:** Portable AI engineering reference baseline  
> **Executable Application:** No (governance, standards, agent configuration, and blueprints)  
> **Adoption Lifecycle:** AI-Assisted Plan $\rightarrow$ Review $\rightarrow$ Implement ([`docs/adoption/ai-implementation-workflow.md`](./docs/adoption/ai-implementation-workflow.md))

---

## 1. Purpose & Scope

This baseline provides a portable, token-efficient foundation for enterprise backend engineering and autonomous AI agent workflows. It standardizes:
- AI agent authority, discovery, safety gates, and truthful reporting ([`AGENTS.md`](./AGENTS.md)).
- Modular, on-demand normative standards ([`docs/standards/`](./docs/standards/README.md)).
- Pre-configured multi-agent orchestration and commands ([`.kilo/`](./.kilo/kilo.json)).
- Structured project profiling and documentation roles ([`docs/`](./docs/README.md)).
- Coordinated change delivery across Feature, API, Database, Config, and Tests ([`docs/engineering/change-delivery-contract.md`](./docs/engineering/change-delivery-contract.md)).
- Post-adoption structural indexing via Graphify with on-demand, token-efficient reading ([`docs/operations/graphify.md`](./docs/operations/graphify.md)).

**Explicit Non-Goal:** This baseline does NOT prescribe or assume a specific .NET runtime version, application architecture (Clean Architecture vs. Vertical Slice vs. CRUD), ORM/database, test runner, container orchestrator, cloud provider, or CI vendor. All technology-specific patterns are active ONLY when confirmed by target repository evidence.

---

## 2. Supported Reference Scopes

The baseline defines two clean reference scopes:

| Scope | Included Baseline Files & Folders | Excluded Files | Recommended For |
|---|---|---|---|
| **`core-reference`** | `BASELINE.md`, `.baseline-version`, `AGENTS.md` (reference block merge), `.kilo/` (config, agents, commands, Graphify helper), `docs/standards/`, `docs/README.md` (adapted index), `docs/repository-profile-template.md`, `docs/engineering/change-delivery-contract.md`, `docs/operations/graphify.md`, and essential collaboration blueprints (`docs/adr/template.md`, `docs/collaboration/feature-spec-template.md`, `docs/collaboration/developer-handoff-template.md`, `docs/releases/release-template.md`) | Profile topic docs (handled via capability fallbacks) | Existing project merge |
| **`full-reference`** | `core-reference` + `README.md`, `CONTRIBUTING.md`, `CHANGELOG.md`, `docs/getting-started/`, `docs/architecture/`, `docs/engineering/`, `docs/api/`, `docs/data/`, `docs/security/`, `docs/operations/`, `docs/releases/`, and templates | Internal cache and deferred contract fixtures | New project initialization |

### Source-Only Adoption Assets
Both scopes copy only permanent artifacts approved by exact destination path. Keep `docs/adoption/` and `.kilo/command/plan-new-project-adoption.md`, `.kilo/command/plan-existing-project-adoption.md`, and `.kilo/command/implement-approved-adoption.md` in this source baseline; never copy them to a new destination. `.kilo/` inclusion above excludes these three commands and universal exclusions below.

For an existing destination, removal of legacy adoption assets requires explicit per-file cleanup approval, baseline ownership evidence, and unchanged/unmodified content checks. A path or baseline marker alone does not establish ownership. Preserve user files, customizations, drifted files, and uncertain ownership; report conflicts. Never recursively delete an unverified directory.

Follow the source [implementation lifecycle](./docs/adoption/ai-implementation-workflow.md): approved plan → merge/adapt permanent files → verify → authorized cleanup → final link/discovery verification. In destination copies, adapt this manifest, README files, indexes, agents, and commands to remove local adoption links and instructions for excluded files, or point to a verified accessible source location. Keep these source links here. Permanent capability fallback is defined in `AGENTS.md` section 2, not in adoption assets.

### Universal Exclusions
Under no scope will the following artifacts be copied or generated:
- `.kilo/agent-manager.json`, `.kilo/worktrees/`, `.kilo/cache/`, `.kilo/generated/`
- Build/cache outputs: `**/bin/`, `**/obj/`, `**/TestResults/`, `**/coverage/`, `**/artifacts/`, `node_modules/`, `*.zip`
- Deferred contract fixtures: `docs/api/endpoints/`, `docs/api/examples/`, `docs/api/postman/collections/`, `docs/assets/diagrams/`

---

## 3. Artifact Classifications

Every document in this repository carries an architectural classification:

- `[CORE]`: Universal authority, safety gate, or operational contract. Maintained consistently across projects.
- `[STANDARD]`: Universal applicability-driven normative standard. Read on-demand based on task triggers; never loaded in full by default.
- `[PROFILE]`: Target repository fact or convention. Contains placeholders in this baseline and MUST be replaced with verified project facts upon adoption.
- `[TEMPLATE]`: Reusable blueprint (e.g. ADRs, specs, handoffs). Copied only when a concrete instance is needed.
- `[DEFERRED]`: Conditional directory/artifact. Intentionally absent until concrete implementation contracts exist.
- `[GENERATED]`: Tool-generated output (e.g. Graphify dependency cache). Not a manually edited authority.

---

## 4. Capability Parity Across Scopes

All AI-assisted engineering capabilities (system analysis, architecture review, backend features, database persistence, testing, code review, ops diagnosis, and structural scanning) remain functional across both `full-reference` and `core-reference` adoptions. If an optional profile document was excluded during brownfield adoption, agents execute using a standardized fallback order:
1. Active target profile documentation (`docs/repository-profile.md` or topic document).
2. Direct inspection of project manifests (`.csproj`), project references, and neighboring code.
3. Canonical universal standards (`docs/standards/`).

The permanent authority is [`AGENTS.md`](./AGENTS.md) section 2. The source-only [`capability mapping`](./docs/adoption/capability-matrix.md) supports adoption planning; destination operation never requires it.

---

## 5. Structural Indexing (Graphify)

- When adopting into a repository containing solution or project files, a Graphify structural scan can be refreshed on-demand via `/refresh-graph`.
- Generated graph summaries are stored in target `.kilo/cache/graphify/` (ignored by version control).
- Graphify is never installed automatically; if missing, adoption records `Graph unavailable` and falls back to manifest discovery without blocking.
- AI agents treat graph data as **on-demand evidence**; they never load raw complete graphs into context by default.
- See [`docs/operations/graphify.md`](./docs/operations/graphify.md) for full operational rules.

---

## 6. Adoption Workflows

Adoption is executed via the transparent AI Plan $\rightarrow$ Review $\rightarrow$ Implement lifecycle:

1. **New Projects**:
   - Command: `/plan-new-project-adoption target="<path>" [scope=full-reference]`
   - Generates a complete adoption plan without editing target files.
   - User reviews the plan and authorizes implementation via `/implement-approved-adoption`.
   - Reference: [`docs/adoption/new-project.md`](./docs/adoption/new-project.md)

2. **Existing Projects**:
   - Command: `/plan-existing-project-adoption target="<path>" [scope=core-reference]`
   - Generates a non-destructive, collision-free adoption plan classifying every file (`ADD`, `MERGE`, `SKIP`, `CONFLICT`, `DEFER`).
   - User reviews the plan, approves conflict resolutions, and authorizes implementation via `/implement-approved-adoption`.
   - Reference: [`docs/adoption/existing-project.md`](./docs/adoption/existing-project.md)
   - Conflict Resolution: [`docs/adoption/conflict-resolution.md`](./docs/adoption/conflict-resolution.md)
   - Verification Checklist: [`docs/adoption/post-adoption-checklist.md`](./docs/adoption/post-adoption-checklist.md)

---

## 7. Non-Negotiable Safety & Governance Gates

Regardless of adoption mode or scope, the following principles remain absolute:
1. **Never Blindly Overwrite**: Existing target repository contracts, instructions, and user worktree modifications are never overwritten without comparison.
2. **Zero Phantom APIs**: Manifests (`.csproj`, lock files) must confirm dependencies before code or configuration references them.
3. **Execution Safety**: Commands, scripts, restores, tests, and database migrations require explicit authorization and environment verification.
4. **Truthful Verification**: Checks are reported strictly as passed, failed, or unexecuted. Never claim a check passed without execution.
