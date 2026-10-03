# Graphify Structural Analysis & Operational Standards

> **Classification:** `[CORE]`  
> **Status:** Normative Standard  
> **Applicability:** Applies to structural repository analysis, dependency graph generation, and cross-module impact tracing.

---

## 1. Role & Operating Philosophy

Graphify provides automated, machine-readable dependency and call-graph analysis. In this baseline:
- **On-Demand Structural Indexing**: Graphify indexing can be refreshed on-demand via `/refresh-graph` or optionally following project setup when solution/project files exist.
- **Optional AI Reading**: Graph data is **NOT loaded into AI context by default**. AI agents read graph summaries only on-demand when full structural discovery is demonstrably necessary.
- **Generated Evidence Only**: Graph output is generated diagnostic evidence (`[GENERATED]`); it does not supersede project manifests (`.csproj`) or architecture decisions.

---

## 2. Execution & Cache Location

- **Storage Location**: All Graphify index artifacts are written exclusively to `.kilo/cache/graphify/` at the repository root.
- **Git Exclusions**: `.kilo/cache/` is strictly ignored by `.kilo/.gitignore` and must never be committed to source control.
- **Output Artifacts**:
  - `summary.json`: Lightweight graph metadata (status, node count, edge count, manifest fingerprint, generated timestamp).
  - `graphify-out/graph.json`: Full structural AST graph produced by code-only extraction (`graphify extract <root> --code-only`).

---

## 3. Availability & Fallback Policy

1. **No Automatic Installation**: Graphify is never installed automatically during adoption or task execution. Tool additions require explicit user authorization.
2. **Graceful Degradation**:
   - If `graphify` is not detected in the environment (`Get-Command graphify`), adoption records `Graph status: Unavailable` and proceeds cleanly.
   - If execution exits with a non-zero code or error, status is recorded truthfully as `failed` with exact exit code.
   - Fallback analysis is performed using standard .NET manifests (`.sln`, `ProjectReference`, `<PackageReference>`) and targeted symbol search.
3. **Staleness Detection**: Graph cache is marked stale if project manifests (`*.csproj`, `*.sln`) produce a different fingerprint than `ManifestFingerprint` in `summary.json`. When stale, agents propose `/refresh-graph`.

---

## 4. AI Reading Rules (Token Efficiency)

AI agents adhere to the following reading protocol:

```text
Task Requiring Structural Understanding
    ↓
Can manifests (.sln, .csproj ProjectReference) answer the dependency?
    ├── YES → Use manifest directly (Lowest token cost)
    └── NO  → Check if .kilo/cache/graphify/summary.json exists
                 ├── Read summary.json (Counts, cycles, module boundaries)
                 └── Inspect only the specific subgraph/neighborhood for affected modules
```

- **Prohibition**: AI agents are FORBIDDEN from reading the complete raw `graph.json` or dumping entire graph representations into prompt context.
- **Agent Ownership**:
  - `team-lead`: Decides when structural graph inspection is justified.
  - `architect-reviewer`: Reads `summary.json` and targeted module subgraphs to assess coupling and cycle risks.
  - Implementation agents (`backend-feature`, `database-engineer`): Receive concise, verified findings from `team-lead` rather than reading graph files directly.

---

## 5. Commands

- `/refresh-graph`: Regenerates `.kilo/cache/graphify/` index for the active repository.
- `/analyze-graph`: Triggers `architect-reviewer` to evaluate structural dependencies using graph evidence.
