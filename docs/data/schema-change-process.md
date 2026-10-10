# Schema Change Process & Compatible Evolution

> **Status:** Reference guidance | **Owner:** Database Engineering
> **Last verified:** Not verified | **Evidence:** Release process design, not executed migrations

## Preconditions

Declare [schema authority](./schema-ownership.md), selected provider and affected consumers.
Record exact engine/client versions and the authorized execution environment.
Inspect existing migrations, deployment hooks and backup procedures before proposing execution.
This guidance does not authorize database connections, migration execution or destructive work.
Zero downtime is a validation objective, not a guarantee from an additive-looking script.
Assess locks, rewrite/build cost, disk headroom and old/new application compatibility.
Externally managed schemas follow their owner's verified change process.

## 1. Expand

- Add compatible tables, nullable columns, optional document fields or versioned search indexes.
- Ensure old writers and readers still work, including defaults and validation rules.
- Add indexes only after checking duplicates, build/locking behavior and resource limits.
- Separate SQL Server, MySQL and PostgreSQL migration sets and snapshots.
- Review generated SQL for the exact selected provider; never reuse it blindly elsewhere.
- Version MongoDB validators/indexes and Elasticsearch mappings under their declared authority.
- Keep existing search aliases on the serving index until replacement validation succeeds.

## 2. Migrate

- Deploy readers that tolerate the transition and define the authoritative write path.
- Use dual writes only when necessary, with explicit failure/reconciliation semantics.
- Keep same-store related changes atomic where required and supported.
- Do not imply atomicity across stores or a database and Elasticsearch.
- Backfill in bounded batches with stable ordering, checkpoints and restart-safe predicates.
- Set retry budgets, concurrency limits, pause controls and progress/failure metrics.
- Preserve concurrent updates through conditional versions or another verified invariant guard.
- Avoid long transactions and remote effects inside transactions.
- Persist required publication/reindex intent durably with state when the store supports it.
- Reconcile counts, domain invariants and representative records before switching reads.

## 3. Contract

Retire all old application instances, workers, jobs and external readers first.
Delete obsolete columns/collections/indexes only in a separate approved release.
Tightening nullability, uniqueness or validators also requires existing-data validation.
Treat renames as expand/copy/switch/contract when compatibility requires it.
Search alias cutover must retain a tested rollback/rebuild path before deleting old indexes.

Before destructive or compatibility-restricting work:

- [ ] Consumer inventory confirms no remaining legacy dependency.
- [ ] Logs/telemetry show no legacy queries for at least 14 days where observable.
- [ ] Account for dormant jobs and consumers that telemetry cannot prove retired.
- [ ] Verified backup exists and restoration has been tested.
- [ ] Database Owner / Technical Lead explicitly authorizes the change in writing.
- [ ] Staging validates rollback or forward-fix, including application-version compatibility.
- [ ] Lock duration, resource budget and execution identity are approved.

## Provider and failure gates

Relational tests use each claimed real engine, not an in-memory substitute.
MongoDB transaction tests use a replica set or supported sharded deployment.
Elasticsearch tests cover reindex interruption, alias cutover and stale/deleted records.
DDL rollback semantics differ by engine; do not assume a transaction undoes migration DDL.
After timeout or connection loss, inspect migration/state history before retrying.
Distinguish confirmed commit, confirmed rollback and unknown outcome.
Replay ambiguous business mutations only through durable idempotency/outcome resolution.
Outbox delivery and multi-store reconciliation remain required after confirmed commit.

## Release evidence

Record reviewed assets, versions, approvals, test commands/results and recovery rehearsal.
Document any skipped checks and remaining compatibility or downtime risk.
Update affected schema/operations guidance in the same authorized change.
See [provider details](./relational-providers.md) and [transaction standards](../standards/persistence-and-concurrency.md).
No provider migrations are implemented or certified by the in-memory Items sample.
