# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

---

## [Unreleased]

### Added
- Explicit .NET 10 Clean Architecture/CQRS greenfield profile and independently selectable integration catalog, including telemetry, persistence, caching, messaging, gRPC, jobs, SignalR chat, notification inbox, and web/mobile push design guidance.
- Isolated `examples/Baseline.Sample` core demonstration with local in-memory persistence; external integrations and durable chat/notification behavior are not certified by this sample.
- Root baseline validation workflow plus profile-isolation regression checks for default dependencies, disabled observability registration, and realtime authorization boundaries.

### Changed
- Separated HTTP CreateItemRequest DTO from Application commands under Presentation Contracts to protect server-resolved actor identity, aligned transport validation with normalized Application/Domain name length invariants, added CI SDK diagnostic logging, and completed exact-invocation evidence recording.
- Optimized sample onboarding with five source-verified local profiles, explicit build-versus-runtime selection, evidence tiers, `latestPatch` SDK policy bounded to the verified 10.0.201 feature band, and consistent shared/provider EF scanning guidance.
- Extracted shared and provider-specific EF mappings into `IEntityTypeConfiguration<T>` classes under persistence `Configurations/`, mapped Domain entities directly without duplicate persistence entities, and kept contexts limited to ordered registration. Business entities remain EF-free in Domain.
- Aligned the .NET 10 greenfield blueprint and executable example with the user-designated Core/Infrastructure/Presentation modular schema, explicit MediatR CQRS, selected capability assemblies and categorized tests; retained brownfield compatibility and optional deployment decisions.
- Kept reference adoption separate from executable example generation and excluded `examples/**` from baseline transfer payloads.
- Distinguished reference-only adoption, selected capability additions, and separately scoped runtime/architecture migration for existing projects.
- Defaulted both new and existing adoption to `core-reference`; `full-reference` remains explicit opt-in with exact-file approval, never bulk recursive copy/mirror.
- Centralized source/destination exclusions including Git metadata and plans, root-document/license preservation and MIT attribution; distinguished source import from later authorized target generation.
- Required normalized non-overlapping roots, containment/duplicate/drift checks, selected-tree dependency validation and actual-change-to-manifest audits. Older plans require re-review; no automatic target migration or cleanup.
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
