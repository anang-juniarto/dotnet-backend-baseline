# Prerequisites

> **Classification:** `[PROFILE]`
> `Status: Draft` | `Owner: Engineering Leads` | `Last verified: Not verified` | `Evidence: Sample SDK policy inspected; installed SDK/editor and target dependencies require verification`

## 1. Choose the Execution Context

- **Reference baseline:** Markdown guidance; no root backend host, database or application build/test command is implied. Reading/adopting reference docs does not require starting services.
- **Isolated example:** Follow [examples/Baseline.Sample/README.md](../../examples/Baseline.Sample/README.md) for actual project paths, SDK selection, run/check commands and limitations. It is a volatile-memory core demo with gated Development auth, not a vendor integration starter or production auth implementation.
- **Consuming target:** Inspect its `global.json`, `.csproj` target frameworks, package manifests and approved profile. Approved new targets default to .NET 10 (`net10.0`), Core Domain/Application MediatR feature slices, modular Infrastructure provider/capability assemblies, Presentation WebApi controllers (optional Grpc), and Unit/Integration/Architecture test projects. Brownfield reference-only adoption retains the supported runtime, architecture and tooling; migration needs separate approval.

## 2. SDK and Editor

| Component | Requirement |
|---|---|
| .NET SDK | The sample pins `10.0.201` with `rollForward: latestPatch` and `allowPrerelease: false`. This permits compatible stable patches in the `10.0.2xx` feature band, not later feature bands or minor/major SDK versions. New targets record their approved pin/policy; brownfield targets preserve theirs unless migration is approved. |
| Visual Studio | .NET 10 requires Visual Studio 2026 version 18.0 or later; Visual Studio 2022 is not the .NET 10 baseline. Verify installed workloads and selected SDK support. |
| Other editors | Rider or VS Code may be selected after verifying their installed version/extensions support the target SDK; no unverified minimum version is prescribed. |
| Git | Needed for repository workflows; use the target's supported version. |
| Containers | Docker/Podman only if selected fixtures or infrastructure require them; verify exact engine/image versions and resource requirements. |

Before changing SDK policy, inspect `global.json` at the chosen execution root and applicable ancestor directories. This reference root has no `global.json`; the sample owns [its pin](../../examples/Baseline.Sample/global.json). Read-only discovery commands are `dotnet --list-sdks`, `dotnet --info` and `dotnet --version` from that execution root. Record the resolved SDK before proposing a change. The roll-forward policy describes permitted resolution, not evidence that any other SDK was tested. Discovery does not install a missing SDK or authorize package restore.

## 3. Optional Infrastructure

Use the [optional stack catalog](../architecture/optional-stack-catalog.md) to select only needed capabilities. No production datastore is implicit. SQL Server/MySQL/PostgreSQL, MongoDB, Elasticsearch, Redis, RabbitMQ, Hangfire, gRPC and Sentry/Seq/OpenTelemetry require their own compatibility, configuration and licensing evidence when selected. MySQL EF provider compatibility with the chosen EF runtime must be verified, not assumed.

SignalR does not require Redis for one-node hosting. Chat needs authentication and authoritative durable storage; persistent notification inboxes need recipient authorization/storage. Real-time delivery and web/mobile push are independent selections; no Firebase, broker or job engine is automatic. Provider SDKs, device/browser requirements and credentials remain unverified until a consuming target certifies them. The sample does not certify these modules.

Inspect the target's actual local manifests and configuration binding before choosing services or tools. Unselected modules add no packages, required options, containers, probes or network calls. API clients and database GUIs are conveniences, not mandatory baseline dependencies. Never install tools or start networked services without authorization.

## 4. Before Running Checks

Discover the actual framework, test runner and supported arguments as described in the [testing guide](../engineering/testing-guide.md). Do not infer a root test suite or generic runner flags from this reference. Use synthetic data, isolated local resources and secret sources outside committed files. Continue with [local development](./local-development.md) for target-specific setup.
