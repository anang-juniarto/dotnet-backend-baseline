# Testing Strategy & Quality Assurance Guide

> **Classification:** `[STANDARD]`
> `Status: Draft` | `Owner: QA & Engineering Leads` | `Last verified: Not verified` | `Evidence: Reference acceptance matrix; target execution required`

This is reference guidance, not a test suite. Approved new targets default to .NET 10 Core Domain/Application MediatR feature slices, modular Infrastructure provider/capability assemblies, and Presentation WebApi controllers (optional Grpc); preserve verified brownfield architecture and test tooling. The isolated [Baseline.Sample README](../../examples/Baseline.Sample/README.md) owns its current checks and commands. Its volatile-memory demo and gated Development auth do not certify persistence, production authentication, chat, notifications, or vendor SDKs.

## 1. Risk-Based Test Portfolio

Approved greenfield targets organize projects under `tests/<Project>.UnitTests/`, `tests/<Project>.IntegrationTests/`, and `tests/<Project>.ArchitectureTests/`. Unit covers Core invariants and MediatR handlers; Integration covers selected provider/capability adapters and Presentation contracts; Architecture verifies Core independence, modular adapter boundaries and composition-root-only Infrastructure wiring. Preserve verified brownfield test layouts; these paths do not prescribe sample commands or install test tooling.

| Test type | Scope | Dependencies when selected |
|---|---|---|
| Unit | Invariants, pure logic, validation | Memory only; no network/database |
| Handler | Commands, read-only queries, cancellation, permissions | Verified fake/mock ports or simple in-memory fixtures |
| Architecture | Forbidden project references; composition-root exception; no transport/vendor types in business contracts | Approved tooling or source/project inspection |
| HTTP contract | Controllers, status/headers, serialization, authorization, error redaction | Verified test host/framework; WebApplicationFactory only if selected and compatible |
| Integration | Selected adapters, persistence, recovery | Authorized isolated real-engine/server fixtures |
| Concurrency | Uniqueness, state transitions, optimistic tokens, duplicate-effect protection when required | Actual selected database/topology |
| End-to-end | Few critical customer journeys | Isolated approved deployment/profile |

Do not introduce Testcontainers, a mocking library, architecture tooling, or a test-host package merely because it appears in an example. Verify manifests, compatibility and permission first. In-memory stores, EF InMemory and SQLite do not establish SQL Server/MySQL/PostgreSQL transaction or concurrency semantics.

## 2. Test Quality Principles

- Assert meaningful outputs, state transitions, and durable effects; assertion-free tests are not evidence.
- Use a verified clock abstraction or `TimeProvider` where supported. Avoid sleep-based timing assertions; use deterministic synchronization and bounded timeouts.
- Unit/handler tests never call live third parties. Integration tests use synthetic data and isolated resources, never production credentials/data.
- Avoid shared mutable state; use per-test isolation and owned cleanup. Do not assume rollback isolates background consumers or separate connections.
- Test confirmed commit, confirmed rollback, and ambiguous outcome separately. Duplicate-effect protection is risk-based, not mandatory storage for every operation.

## 3. Discovering and Running Checks

The reference baseline has no root application test command. For the isolated example, follow its [README](../../examples/Baseline.Sample/README.md); do not substitute consuming-target placeholders. For a consuming target:

1. Inspect `global.json`, affected `.csproj` files, package/lock manifests, repository scripts and CI before choosing commands.
2. Verify the test framework and runner (for example xUnit/NUnit/MSTest and VSTest/Microsoft.Testing.Platform), exact versions and SDK compatibility. Do not infer a runner from the framework name alone.
3. Use that target's documented solution/project paths and runner-supported arguments. Filters, verbosity, result logging and coverage switches are not interchangeable across runners; publish exact commands only after verification. No universal `dotnet test --filter` command is prescribed here.
4. Discover build, formatting/lint, analyzer/typecheck and test checks separately. Inspect scripts/hooks before running them; restore/downloads, containers and external calls retain approval gates.
5. Run the smallest relevant isolated checks, then the authorized broader suite. Missing optional services are explicit unexecuted checks, not passed tests or a reason to install infrastructure automatically.

The sample README owns commands for five distinct profiles: default memory host, selected host modules, aggregate compilation, separate gRPC host and artifact/CI validation. Follow its routes rather than duplicating commands here. Aggregate compilation is not all-stack runtime testing. The sample profile's recorded test execution results are evidence for its specific suite; consult current recorded execution results in the profile rather than duplicating numbers across guides. Other permitted SDK patches are not tested merely because `latestPatch` allows resolution.

For a new target, select a compatible framework/runner and packages before creating tests. Preserve brownfield choices unless an independently approved change requires migration.

## 4. Selected-Capability Acceptance Matrix & Real-Provider Gates

Apply only selected rows from the [optional stack catalog](../architecture/optional-stack-catalog.md). Real-provider tests must not run against production databases or unverified targets. Before executing real-provider checks, verify:
- An approved target project or isolated pilot exists with explicit capability selection.
- Isolated test infrastructure (ephemeral container or dedicated test server) uses synthetic credentials outside source.
- Lifecycle cleanup, resource limits, and failure handling are defined.
- Schema creation vs. migration upgrade are distinguished (e.g. `EnsureCreated` does not prove migration capability).

Evidence tiers must be distinguished:
- **Implemented**: adapter code/registration exists.
- **Compile-verified**: project/host builds with 0 errors/warnings for selected dependencies.
- **Runtime-tested**: in-process automated tests pass for tested behaviors.
- **Live-verified**: executed against isolated real engine/broker/service.
- **Deferred**: unverified or unimplemented integrations.

Record exact SDK/package/server/image versions and topology, date, commands, results and limitations. A package build alone is not integration certification.

| Area | Evidence required when applicable |
|---|---|
| Core CQRS/HTTP | Command invariants, read-only bounded queries, cancellation, 401/403, validation, not-found, conflict/precondition semantics, redacted Problem Details; preserve existing published envelopes |
| SQL Server/MySQL/PostgreSQL | Each selected provider's real-engine migrations, constraints, concurrency, rollback/unknown outcome; no provider compatibility inferred from another |
| MongoDB | Collection/index ownership, uniqueness, mapping/concurrency; replica-set transaction tests if transactions selected |
| Elasticsearch | Mapping/version compatibility, refresh/staleness, projection recovery, rebuild and outage without corrupting authoritative data |
| Redis | TTL, invalidation after commit, outage bypass, bounded timeouts and tenant isolation where applicable |
| RabbitMQ | Confirms, ack-after-success, duplicate/poison deliveries, bounded retry/DLQ, durable publication recovery after restart where required |
| Hangfire | Selected storage adapter restart/retry behavior, repeat-safe effects where required, scoped dependency resolution and dashboard denial |
| gRPC | HTTP/2/TLS, contract evolution, auth, status mapping, deadlines/cancellation and message limits |
| OpenTelemetry/Seq/Sentry | One owner/export path per signal, correlation, sampling, redaction, bounded labels/buffers, collector outage; handled-error capture without duplicates |
| SignalR | Authenticated connection, authorized group membership, reconnect/resynchronization, rate/payload bounds; selected multi-node routing/scale-out profile only |
| Chat | Conversation membership and sender identity, durable history/pagination, ordering contract, duplicate-send protection when required, reconnect recovery; transport acceptance is not persistence |
| In-app notifications | Recipient isolation, durable inbox, unread/read concurrency, preferences, pagination and offline recovery; real-time delivery optional |
| Web/mobile push | Selected provider/device/browser matrix, permission/token lifecycle, revoked/stale tokens, safe payloads, retry/duplicate behavior and provider outage; acceptance does not prove device receipt |
| Omitted modules | No package/project/container, required setting, hosted service, probe or network dependency for unselected capabilities |

Cover core, each selected adapter and critical dependency pairs rather than all combinations. Chat, inbox, real-time delivery and external push are independent selections; Redis/RabbitMQ/Hangfire/Firebase are not implicit test prerequisites.

## 5. Verification Reporting Standards

- State outcome: Review-only, Implemented, Partially implemented, or Blocked.
- Report exact executed commands, exit results and available counts; separate static inspection from executable evidence.
- Identify failures and reproductions without exposing secrets. List unexecuted checks and reasons, including unavailable credentials, unsupported packages or deferred integrations.
- Separate reference-document review, isolated-sample checks and consuming-target certification. Never claim any build, lint, test or integration passed unless actually executed successfully.
