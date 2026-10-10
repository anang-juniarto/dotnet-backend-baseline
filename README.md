# Universal Backend .NET Engineering Baseline

> **Repository Type:** Portable AI engineering reference baseline  
> **Baseline Version:** `2.0.0` (see [`BASELINE.md`](./BASELINE.md))  
> **Reference Assets:** Non-executable governance, standards, agent configuration, and blueprints
> **Optional Executable Example:** `examples/Baseline.Sample` — isolated local in-memory demo; no external stack certified
> **Adoption Lifecycle:** AI-Assisted Plan $\rightarrow$ Review $\rightarrow$ Implement ([`docs/adoption/ai-implementation-workflow.md`](./docs/adoption/ai-implementation-workflow.md))

**Source-only adoption:** Keep `docs/adoption/` and the three adoption commands in this baseline, not in new destinations. Follow the [canonical lifecycle](./docs/adoption/ai-implementation-workflow.md) for approved, ownership/drift-checked legacy cleanup after verification. Adapt destination README/index/manifest copies to omit missing local adoption links; retain these links in source. Permanent capability fallback is in `AGENTS.md` section 2.

**Minimal adoption default:** New and existing projects both use `core-reference`; `full-reference` is explicit opt-in. [BASELINE.md](./BASELINE.md) is the canonical exact-file allowlist/exclusion policy: no bulk recursive copy/mirror, no Git metadata or plan/state payload import, and no unlisted files. Preserve destination README identity, application CHANGELOG history and root license; optional CONTRIBUTING changes must fit the target workflow. Retain the MIT notice at an approved attribution location. Plans verify path overlap, containment, drift, duplicate mappings and dependency closure, then audit actual changes against the manifest. Exclusions do not delete existing target artifacts; later authorized generation is separate. Older plans require re-review, not automatic target migration.

This portable reference defaults approved greenfield applications to **.NET 10, Clean Architecture and MediatR CQRS**, using `src/Core` (Domain/Application), modular provider/capability assemblies under `src/Infrastructure`, and `src/Presentation` (WebApi and separately selected Grpc). The [canonical selectable schema](./docs/architecture/repository-map.md) follows the user-owned `gemini-code-1791622287764.txt`; it is a blueprint, not a claim that all modules exist. Start with the [greenfield profile](./docs/architecture/net10-baseline-profile.md), then select only needed capabilities from the [optional-stack catalog](./docs/architecture/optional-stack-catalog.md). Reference scope (`core-reference` by default) is separate from runtime capability selection.

Brownfield reference-only adoption preserves the discovered runtime, architecture, API contracts and packages. Selected-capability additions and incremental migrations require their own approval; missing topic documents retain the `AGENTS.md` discovery fallback. The optional sample demonstrates local in-memory use cases plus compiled modular provider/capability adapters and separate gRPC/realtime transports. Its [verification record](./examples/Baseline.Sample/docs/repository-profile.md) distinguishes unit/in-process/architecture and configuration checks from certification; passing tests do not certify live provider delivery, durable chat/inbox/push or production deployment. Kubernetes, Helm and cloud deployment are not mandatory. `examples/**` is excluded from baseline source transfer; executable target generation requires a separate exact-file manifest.

---

## Choose an entry point

- **Run the example:** [five sample profiles](./examples/Baseline.Sample/README.md), beginning with memory-only. Relational profiles require separately provisioned schema and are not certified database quick-starts.
- **Create a new target:** [new-project adoption](./docs/adoption/new-project.md) → approved exact-file generation → target verification.
- **Adopt into an existing target:** [existing-project adoption](./docs/adoption/existing-project.md), preserving runtime/contracts by default.
- **Maintain this baseline:** [contribution checks](./CONTRIBUTING.md#3-verification--testing) and the [root validation workflow](./.github/workflows/baseline-validation.yml). GitHub-run results must be distinguished from local verification.

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
