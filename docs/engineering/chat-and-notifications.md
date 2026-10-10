# Optional Chat and Notifications

> **Classification:** `[STANDARD]`
> **Status:** `Draft` | **Owner:** Application Engineering
> **Last verified:** Not verified | **Evidence:** Plan section 17; design invariants only

## Independent selections

- The selected greenfield default is .NET 10 Clean Architecture with MediatR feature slices and canonical Core/Infrastructure/Presentation modular assemblies.
- Chat, persistent inbox, SignalR transport and external push require explicit selection.
- Chat needs authenticated membership and an authoritative history store.
- Inbox needs authenticated recipients and authoritative persistence; SignalR is optional.
- External push can exist without chat, inbox or SignalR.
- FCM is a proposed provider option, not an installed SDK or certified adapter.
- No Firebase authentication or database migration is implied by provider choice.
- The sample's optional receive-only SignalR transport does not implement durable chat, notification inbox or external push. See its README/profile for actual compiled capability evidence.

## Chat invariants

- Initially select only required text conversation, membership, send and history use cases.
- Attachments, moderation, editing, search and voice/video require separate scope.
- Treat text as untrusted content; client rendering must not execute supplied HTML.
- Derive sender identity server-side and check current membership on every command/query.
- Tenant boundaries apply to identifiers, membership, history and event routing.
- Membership removal must revoke ongoing live access, not just future HTTP access.
- Persist a stable message ID and defined ordering/cursor before durable acceptance.
- Use a client operation ID unique within conversation/sender scope to deduplicate retries.
- Concurrent ordering requires a sequence or deterministic stable cursor/tie-breaker.
- Bound history page sizes; wall-clock timestamps alone cannot prevent skipped records.
- Durable acceptance, live publication, receipt and reading are distinct states.
- Clients recover authorized history after reconnect and deduplicate events by message ID.
- Optional durable read cursors must be monotonic per recipient and privacy-aware.

## Notification flow

1. Record business notification intent transactionally when reliable coupling is required.
2. Resolve authorized recipients and channel preferences through trusted server policy.
3. Persist recipient records when inbox is selected; deduplicate business event/recipient keys.
4. Dispatch independent SignalR hints and/or external push attempts after commit.
5. Persist attempt outcomes and retry schedules without marking content read.
6. Let authorized clients retrieve content and explicitly update eligible read state.

- Inbox state survives live transport/provider outages when persistence is selected.
- Push-only recovery still needs durable intent/attempt storage, not an artificial inbox.
- Notification creation, unread count and mark-read operations enforce recipient ownership.
- Define mark-all boundaries and concurrent arrival behavior; repeated acknowledgments are safe.
- Define expiration/archive, quiet hours, locale/time zone and channel consent semantics.
- Transactional and marketing consent are separate product/privacy policies.

## Boundaries and dispatch

- Application owns use cases and ports; Domain/Application do not depend on vendor SDKs.
- Infrastructure owns selected storage/provider adapters; API owns hub presentation types.
- Use the simplest coherent model; do not mandate a universal notification framework.
- A durable store-backed dispatcher can suffice; RabbitMQ and Hangfire are not mandatory.
- Persist publication intent atomically for required recovery, then process with bounded retries.
- Do not claim a database write and external send are atomic or exactly-once.
- Unknown send outcomes may produce duplicates; stable IDs support client deduplication.
- Keep private content out of lock-screen payloads unless explicit privacy policy allows it.

## Acceptance evidence

- Test non-member/other-recipient denial, tenant isolation and membership revocation.
- Test duplicate sends, concurrent ordering, cursor pagination and reconnect catch-up.
- Test unread/read state across devices and repeated acknowledgments.
- Test commit success with broadcast/provider failure and dispatcher restart recovery.
- Test preferences, expiration and registration ownership using fake provider adapters.
- Live device delivery needs separate approved accounts/devices and client scope.
- Unselected modules add no schema, hub, provider secrets or required hosted process.

Related: [SignalR](../api/signalr.md), [push operations](../operations/push-notifications.md).
