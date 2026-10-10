# Contributing to Backend Services

This document defines the contribution workflow, quality standards, and verification guidelines for software engineers and AI collaborators.

---

## 1. Task Classification & Workflow

Every contribution should be classified into one of the following modes:

| Mode | Purpose | Required Output |
|---|---|---|
| **Explain / Review** | Code analysis, technical questions, or code reviews | Evidence-based explanation; cite `file:line`; NO file modifications |
| **Diagnose** | Bug triaging, error analysis, root cause analysis | Reproduction steps, hypotheses, and proven root cause; NO unsolicited fixes |
| **Bugfix** | Localized correction of defects | Minimal coherent patch, characterization regression test, release note entry |
| **Feature** | New capabilities or enhancements | Inline scope/invariants/test strategy; separate specification only when risk or coordination warrants it; implementation, relevant tests, docs synchronization, release note |
| **Refactor** | Structural improvements preserving behavior | Regression tests proving zero behavioral change, ADR if architectural |
| **Operations** | Config, deployment, script changes | Environment safety validation, rollback procedure, no exposed secrets |

---

## 2. Contribution Standards

### 2.1 Code & Quality Principles
- **Smallest Coherent Change**: Keep diffs tight, reviewable, and focused. Follow the simple-first and risk-based rules in [`AGENTS.md`](./AGENTS.md); do not introduce boilerplate, abstractions, or architectural layers without a verified need.
- **Dependency Boundaries**: Adhere to layer rules defined in [`docs/architecture/repository-map.md`](./docs/architecture/repository-map.md) and [`docs/engineering/coding-standards.md`](./docs/engineering/coding-standards.md).
- **Asynchronous Code**: Always pass `CancellationToken` through asynchronous I/O methods. Never block async code using `.Result` or `.Wait()`.
- **Zero Secrets**: Never commit tokens, passwords, connection strings, or production URLs. Use user secrets or environment variables.

### 2.2 Documentation Synchronization
When a change modifies API contracts, business invariants, configuration, or operational procedures, the corresponding documentation under `docs/` MUST be updated in the same change.
- Synchronize affected repository-owned documentation without a separate reminder; report no documentation impact when applicable. Do not manually edit generated/vendor artifacts.
- Follow [`C# documentation rules`](./docs/standards/csharp-and-documentation.md#14-c-and-documentation-standards) for touched handwritten declarations.
- Refer to [`docs/README.md`](./docs/README.md) for document authority and ownership.
- Do not invent team owners, verification dates, or test passes. Mark unverified details as `TBD` or `Draft`.

---

## 3. Verification & Testing

Before requesting review or merging:
1. Discover and run the applicable validation commands. Reference assets require documentation/configuration checks. The isolated executable example under [`examples/Baseline.Sample/`](./examples/Baseline.Sample/README.md) additionally requires its documented build/test checks when touched; its passing tests do not certify optional external adapters. In an adopted destination, use that project's verified build/test commands, for example:
   ```bash
   dotnet build
   dotnet test
   ```
2. For API changes, verify request/response serialization against contract expectations.
3. The root [baseline validation workflow](./.github/workflows/baseline-validation.yml) defines default Release build/test/format checks and isolated optional-host compile jobs. The nested sample workflow is reference material, not a second active root workflow. A local passing command is not evidence that GitHub Actions executed.
4. Run default-profile tests independently of optional-profile build outputs. Use separate CI workspaces/jobs; when testing locally, clean/rebuild the default profile before tests after changing build flags. Do not start databases, brokers or exporters merely to validate reference documentation.
5. Record working directory, SDK, exact command, date, result and excluded coverage in the sample profile or target evidence. Keep fixed test totals out of multiple overview documents. NuGet advisory reporting is not a security enforcement gate unless a failure policy is defined.
6. Truthful reporting: Explicitly document which tests were executed and passed, and which were omitted (with clear rationale). Adapt or omit source-only example/workflow routes when this document is merged into an adopted destination.

---

## 4. Git & Release Practices

- **Branching**: Use concise kebab-case branches (e.g., `fix/order-timeout`, `feature/payment-webhook`).
- **Commit Messages**: Follow Conventional Commits:
  ```text
  <type>(<scope>): <short summary in imperative mood>

  [optional detailed body explaining context and rationale]
  ```
  Types: `feat`, `fix`, `refactor`, `perf`, `test`, `docs`, `chore`.
- **Release Staging**: Significant functional, breaking, or operational changes must be recorded under the `[Unreleased]` section of [`CHANGELOG.md`](./CHANGELOG.md).
