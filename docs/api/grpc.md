# Optional gRPC Contracts

> **Classification:** `[STANDARD]`
> **Status:** `Draft` | **Owner:** API Governance Team
> **Last verified:** Not verified | **Evidence:** Design guidance; no certified adapter

## Selection boundary

- The selected greenfield default is .NET 10 REST controllers with MediatR CQRS in `src/Core`; optional gRPC is a separate `src/Presentation/<Project>.Grpc` host.
- gRPC is an independently approved transport, not a required replacement for REST.
- Preserve existing runtime and published contracts in brownfield adoption.
- The [sample README](../../examples/Baseline.Sample/README.md) records the actual gRPC implementation/build status. A compiling transport does not certify TLS/proxy behavior, production authentication or durable persistence.
- Verify target frameworks, server/client packages and hosting support before implementation.

## Contract authority

- Declare the authoritative `.proto` files, owner and generation procedure.
- Generated bindings are outputs; never hand-edit them or maintain competing schemas.
- Use transport DTOs rather than exposing persistence entities or domain internals.
- Keep field numbers stable; reserve removed field numbers and names.
- Do not reuse removed tags or silently change field meaning, type or cardinality.
- Review default values, presence and unknown fields with actual supported clients.
- Version incompatible services deliberately and document migration/deprecation.
- REST OpenAPI does not describe a native gRPC service contract.
- gRPC-Web or HTTP transcoding requires separate selection and compatibility testing.

## Application boundaries

- API-hosted services map requests to Application-owned commands and queries.
- Domain/Application must not depend on gRPC server types or generated transport bindings.
- Reuse the same authorization, validation and idempotency rules across transports.
- Propagate cancellation and deadlines into handlers and downstream I/O.
- Bound message sizes, stream duration, concurrency and per-identity usage.
- Streaming is not durable history, a queue, or proof of recipient processing.

## Authentication and authorization

- Require TLS and the selected identity mechanism; do not invent a universal token scheme.
- Validate credentials server-side and derive actor/tenant context from trusted claims.
- Authorize each resource operation, not merely the initial connection or service entry.
- Review access revocation and token expiry during long-running streams.
- Do not send credentials in message payloads or record authorization metadata in logs.
- Browser clients require an explicitly reviewed browser-compatible transport path.

## Errors and retries

- Define stable gRPC status and application error-code mappings for each operation.
- HTTP Problem Details and HTTP status tables are not native gRPC wire envelopes.
- Distinguish invalid input, unauthenticated, forbidden, missing and conflicting state.
- Redact exception details; expose safe correlation metadata where supported.
- A deadline or disconnect can leave a mutation outcome unknown.
- Retry only safe/idempotent operations with bounded backoff and remaining deadline.
- Deduplicate retried commands using an operation key with a documented uniqueness scope.
- Do not claim exactly-once execution from transport retry behavior.

## Adoption validation

- Test generated bindings against each supported client/runtime version.
- Test schema evolution, malformed input, authorization and tenant isolation.
- Exercise cancellation, deadlines, oversized messages and interrupted streams.
- Verify HTTP/2, TLS termination, proxies and load-balancer behavior in the chosen topology.
- Test ambiguous mutation outcomes and duplicate requests with a fake downstream boundary.
- Record package versions and executed evidence; static guidance is not certification.

Related: [API governance](./README.md), [authentication](./authentication.md),
[response contracts](./response-and-error-contracts.md).
