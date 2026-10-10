# Redis Cache and Independent Backplane Roles

> **Classification:** `[PROFILE]`
> **Status:** Draft | **Owner:** Backend / Platform Engineering
> **Last verified:** Not verified | **Evidence:** Optional reference design; Redis adapter not certified

## Purpose and selection

Redis cache is an optional performance adapter, not an authoritative business store, ledger or transaction substitute. SignalR Redis backplane is an independent scale-out selection; single-node SignalR requires neither it nor a cache. Neither role selects Hangfire Redis storage. Unselected roles contribute no packages, settings, processes, probes or egress. The sample includes a separately compiled bounded Redis byte-cache adapter, not selected by its default memory host; no Redis server/backplane integration is certified. See its README/profile for exact evidence.

For approved .NET 10 Clean Architecture/CQRS targets, Application owns use-case cache ports; Infrastructure implements cache adapters, with API/Worker composition-root registration. The API owns its SignalR hub/backplane wiring. Candidate cache families are the Microsoft distributed-cache Redis adapter and StackExchange.Redis; verify exact client/server/framework compatibility, deployment topology, maintenance and product/edition licensing before selection. No versions are certified here.

## Cache contract and recovery

- Define cache-aside ownership, bounded value size, serialization/schema version, key prefix and TTL per entry. Include environment/service and trusted tenant/owner scope where applicable; never trust a caller-provided namespace. Keys and values must not contain credentials or unnecessary private content.
- Read the authoritative store on miss; invalidate or refresh only after confirmed commit. Account for concurrent stale fills with a reviewed version/invalidation strategy. TTL limits staleness; it is not proof of consistency. A failed invalidation must not be reported as a rolled-back business commit.
- Bound operation timeouts, connection/reconnect behavior and fallback concurrency. Optional cache outages bypass to the authoritative store with load protection, not unlimited retries or a stampede. Apply TTL jitter/coalescing only where needed; document stale-serving and negative-cache rules explicitly.
- Choose eviction/memory limits and monitor misses, latency, timeouts and saturation. No-eviction settings alone do not establish durable business storage. Distributed locks are not substitutes for authoritative constraints or transaction/concurrency protection.

## Backplane and operational boundaries

Use separate `ConnectionStrings:Redis` and `ConnectionStrings:SignalRBackplane` roles and isolated prefixes/channels, even if an approved deployment is shared. Sharing capacity requires isolation and load/outage review. Configure TLS, least-privilege ACLs and private access; do not expose Redis publicly. See [configuration](./configuration.md) and [secrets](../security/secrets-management.md).

Backplane pub/sub is not durable chat/inbox history or a general-purpose arbitrary-worker publishing API. Required recovery remains datastore/outbox/history-owned. Multi-node SignalR must test affinity and documented exceptions, proxy behavior and reconnect catch-up; a backplane outage can lose live hints. See [SignalR](../api/signalr.md). Cache/backplane outage criticality is assessed separately under [readiness policy](./observability.md); optional cache failure does not automatically remove API traffic.

## Acceptance and limitations

Test hit/miss/expiry, after-commit invalidation, concurrent stale fill, serialization change, owner/tenant isolation, size/TTL bounds, timeout/bypass and restart/outage load. For backplane selection, separately test multi-node delivery, affinity, disconnect/reconnect and history recovery. Record exact versions/topology and isolated evidence before certification. Follow [persistence](../standards/persistence-and-concurrency.md) and [resilience standards](../standards/resilience-and-workers.md); this guide performs no live checks.
