# Optional SignalR Transport

> **Classification:** `[STANDARD]`
> **Status:** `Draft` | **Owner:** API Governance Team
> **Last verified:** Not verified | **Evidence:** Plan section 17; no runtime certification

## Selection and ownership

- The selected greenfield default is .NET 10 controllers and MediatR CQRS feature slices, with modular Infrastructure and Presentation assemblies.
- SignalR is optional live transport; inbox and external push are independent selections.
- The [sample README](../../examples/Baseline.Sample/README.md) records optional hub implementation status. An authenticated live transport does not implement durable chat history, notification inbox storage, or background push delivery.
- SignalR is not a history store, durable queue, offline push service or read receipt.
- Keep hubs thin: map safe request DTOs into Application-owned use cases.
- The Application realtime port must not reference hub or vendor SDK types.
- An API-owned adapter may implement that port using presentation hub contracts.
- A separate worker must not reference the API project to publish events.
- Select an explicit worker-to-broadcaster or managed-service route when needed.

## Authentication and membership

- Authenticate connections and authorize every protected hub invocation/resource.
- Derive sender and tenant from validated server identity, never client-supplied IDs.
- Check current conversation membership for join, send, history and read operations.
- Group names, connection IDs and user IDs are routing data, not access grants.
- Revoke ongoing access when membership changes, including connection/group removal.
- Scope user routing by tenant where tenancy exists; test cross-tenant collisions.
- Define token expiry/revocation and reconnect policy for the exact .NET 10/client versions.
- Do not copy newer auth-refresh APIs from unversioned documentation without verification.
- Browser WebSocket/SSE tokens may use query strings; extract only on approved hub routes.
- Redact credentials/query strings in proxy, application and telemetry logs.
- Review allowed origins and WebSocket origin checks; CORS alone is not authorization.

## Event and invocation contracts

- Version public events and requests; do not broadcast internal entities or secrets.
- Include stable message/notification IDs and authoritative catch-up cursors where applicable.
- Document event names, payloads, ordering scope and backwards-compatible evolution.
- REST and hub sends must use the same command validation and deduplication semantics.
- Define safe invocation error codes separately from HTTP Problem Details.
- Hub completion is not proof of recipient delivery, display or user reading.
- Bound payload length, invocation frequency, group sizes and concurrent connections.
- HTTP request rate limiting does not automatically throttle each hub invocation.

## Reconnect and history

- Persist accepted chat messages and cursor data in the selected authoritative store first.
- For recoverable publication, atomically persist intent and dispatch after commit.
- Otherwise label live events best-effort hints and recover through authorized history/inbox.
- Database commit and live broadcast are not one atomic transaction.
- Clients deduplicate by stable ID and fetch bounded cursor-based catch-up after reconnect.
- Reauthorize and rejoin permitted groups; do not rely on ordinary reconnect restoring them.
- Timestamp alone is not a reliable concurrent-message ordering contract.
- Presence and typing are expiring hints, not durable online status.

## Hosting and validation

- Single-node hosting needs no Redis backplane.
- Multiple nodes require an explicitly selected/tested backplane or managed service.
- Verify affinity requirements for the actual transports, negotiation and proxy topology.
- A backplane outage does not buffer missed messages durably; history recovery still applies.
- Test proxy upgrades, idle timeouts, connection draining, rollout and reconnect storms.
- Test non-member denial, revocation, tenant isolation and duplicate sends.
- Test restart/broadcast failures with committed history/inbox still recoverable.
- Record exact server/client versions and selected scale-out evidence before deployment.

Related: [chat and notifications](../engineering/chat-and-notifications.md),
[push operations](../operations/push-notifications.md), [authentication](./authentication.md).
