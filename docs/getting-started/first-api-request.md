# First API Request Guide

> **Classification:** `[PROFILE]`
> `Status: Draft` | `Owner: Engineering Leads` | `Last verified: Not verified` | `Evidence: Reference verification checklist; routes and contracts owned by executable target`

## 1. Select the Actual Application

The reference baseline publishes guidance, not a running root API. Approved new consuming targets default to .NET 10 Core Domain/Application MediatR feature slices, modular Infrastructure and Presentation WebApi REST controllers dispatching requests through MediatR (optional Grpc); that choice does not create routes. Brownfield targets retain their published routes, auth and response contracts until an approved migration.

For the isolated example, follow [Baseline.Sample/README.md](../../examples/Baseline.Sample/README.md) for exact routes, request bodies, ports, demo-auth opt-in and commands. This guide intentionally does not duplicate evolving sample contracts. The sample is volatile-memory only, with gated Development auth; it does not prove production authentication, persistence, SignalR/chat, notification inboxes or push integration.

Select the default memory or separate gRPC request profile through that README. Selected host-module, aggregate-compilation and artifact/CI profiles are separate checks, not proof that an endpoint or external service is running. Keep command ownership there for all five profiles; `examples/**` is not an adoption payload.

For a consuming target, use its generated/published OpenAPI when present, actual controller/endpoint registration and target API documentation. The baseline [API reference](../api/README.md) is guidance, not proof that a target implements a route or OpenAPI UI.

## 2. Safety and Discovery

- Use only an authorized loopback local/test target and synthetic data. Never point requests at staging/production or a live external provider by default.
- Confirm the actual host address from launch settings/startup output; do not guess a port, `/api/v1` prefix, health route or token issuer.
- Discover whether authentication is required and how the target supplies a synthetic test identity. Do not reuse sample demo headers or enable demo auth in a consuming production host.
- Inspect actual liveness/readiness endpoint registration and contract if provided. Do not assume `/health/live`, `/health/ready`, JSON content type or a `{"status":"Healthy"}` body. If probes are absent, record that fact rather than inventing them.
- Readiness should reflect required serving dependencies; noncritical telemetry/cache failures are not automatically liveness failures. Omitted modules require no probes or services.

## 3. Make and Validate the Request

Use the target/sample README's verified request command, replacing only documented local values. On Windows distinguish `curl.exe` from shell aliases if using curl; use syntax supported by the actual shell. No universal bearer token, route, resource or response payload is prescribed here.

Check applicable acceptance criteria:

- Status, content type, location/pagination/precondition headers and body match the actual contract. A probe may legitimately return text or an empty body.
- New HTTP targets default to plain success DTOs and Problem Details errors; preserve existing envelopes/status codes in brownfield targets. See [response/error guidance](../api/response-and-error-contracts.md).
- Validation and authorization denial are verified, not just a happy-path response; distinguish unauthenticated and forbidden behavior according to the target contract.
- Errors reveal no stack traces, tokens, SQL or connection strings. An ownership-sensitive not-found response does not disclose another user's resource.
- Any mutation uses disposable synthetic data and documented cleanup; repeat only when duplicate-effect behavior is understood. Durable idempotency is selected by risk, not a blanket requirement on all requests.

## 4. Optional Transports and Delivery

HTTP success alone does not certify gRPC, SignalR, chat, inbox or push. If selected, use the target's verified proto/client or real-time contracts and [capability test matrix](../engineering/testing-guide.md): authenticated membership/recipient isolation, durable history/inbox recovery, reconnect semantics and provider failure/token lifecycle as applicable. SignalR delivery is not durable history; push-provider acceptance is not device receipt. No Firebase, Redis, RabbitMQ or Hangfire dependency is inferred from these choices.

Report actual command/target and observed outcomes without secrets. Separate reference review, sample checks and consuming-target evidence; list unexecuted checks and deferred integrations explicitly.
