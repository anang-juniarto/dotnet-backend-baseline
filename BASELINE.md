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

Both new and existing projects default to `core-reference`. `full-reference` is explicit opt-in for needed additional references, never permission to transfer an entire tree. The table defines discovery candidates, not an executable copy list. Every transferred or adapted file requires an approved exact-file manifest entry; unlisted files are not imported. Directory names and globs never authorize bulk recursive copy or mirror operations.

| Scope | Included Baseline Files & Folders | Excluded Files | Recommended For |
|---|---|---|---|
| **`core-reference`** | `BASELINE.md` (adapted), `.baseline-version`, `AGENTS.md` (reconciled reference block), selected permanent `.kilo/` config, agents, commands and Graphify helper, `docs/standards/`, `docs/README.md` (adapted index), `docs/repository-profile-template.md`, `docs/engineering/change-delivery-contract.md`, `docs/operations/graphify.md`, and essential blueprints (`docs/adr/template.md`, `docs/collaboration/feature-spec-template.md`, `docs/collaboration/developer-handoff-template.md`, `docs/releases/release-template.md`) | Optional profile topic docs (capability fallback applies), plus all universal exclusions | Default for new and existing projects |
| **`full-reference`** | Core candidates plus needed files from `docs/getting-started/`, `docs/architecture/`, `docs/engineering/`, `docs/api/`, `docs/data/`, `docs/security/`, `docs/operations/`, `docs/releases/`, and additional templates; root documents subject to preservation rules below | All universal exclusions; baseline identity/history is not application content | Explicit opt-in for either project type |

Select permanent agents/commands with their dependencies, including `.kilo/scripts/graphify-index.ps1`, `/refresh-graph`, `/analyze-graph`, and Graphify guidance as a coherent set when selected. Core blueprints above are reusable references; additional templates require a concrete need. Do not automatically create sample ADR, feature-spec, handoff, or release instances. Profile initialization from the template is a separate exact approved destination entry, not an implicit generation step.

### Source-Only Adoption Assets
Both scopes copy only permanent artifacts approved by exact destination path. Keep `docs/adoption/` and `.kilo/command/plan-new-project-adoption.md`, `.kilo/command/plan-existing-project-adoption.md`, and `.kilo/command/implement-approved-adoption.md` in this source baseline; never copy them to a new destination. `.kilo/` inclusion above excludes these three commands and universal exclusions below.

For an existing destination, removal of legacy adoption assets requires explicit per-file cleanup approval, baseline ownership evidence, and unchanged/unmodified content checks. A path or baseline marker alone does not establish ownership. Preserve user files, customizations, drifted files, and uncertain ownership; report conflicts. Never recursively delete an unverified directory.

Follow the source [implementation lifecycle](./docs/adoption/ai-implementation-workflow.md): approved plan → merge/adapt permanent files → verify → authorized cleanup → final link/discovery verification. In destination copies, adapt this manifest, README files, indexes, agents, and commands to remove local adoption links and instructions for excluded files, or point to a verified accessible source location. Keep these source links here. Permanent capability fallback is defined in `AGENTS.md` section 2, not in adoption assets.

### Universal Exclusions
Neither scope imports the following source payloads. Apply exclusions to both source paths and mapped destination paths before approval and again before execution; renaming cannot bypass an exclusion:
- Git metadata: any `.git` file or directory, including nested repositories and worktree pointer files.
- Source-only adoption assets listed above and `.kilo/plans/`.
- `.kilo/agent-manager.json`, `.kilo/worktrees/`, `.kilo/cache/`, `.kilo/generated/`.
- Build/cache outputs: `**/bin/`, `**/obj/`, `**/TestResults/`, `**/coverage/`, `**/artifacts/`, `node_modules/` at any depth, and `*.zip` at any depth.
- Deferred contract fixtures: `docs/api/endpoints/`, `docs/api/examples/`, `docs/api/postman/collections/`, `docs/assets/diagrams/`.
- Secrets and machine-local configuration; unselected `.kilo/` content is never imported automatically.

Exclusion means **do not import**, not delete or overwrite similarly named existing destination artifacts. Later target-generated build/cache/Graphify output or real endpoint documentation requires a separately authorized operation; it is not source payload transfer or implicit adoption scaffolding.

### Root Documents & Attribution
- Preserve destination `README.md`; any approved adaptation describes the target, not the baseline's identity.
- Preserve destination `CHANGELOG.md`; do not import baseline release history as application history. Track baseline identity through `.baseline-version` and the approved manifest.
- `CONTRIBUTING.md` is optional: preserve or explicitly merge only guidance compatible with the target workflow.
- Adapt destination `BASELINE.md`, indexes, agents, and commands to the selected tree, including omitted topic docs; no broken local routes to excluded assets. Preserve source-baseline adoption links here.
- Read source `LICENSE` before transfer. Its MIT terms require the copyright and permission notice in copies or substantial portions. Record an exact approved attribution location preserving the notice (including `Copyright (c) 2026 Anang Juniarto`; retain the full license text). Do not replace the target root license or imply the entire application is MIT-licensed. Unresolved attribution location or license decisions are `CONFLICT`/`DEFER` before related transfer; this baseline's `LICENSE` remains unchanged.

### Exact-File Approval & Audit
Normalize and verify absolute source/destination roots; reject equal or ancestor/descendant roots. Resolve aliases/junctions when verifiable; uncertain physical paths block writes. Do not automatically follow symlinks/reparse points during discovery. Verify every mapped file is contained in the intended root, rejects traversal, and has no duplicate destination under the target filesystem's path comparison rules.

Record source/target content state, scope basis, concrete need, and `ADD`/`MERGE`/`SKIP`/`CONFLICT`/`DEFER` per exact file. Inventory excluded/deferred candidates and reasons. Before writes, recheck paths, exclusions, source/target drift, attribution and dependency closure against the approved manifest; scope changes or new conflicts require renewed approval. Audit actual changes against that manifest afterward, including adaptations and generated profile files. Verify links/anchors and agent/command discovery against the selected destination tree, not just the source. Preserve unrelated work and do not automatically roll back or destructively clean up failed adoption.

These rules apply to future adoption plans; re-review older plans before execution. No automatic migration or target cleanup follows from this policy change.

---

## 3. Artifact Classifications

Every document in this repository carries an architectural classification:

- `[CORE]`: Universal authority, safety gate, or operational contract. Maintained consistently across projects.
- `[STANDARD]`: Universal applicability-driven normative standard. Read on-demand based on task triggers; never loaded in full by default.
- `[PROFILE]`: Target repository fact or convention. Contains placeholders in this baseline and MUST be replaced with verified project facts upon adoption.
- `[TEMPLATE]`: Reusable blueprint (e.g. ADRs, specs, handoffs). Core-listed blueprints are reference candidates; additional templates require a concrete need. Creating an instance requires separate exact approval.
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
   - Command: `/plan-new-project-adoption target="<path>" [scope=core-reference]`
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
