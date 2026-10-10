# Security Architecture and Selected Capability Boundaries

> **Classification:** `[PROFILE]`
> **Status:** Draft | **Owner:** Security Engineering / AppSec
> **Last verified:** Not verified | **Evidence:** Reference threat boundaries; no external adapter certification

## Scope and authority

Approved new targets use .NET 10 Clean Architecture/CQRS with API/optional Worker composition roots. Domain/Application enforce business policy through vendor-neutral contracts; transport authentication and Infrastructure adapters do not replace resource authorization. Preserve brownfield security/contracts unless migration is approved. The isolated sample covers volatile owner-scoped Items only; its development-only demo authentication is not production identity or certified external integration.

Choose only approved [capabilities](../architecture/optional-stack-catalog.md) and apply [security standards](../standards/security-and-boundaries.md). Unselected modules add no exposed endpoints, credentials, packages, hosted services or egress. Record exact SDK/provider versions, threat review, dependency/license evidence and tests before claiming support. This guide does not provision identity, secrets or services.

## Trust boundaries

Treat Internet input, headers, hub arguments, message/job payloads, push registrations, logs and provider responses as untrusted. Authenticate at each exposed transport; derive actor identity server-side and authorize every command/query against current resource ownership/membership and tenant scope where applicable. Possession of an ID, connection group, push token or trace context grants no permission. Use bounded typed DTOs; do not bind internal entities or trust role/tenant fields from payloads.

| Selected surface | Required boundary controls |
|---|---|
| REST/gRPC | TLS, verified proxy trust, authentication/authorization, input/body/message limits, rate limits and deadlines; sanitized public errors |
| Datastores/search | Private least-privilege identities, parameterized queries, ownership-filtered reads/writes, encrypted sensitive data and approved schema authority; search projections retain authorization requirements |
| Redis cache/backplane | Separate roles/namespaces/ACLs, private TLS access, no secrets in keys; cache is not authorization authority or durable history |
| RabbitMQ | Isolated vhost/routing permissions, TLS, bounded schema-validated payloads, authorized context, protected outbox/DLQ and controlled replay |
| Hangfire | Independently reviewed durable storage, minimized persisted arguments, restricted authenticated/authorized dashboard and audited administrative mutations |
| SignalR/chat/inbox | Current membership/recipient checks on commands, queries and hub invocations; revoke live access, bound connections/invocations, reauthorize after reconnect; groups are not authorization |
| Web/mobile push | Authenticated registration ownership and reassignment, logout/account-switch/rotation cleanup, reviewed provider egress and consent; destination is not identity |
| Seq/Sentry/OTel | Pre-export scrubbing, bounded sampling/buffering, least-privilege ingest/query access and retention/residency review; no duplicate unintended exports |

## Network and payload protection

Use approved TLS 1.2+ policy with certificate/hostname validation; never bypass validation for convenience. Restrict administrative listeners, health details and dashboards independently of public API access. Permit only selected outbound destinations and private service paths; dependency discovery must not silently enable integrations.

For user-influenced outbound URLs, apply reviewed HTTPS/port/destination rules, normalized DNS/IP checks including IPv4/IPv6 loopback, private/link-local and metadata addresses, redirect controls and rebinding defenses. Direct Web Push requires a supported push-service destination policy, not arbitrary subscription POSTs. Allow-list deep links. Treat chat text as untrusted rendering input; attachments/uploads require separately selected content/type/size/storage controls.

Minimize push payloads to safe notification IDs; fetching private content requires authorization. Provider acceptance does not mean device delivery/read. Avoid private lock-screen content unless explicitly permitted. Chat/history/inbox retention, read receipts, consent and deletion/backup handling need product/privacy ownership. Protect message/job/DLQ and push-registration storage as well as live traffic.

## Secrets, telemetry and assurance

Use approved platform cryptography and password-hashing policy; do not invent algorithms. Separate credential lifecycle from data encryption/key rotation and define owners. Follow [secrets management](./secrets-management.md) and [telemetry ownership](../operations/observability.md), including .NET 10 handled-exception diagnostics. Scrub authorization/cookies/query tokens, bodies, push destinations and private content at application, proxy and exporter layers. Diagnostic logs are not a tamper-resistant audit trail.

Test resource/membership/recipient denial, revocation/reconnect, registration takeover, tenant isolation where applicable, SSRF/limits, dashboard denial, secret redaction and disabled-module absence. Review administrative audit access/retention and threat-specific abuse cases. Exact target tests and live-service verification remain separately authorized and unexecuted by this reference.
