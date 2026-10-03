# Universal Backend .NET Engineering Baseline

> **Repository Type:** Portable AI engineering reference baseline  
> **Baseline Version:** `2.0.0` (see [`BASELINE.md`](./BASELINE.md))  
> **Executable Application:** No (governance, standards, agent configuration, and blueprints)  
> **Adoption Lifecycle:** AI-Assisted Plan $\rightarrow$ Review $\rightarrow$ Implement ([`docs/adoption/ai-implementation-workflow.md`](./docs/adoption/ai-implementation-workflow.md))

**Source-only adoption:** Keep `docs/adoption/` and the three adoption commands in this baseline, not in new destinations. Follow the [canonical lifecycle](./docs/adoption/ai-implementation-workflow.md) for approved, ownership/drift-checked legacy cleanup after verification. Adapt destination README/index/manifest copies to omit missing local adoption links; retain these links in source. Permanent capability fallback is in `AGENTS.md` section 2.

Welcome to the universal backend engineering baseline. This repository provides a portable, token-efficient foundation for enterprise backend services, multi-agent AI orchestration, and software governance.

---

## 1. Quick-Start Discovery Checklist

Before editing code or configuring a target repository, discover and verify the actual repository profile:

1. **Verify Runtime & SDK**: Inspect installed .NET SDK (`dotnet --info`) and `.csproj` `<TargetFramework>`.
2. **Discover Solution & Projects**: Locate the solution file (`.sln` or `.slnx`) and project dependency graph.
3. **Inspect Active Commands**: Check repository commands in `.kilo/command/`, `Makefile`, or CI pipelines for `build`, `test`, `lint`, and `run`.
4. **Identify Contracts & Schema Authority**: Determine if database schemas are code-first, database-first, or externally managed (`docs/data/schema-ownership.md`).
5. **AI Guardrails**: AI agents MUST read [`AGENTS.md`](./AGENTS.md) before performing any tasks.

---

## 2. Documentation Map

| Category | Primary Entry Point | Description |
|---|---|---|
| **Baseline Manifest** | [`BASELINE.md`](./BASELINE.md) | Scope definitions, artifact classifications, and non-goals |
| **Overview & Index** | [`docs/README.md`](./docs/README.md) | Reading paths, document authority, and lifecycle status definitions |
| **Adoption Playbooks** | [`docs/adoption/`](./docs/adoption/README.md) | Guides for new project bootstrap and existing project integration |
| **Engineering Standards** | [`docs/standards/`](./docs/standards/README.md) | Universal, applicability-driven enterprise backend engineering standards |
| **Getting Started** | [`docs/getting-started/`](./docs/getting-started/prerequisites.md) | Prerequisites, local setup, configuration, and first request |
| **Architecture** | [`docs/architecture/`](./docs/architecture/overview.md) | System context, repository map, and domain glossary |
| **Decisions (ADR)** | [`docs/adr/`](./docs/adr/README.md) | Architecture Decision Records and ADR authoring template |
| **Engineering** | [`docs/engineering/`](./docs/engineering/coding-standards.md) | Coding standards, feature creation playbook, and test strategy |
| **API Governance** | [`docs/api/`](./docs/api/README.md) | API conventions, authentication, error contracts, and endpoint specifications |
| **Data & Persistence** | [`docs/data/`](./docs/data/database-overview.md) | Schema authority, database overview, and schema migration process |
| **Security** | [`docs/security/`](./docs/security/overview.md) | Threat boundaries, authorization policies, and secrets management |
| **Operations** | [`docs/operations/`](./docs/operations/deployment.md) | Configuration, deployment safety, observability, and runbooks |
| **Collaboration** | [`docs/collaboration/`](./docs/collaboration/README.md) | Feature specifications and developer handoff records |
| **Releases** | [`docs/releases/`](./docs/releases/unreleased.md) | Unreleased staging log and release notes template |

---

## 3. Contribution & AI Guardrails

- For human contributors: Follow [`CONTRIBUTING.md`](./CONTRIBUTING.md) for branch naming, coding style, pull requests, and testing standards.
- For AI agents: Adhere to [`AGENTS.md`](./AGENTS.md) and the routed standards in [`docs/standards/`](./docs/standards/README.md).
- Release tracking: See [`CHANGELOG.md`](./CHANGELOG.md) for versioned release history.
