# Secrets Management and Credential Lifecycle

> **Classification:** `[PROFILE]`
> **Status:** Draft | **Owner:** Security Operations / DevOps
> **Last verified:** Not verified | **Evidence:** Selected-capability reference policy; no secret-store adapter certification

## Storage and injection

Never commit real credentials, private keys, production connection strings or secret-bearing fixtures/configuration. Include artifacts, logs, screenshots, telemetry, job arguments, outbox/DLQ payloads and backups in leakage review. Use approved secret scanning in the target's delivery process; this reference does not claim scanners or hooks are installed. Follow [security standards](../standards/security-and-boundaries.md) and [supply-chain controls](../standards/delivery-and-supply-chain.md).

| Environment | Reviewed source | Boundary |
|---|---|---|
| Local development | Approved local secret storage; .NET Secret Manager only when explicitly configured | User secrets are not encrypted production vault storage; never use live production credentials |
| CI/isolated tests | CI vault or approved short-lived identity/injection | Scope to job/environment, mask output and restrict untrusted pull-request access |
| Staging/production | Selected managed secret store/workload identity or approved equivalent | Least privilege, audited access, version/rotation ownership and explicit availability policy |

Prefer short-lived/workload identities where the selected provider supports them. Secret-store SDK/package/host compatibility requires exact-version verification and explicit selection; no cloud provider is mandatory. Avoid secrets in CLI arguments, shell history or plaintext tracked settings. Environment variables are an injection mechanism, not encrypted storage; restrict process/debug/dump access. Define whether each setting reloads or requires restart; do not assume clients refresh credentials automatically.

## Selected secret inventory

Keys below are proposed application conventions from [configuration](../operations/configuration.md), not installed SDK options. Preserve verified brownfield keys until approved migration. Omitted capabilities require no secrets and perform no secret lookup or outbound work. The in-memory Items sample needs no external-service credentials.

| Selected role | Proposed key/source | Handling |
|---|---|---|
| Primary relational store | `ConnectionStrings:Primary` | One primary convention; do not also require `DefaultConnection`/`Database:ConnectionString` |
| MongoDB | `ConnectionStrings:MongoDb` | Scope database/collection rights and protect embedded credentials |
| Elasticsearch | `Elasticsearch:ApiKey` or selected identity | Restrict index privileges; separate administrative rebuild identity where needed |
| Redis cache | `ConnectionStrings:Redis` | Private TLS/ACL identity and isolated namespace |
| SignalR Redis backplane / managed service | `ConnectionStrings:SignalRBackplane` or selected managed identity | Independent of cache; restrict pub/sub role and redact hub query tokens |
| RabbitMQ | `ConnectionStrings:RabbitMq` | Vhost/publish/consume/admin privileges separated |
| Hangfire storage | `ConnectionStrings:Hangfire` or selected storage credential | Independently selected provider/license; protect persisted job arguments/dashboard access |
| Seq / OTLP | `Seq:ApiKey` / approved exporter authentication source | Restricted ingest identity, endpoint review and no secret-bearing diagnostic URLs |
| Sentry | `Sentry:Dsn` through approved configuration source | Review exposure/ingest abuse; never distribute privileged Sentry auth tokens as DSNs |
| FCM / APNs / Web Push | Selected service identity / APNs private key / VAPID private key via secret store | Least-privilege provider/app/environment isolation; public client configuration or VAPID public key is not the private credential |
| Transport TLS / application cryptography | Selected certificate/private-key and key-management source | Explicit ownership, expiration/rotation and access controls; no unselected identity scaffolding |

Push destinations, subscription endpoints/keys and device tokens are sensitive routing/user data even where they are not server credentials. Restrict storage access, redact telemetry and define deletion/retention with registration lifecycle. Bind registrations to authenticated ownership; token possession does not authenticate an actor. Minimize private notification/chat content in all delivery and telemetry channels.

## Rotation and incidents

1. Inventory owner, purpose, environment, privileges, expiry, source/version and consuming hosts without recording secret values.
2. Provision a replacement and use an overlap window only when the provider supports it. Validate clients reload/reconnect or roll hosts safely; keep in-flight work and scoped identities compatible.
3. Verify replacement usage through safe credential-version metadata, not secret text. Revoke the old credential and test denied use. An absence of sampled telemetry alone does not prove old credential use has stopped.
4. For compromise, revoke/rotate promptly, assess token/session and data exposure, preserve sanitized incident evidence and follow authorized response procedures. History cleanup does not undo exposure; destructive history rewriting requires separate approval.

Webhook verification overlap, APNs/FCM identities, VAPID rotation and destination renewal have provider-specific semantics; verify them before rollout rather than assuming universal dual-key behavior. Plan failure modes for vault/exporter outages without leaking values or silently falling back to production/default credentials.

## Privacy and validation gates

Scrub secrets from validation errors, connection failures, URLs/query strings, headers/cookies, exception data, breadcrumbs, traces and proxy logs before export. Disable raw body capture by default. Review telemetry retention/residency/access and message/job/DLQ/backups separately; operational telemetry is not the authoritative security audit record.

Verify missing selected secrets fail safely, disabled modules need nothing/send nothing, least privilege and environment isolation hold, redaction survives errors, rotation works across API/Worker instances and dashboard/log access is denied to unauthorized users. Follow [observability policy](../operations/observability.md). No network lookup, real credential injection, rotation or live verification is executed by this guide.
