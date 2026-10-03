# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

---

## [Unreleased]

### Changed
- Aligned engineering workflows and agents with simple-first implementation, risk-based specifications/reviews, and removal of unnecessary boilerplate.
- Required same-change maintenance of affected repository-owned documentation and concise XML documentation for touched handwritten classes, actions, properties, and fields.
- Kept adoption guides and commands source-only, with approved ownership/drift-checked cleanup and final verification for existing destinations.
- Aligned contribution guidance and adoption verification with applicable repository capabilities and approved effective configuration.

## [2.0.0] - 2026-10-03

### Added
- Transparent AI-assisted implementation commands: `/plan-new-project-adoption`, `/plan-existing-project-adoption`, and `/implement-approved-adoption`.
- AI implementation lifecycle specification in `docs/adoption/ai-implementation-workflow.md`.
- Standardized adoption plan schema in `docs/adoption/adoption-plan-template.md`.
- Defined clean reference scopes: `core-reference` and `full-reference`.

### Removed
- Removed experimental script-based adoption automation (`adoption-common.ps1`, `adoption-preview.ps1`, `adoption-apply.ps1`, `validate-adoption.ps1`) and replaced with safe, transparent AI editing tools under human review.
- Removed `/adopt-new-project` and `/adopt-existing-project` commands.

### Changed
- Reframed baseline repository as a production-grade AI Engineering Reference Baseline in `BASELINE.md`, `README.md`, and `docs/adoption/`.

## [1.1.0] - 2026-10-03

### Added
- Graphify structural analysis standard (`docs/operations/graphify.md`) and commands (`/analyze-graph`, `/refresh-graph`).
- Automated post-adoption Graphify indexer (`.kilo/scripts/graphify-index.ps1`) with token-efficient summary caching in `.kilo/cache/graphify/`.
- Deterministic, drift-safe adoption preview and apply scripts (`.kilo/scripts/adoption-preview.ps1`, `.kilo/scripts/adoption-apply.ps1`) using temporary preview manifests.
- Coordinated Change Delivery Contract (`docs/engineering/change-delivery-contract.md`) and command (`/implement-change`) aligning Feature, API, Database, Config, and Tests.
- Baseline Capability & Fallback Matrix (`docs/adoption/capability-matrix.md`) guaranteeing full capability availability across adoption scopes.

### Changed
- Refactored all specialist agents in `.kilo/agent/` to enforce the 3-tier capability fallback hierarchy.
- Neutralized candidate product options in `docs/repository-profile-template.md` to discovery-oriented specifications.
- Updated `.kilo/.gitignore` to ignore `.kilo/cache/`, `.kilo/generated/`, and `.kilo/worktrees/`.
- Updated `BASELINE.md`, `.baseline-version`, `AGENTS.md`, and `docs/README.md` to reference baseline version 1.1.0 and aligned delivery contracts.

## [1.0.0] - 2026-10-03

### Added
- Baseline manifest (`BASELINE.md`) and baseline version tag (`.baseline-version`).
- Modular normative engineering standards under `docs/standards/` (14 domain-specific modules with router).
- Adoption playbooks in `docs/adoption/` for greenfield bootstrap, brownfield merge, conflict resolution, and post-adoption verification.
- Reusable repository profile template in `docs/repository-profile-template.md`.
- Kilo commands `/adopt-new-project` and `/adopt-existing-project` supporting two-phase preview-then-apply workflows.

### Changed
- Neutralized technology and architecture assumptions across all specialist agents in `.kilo/agent/`.
- Updated `AGENTS.md` and `docs/README.md` to establish clear document classification standards (`[CORE]`, `[STANDARD]`, `[PROFILE]`, `[TEMPLATE]`, `[DEFERRED]`, `[GENERATED]`).
- Replaced monolithic `Enterprise .NET Backend AI Engineering Guide.md` with routed modular standards.

### Deprecated
- *(No deprecations recorded)*

### Removed
- *(No removals recorded)*

### Fixed
- *(No fixes recorded)*

### Security
- *(No security advisories recorded)*
