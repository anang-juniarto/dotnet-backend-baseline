# Optional Push Notification Operations

> **Classification:** `[STANDARD]`
> **Status:** `Draft` | **Owner:** Platform and Application Engineering
> **Last verified:** Not verified | **Evidence:** Plan section 17; no certified external adapters

## Provider selection gate

- The selected backend default is .NET 10; external push remains independently optional.
- Push does not require chat, SignalR or a persistent inbox.
- Proposed starting option: FCM for web/Android, with configured APNs delivery for Apple.
- This is not an installed SDK, verified API surface, provider account or delivery guarantee.
- Verify exact server/client SDK versions, identifier semantics and supported platforms first.
- Direct Web Push/VAPID, direct APNs or managed providers require separate selection/research.
- Do not assume newer installation/token APIs from unversioned examples are compatible.
- Record chosen platforms, adapter owner, quotas, licensing and verification evidence.
- No external push adapter is implemented or certified here. The sample's optional SignalR live hints are not web/mobile OS push; its default Items data remains volatile memory.

## Registration lifecycle

- Authenticate destination registration, refresh and removal APIs.
- Bind owner/tenant to trusted actor context, not a user ID in the request.
- A destination identifier is not authentication or proof of installation ownership.
- Require verified device/installation ownership before reassignment to another account.
- Support multiple devices, rotation, permission withdrawal and expired destinations.
- Define logout/account-switch behavior to prevent notifications leaking to a previous owner.
- Select lifecycle semantics from the verified provider/client SDK, not a universal token model.
- Remove invalid registrations according to classified provider responses.

## Security and privacy

- Store service-account/APNs/VAPID private credentials in approved secret storage.
- Restrict access to tokens, subscription endpoints and subscription key material.
- Redact destinations, credentials and sensitive payloads from logs and telemetry.
- Publish only intentionally public client configuration; never distribute server credentials.
- Prefer minimal payloads with stable notification IDs; fetch protected content with authorization.
- Allow-list deep links; do not turn untrusted payload links into arbitrary navigation.
- Avoid private chat text on lock screens without explicit product/privacy consent.
- Direct Web Push must validate subscription destinations under an outbound SSRF policy.
- Restrict HTTPS destinations/ports, redirects and DNS resolution; never POST to arbitrary URLs.
- Separate transactional consent, marketing consent and OS/browser permissions.

## Durable dispatch and retries

- Persist intent/attempt state when recovery is required, even in push-only selections.
- Inbox persistence and each external delivery attempt have independent outcomes.
- Existing durable workers or a store-backed dispatcher can suffice; no mandatory broker.
- Classify permanent, transient, throttled and unknown outcomes from verified provider rules.
- Honor retry guidance and throttling; use bounded exponential backoff with jitter.
- Set retry ceilings, deadlines, TTL and stale-notification expiry; avoid infinite retries.
- Timeouts may leave an accepted send unknown; duplicates remain possible after retry.
- Use stable notification IDs and client deduplication; do not promise exactly-once delivery.
- Set reviewed collapse/TTL semantics, quiet hours, locale and time zone behavior.
- Provider acceptance is not guaranteed device receipt, display, execution or reading.
- Track attempt counts, queue age, permanent failures and invalid-registration cleanup safely.

## Client and rollout boundaries

- Browser delivery requires separately scoped HTTPS/service worker, permission UX and lifecycle.
- Mobile delivery requires selected platform SDKs and foreground/background handling.
- Apple delivery requires reviewed APNs configuration and supported app/platform behavior.
- Backend-only adoption does not create client code, service workers, accounts or certificates.
- Browser/OS power management and permission rules can delay/drop notifications.
- Do not promise silent background execution or delivery SLAs without measured support.
- Validate using fake/stub adapters by default, including retry and unknown-outcome paths.
- Test account switch, ownership theft, rotation, preference denial and payload redaction.
- Live sends require explicit test-account/device permission, isolated targets and client scope.
- Record unexecuted real-device/platform checks rather than inferring them from server success.

Related: [chat and notifications](../engineering/chat-and-notifications.md),
[SignalR](../api/signalr.md), [authentication](../api/authentication.md).
