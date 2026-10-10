# .NET 10 Greenfield Baseline Profile

> **Classification:** `[PROFILE]`
> **Status:** Draft
> **Owner:** Architecture / Engineering
> **Last verified:** Not verified
> **Evidence:** Approved reference decisions; not package or integration certification

## 1. Applicability and evidence

This is the default for separately approved greenfield generation, not a description of a deployed application. Reference assets remain non-executable. The repository also contains the optional isolated `examples/Baseline.Sample` local in-memory demo, excluded from baseline transfer by `examples/**`; it does not certify external integrations or production durability.

Reference-only brownfield adoption preserves verified runtime, architecture, packages, persistence and published contracts. Selected-capability implementation and incremental architecture/runtime migration are separate approval modes. When profile documents are absent, inspect relevant manifests/source and apply available standards to verified capabilities; do not block work or generate missing scaffolding.

## 2. Default decisions

| Concern | Greenfield default | Qualification |
|---|---|---|
| Runtime | `net10.0` | Pin an available stable SDK with explicit roll-forward after verification; no invented version |
| Layers | Core (Domain/Application), Infrastructure, Presentation | Modular provider/capability assemblies; composition root in each host |
| Use cases | Feature-organized commands/queries with MediatR handlers | MediatR explicitly selected for greenfield; no implied event sourcing, split databases or broker |
| HTTP | REST controllers | Preserve existing Minimal APIs; transport migration needs approval |
| Responses | Plain success DTOs and RFC 9457 Problem Details | Preserve existing published envelopes/status codes until consumer migration |
| Persistence | None implicitly | Select one primary store per bounded context; stateless profile is valid |
| Adapters | Selected provider/capability assemblies under Infrastructure | Shared EF mappings/context separated from SqlServer, PostgreSql and MySql migrations/registration |
| Optional gRPC | Separate Presentation Grpc host | Protos/services/interceptors map to Application; host selection remains optional |
| Optional messaging | MassTransit with RabbitMQ when selected | Dedicated Infrastructure.Messaging assembly; existing brownfield stacks preserved |
| Optional consumers/jobs | Separate worker host | Explicit in-process mode may be reviewed with lifecycle/scaling trade-offs |
| Optional telemetry | OpenTelemetry preferred vendor-neutral route | Sentry and Seq independently selectable; one capture/export owner per signal |
| Example feature | One harmless CRUD use case when approved | No speculative domain, identity server, payments or tenancy scaffolding |

Use SDK compiler defaults rather than floating `LangVersion=latest`; enable nullable and implicit usings in new projects. Preserve compatible analyzer and runner policies in existing applications.

## 3. Dependency and execution contracts

- Domain has no application, infrastructure, transport or vendor SDK dependencies. Tactical DDD and base entities are not mandatory.
- Application references Domain, MediatR and approved application-level validation/behavior packages; it owns transport-neutral commands, queries, results and use-case-specific ports. MediatR pipeline behaviors compose selected validation, logging, tracing and performance concerns without leaking vendor SDKs into Domain. Do not add unused interfaces or generic repository wrappers.
- Infrastructure implements Application ports; SDK types, persistence documents and tracking objects stay in adapters.
- API references Application, and Infrastructure only for composition-root wiring. Controllers map transport DTOs to MediatR requests, propagate cancellation through dispatch and map results; they do not call Infrastructure directly.
- Optional Worker references Application and Infrastructure for its own composition root, never API. Establish a scope per delivery/job; persist stable small versioned payloads, not services or aggregates.
- Commands enforce authorization/invariants and have one explicit transaction owner. Persist required publication intent atomically where supported. Distinguish confirmed commit, rollback and unknown outcome; never blindly retry an ambiguous non-idempotent command.
- Queries are read-only, bounded and projected through read ports; a separate read store is not required. State material projection staleness.
- Use MediatR behaviors where cross-cutting Application concerns justify them; do not clone a mediator framework or add unused abstractions. Durable idempotency storage is risk-based, not universal boilerplate.
- The canonical user-owned `gemini-code-1791622287764.txt` defines the selectable structural superset. Maintained guidance uses .NET 10 and RFC 9457; the raw file remains unchanged. Kubernetes, Helm, Terraform/cloud, Docker, CPM and CI providers are optional approved deployment/tooling choices, not prerequisites.

See [repository topology](./repository-map.md) and [normative boundaries](../standards/architecture-and-use-cases.md).

## 4. Capability selection

Use the [optional-stack catalog](./optional-stack-catalog.md). Reference scope (`core-reference` or explicit `full-reference`), adoption mode and runtime capabilities are separate selections. Select at generation time: omitted modules have no package, container, required setting, schema, health check, hosted service or network dependency. Runtime switches apply only to intentionally included features.

SignalR transport, durable chat, persistent inbox and external web/mobile push are distinct choices. Chat selects identity, authoritative storage and SignalR; inbox can run without SignalR or push; push can run without chat or inbox but needs recoverable delivery intent where required. Single-node SignalR does not require Redis. Firebase authentication/storage, RabbitMQ and Hangfire are not implied by push selection.

Application owns realtime ports; API owns adapters referencing hubs. A separate worker publishes through an explicit managed-service route or durable signal consumed by an API broadcaster, not an API project reference. A Redis backplane is not an arbitrary-worker publishing API.

## 5. Approval and acceptance

Before target generation, approve exact per-file actions, destination, selected SDK/packages/server versions, licensing, local infrastructure and test isolation. Do not transfer source plans, state, adoption assets or `examples/**`. Preserve target identity/history/license and MIT attribution under the [manifest policy](../../BASELINE.md).

Validate core references, handler invariants/read-only queries, cancellation, HTTP contracts and disabled-module absence. Certify each selected adapter against its actual engine/provider, including recovery and outage behavior; an in-memory demo or successful package build is insufficient. Record exact versions/date and passed, failed and unexecuted checks. No production environment, live provider test, restore, migration or service startup is implicitly authorized.
