# Baseline Merge Conflict Resolution Matrix

> **Classification:** `[CORE]`  
> **Status:** Normative Guidance  
> **Authority:** Precedence rules for resolving collisions between baseline artifacts and existing project configurations.

This source-only guide follows the [canonical lifecycle](./ai-implementation-workflow.md). Existing destination cleanup is a separate exact per-file approval after successful permanent verification, with ownership and drift/customization rechecks before deletion and final link/discovery verification afterward. Preserve user-owned, customized, and uncertain files as conflicts; never remove source assets or recursively delete an unverified folder.

When merging the Universal Backend .NET Engineering Baseline into an existing repository, apply the following deterministic resolution rules:

---

## 1. Governance & Authority Precedence

In any conflict, resolve technical authority in this order:
1. **Applicable Law, Regulations, and Security Policies** (non-waivable).
2. **Effective Published Contracts** (live APIs, integration events, database schemas).
3. **Accepted Project ADRs** (formal decisions under target `docs/adr/`).
4. **Existing Target-Specific Guardrails** in target `AGENTS.md`.
5. **Universal Baseline Standards** (`docs/standards/`).
6. **Generic Baseline Templates & Examples**.

A baseline template or illustrative pattern MUST NEVER override an effective, published project contract.

---

## 2. Collision Resolution Matrix

| Collision Category | Typical Scenario | Precedence Rule | Resolution Action |
|---|---|---|---|
| **Path Safety / Mapping** | Equal or ancestor/descendant roots, unresolved aliases/junctions, traversal or duplicate destinations | Verified physical containment and unique destinations are mandatory. | Block before writes; do not automatically follow links/reparse points. Reconcile and obtain renewed exact approval. |
| **Excluded Payload** | Source or mapped destination matches a canonical exclusion, even after rename | [BASELINE.md](../../BASELINE.md#universal-exclusions) applies in both scopes. | Do not import; inventory reason. Preserve pre-existing target artifacts; exclusion is not cleanup authorization. Later authorized generation is separate. |
| **Root Identity / History** | Baseline README, CHANGELOG or CONTRIBUTING conflicts with target | Target identity/history/workflow wins. | Preserve README and CHANGELOG; no baseline release-history import. Only exact approved target-specific README/optional CONTRIBUTING adaptations; track baseline identity separately. |
| **License / Attribution** | Target root license differs or attribution location is unknown | Preserve target license and source MIT copyright/permission notice. | Read LICENSE, approve exact attribution location retaining full notice; do not imply application-wide relicensing. Unresolved decision is CONFLICT/DEFER before related transfer. |
| **Approval Drift** | Source/target content, scope or dependency changes after approval | Exact approved manifest and recorded state are required. | Stop dependent writes and refresh approval; no overwrite, unlisted writes, automatic rollback or destructive cleanup. Audit actual changes against manifest. |
| **Root AI Contract** | Target already has an `AGENTS.md` | Target specific rules win; universal guardrails merge. | Inject baseline universal guardrails (instruction precedence, anti-hallucination, evidence reporting, discovery) without deleting target-specific guidelines. |
| **Kilo Configuration** | Target has existing `kilo.json` or `kilo.jsonc` | Stricter permissions and existing settings win. | Merge only approved missing settings; preserve existing `default_agent` and compaction values unless an explicit approved change applies. Validate approved-effective settings, including renamed agents. Do NOT delete existing MCP servers, plugins, or custom permissions. |
| **Agent Name Collision** | Target already defines `code-reviewer.md` or `team-lead.md` | Target agent takes precedence. | Append `-baseline` suffix to the imported agent (e.g. `code-reviewer-baseline.md`) or review and merge responsibilities. |
| **Command Collision** | Target already defines `/review-code` | Target command takes precedence. | Rename imported command to `/baseline-review-code` or preserve target command. |
| **Architectural Style** | Target uses Vertical Slice or Minimal CRUD, baseline standards mention Clean Architecture | Verified target architecture wins. | Standards are applicability-driven. Do NOT force Clean Architecture or MediatR. Record target architecture in `docs/repository-profile.md`. |
| **API Response Format** | Target returns custom `{ "data": ... }`, baseline mentions RFC 9457 Problem Details | Effective published contract wins. | Preserve target response envelope. Document target format in `docs/api/response-and-error-contracts.md`. |
| **Schema Governance** | Target uses Database-First or Flyway, baseline mentions EF Core Migrations | Verified target schema owner wins. | Set schema authority to Database-First / External in `docs/data/schema-ownership.md`. Do NOT generate EF Core migrations. |
| **Documentation Paths** | Target maintains docs in `wiki/` or `documentation/` | Target convention wins. | Do not move files. Map documentation roles in `docs/README.md` to the target's existing directory structure. |
| **Package Dependencies** | Baseline agent prompt mentions Dapper, Serilog, or OpenTelemetry, but target does not install them | Target manifests (`.csproj`) win. | Baseline mentions are conditional examples. Do NOT install packages or add dependencies without explicit task authorization. |
| **Global Kilo Overrides** | `~/.config/kilo/` has conflicting global rules | Project configuration wins over global. | Ensure `.kilo/` project files override global defaults where appropriate. |

---

## 3. Decision Approval Protocol

When `/plan-existing-project-adoption` generates an adoption plan, it records a `Decision Key` for every file collision (e.g. `merge-agents-contract`, `merge-kilo-config`, `agent-collision-<name>`). 

To authorize implementation, review the plan and pass approval to `/implement-approved-adoption`:
```text
/implement-approved-adoption target="C:\Projects\ExistingApi" [scope=core-reference] approved=true
```
If an unapproved collision remains without an explicit user direction, the AI agent will halt safely before any target files are modified.
