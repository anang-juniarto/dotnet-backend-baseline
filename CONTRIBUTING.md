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
1. Discover and run the repository's applicable validation commands. This non-executable baseline requires documentation/configuration checks, not application builds. In a destination with a verified .NET project, use its build/test commands, for example:
   ```bash
   dotnet build
   dotnet test
   ```
2. For API changes, verify request/response serialization against contract expectations.
3. Truthful reporting: Explicitly document which tests were executed and passed, and which were omitted (with clear rationale).

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
