# Persistence, Schema Authority, Transactions, and Concurrency Standards

> **Classification:** `[CORE STANDARD]`  
> **Status:** Normative Standard  
> **Applicability:** Active for any task involving data mutations, database schema, transactions, concurrency control, or financial persistence.  
> **Source Migration:** Formed from Section 6.3 (Command Lifecycle) and Section 9 (Persistence, Schema Authority, Transactions, and Concurrency) of the Enterprise .NET Backend AI Engineering Guide.

---

## 6.3 Command Lifecycle

Apply steps that are relevant to the use case:
1. Validate contract input.
2. Resolve authenticated actor and tenant scope.
3. Load required state without redundant reads, but do not treat a pre-transaction snapshot as protection of mutable invariants.
4. Return a not-found/authorization-safe outcome.
5. Return early for verified idempotent or no-change paths, respecting current authorization and durable operation state.
6. Open a transaction immediately before mutation when atomicity requires one.
7. Enforce or revalidate mutable invariants at the authoritative write boundary using constraints, atomic conditional writes, concurrency tokens, or appropriate locking/isolation. This includes balances, stock, uniqueness, ownership, and authorization state whose stability matters. A transaction alone does not make an earlier read current; handle conflicts explicitly.
8. Execute invariant-preserving atomic mutations and persist required outbox/idempotency records atomically with their associated state. Commit successfully or ensure rollback through explicit handling or verified transaction-disposal semantics.
9. Release external effects only after confirmed commit. In-process post-commit notifications are best-effort and may be lost on process failure; required effects need durable outbox or equivalent persisted recovery intent, retries, deduplication, and reconciliation ([distributed-systems.md Section 10.2](./distributed-systems.md#102-messaging-and-outbox)). Do not substitute untracked fire-and-forget for required work.
10. Return the selected compatible contract; where the Clean Architecture profile applies, return a transport-neutral result for Presentation mapping.

Transactions MUST be short. Calls to the database resource participating in the transaction are permitted. Unrelated remote calls and irreversible external effects MUST NOT execute while it is open. Cross-resource atomic protocols require an explicitly selected, verified design; otherwise use the multi-resource workflow rules in [distributed-systems.md Section 10.6](./distributed-systems.md#106-multi-resource-workflows-sagas-and-compensation). An exceptional deviation requires a time-bounded formal waiver with bounded timeouts, lock/connection impact and partial-failure analysis, compensating controls, and evidence that no post-commit or durable-intent design satisfies the invariant.

Distinguish transaction outcomes explicitly:
- If cancellation is observed before commit begins, stop further mutations, do not initiate commit, and ensure rollback through explicit handling or verified disposal semantics.
- Once commit has been initiated, cancellation, timeout, or connection loss can leave the outcome unknown. Do not infer rollback from an exception or blindly replay the mutation. Resolve the outcome through authoritative state and durable idempotency records; retain an unknown/pending outcome until reconciliation establishes it.
- After confirmed commit, the operation remains authoritative even if the request is canceled or the client disconnects. Required remaining effects MUST continue through durable outbox/recovery mechanisms. Durable operation state and logs MUST distinguish confirmed commit, confirmed rollback, and unknown outcome; a response MUST do so only while a response channel remains available.

---

## 9. Persistence, Schema Authority, Transactions, and Concurrency

### 9.1 Schema Authority Profiles

A repository SHOULD explicitly declare, or permit evidence-based discovery of, one profile:
- database-first;
- migration/code-first;
- externally managed schema;
- hybrid or platform-managed schema with explicit object ownership, migration order, compatibility responsibility, and production deployment authority.

Unresolved shared ownership MUST block only a requested persistence change whose safety or compatibility depends on that authority; it does not authorize unrelated governance artifacts. For repository-owned relational schemas, production changes requiring rolling compatibility MUST use Expand-Migrate-Contract:
1. **Expand** with backward-compatible schema.
2. **Migrate** application versions and bounded backfills.
3. **Contract** only after old readers/writers are retired and verified.

Externally managed schemas MUST follow the owning authority's verified compatible-evolution process. Destructive migrations require backup/recovery analysis, consumer analysis, and explicit approval.

### 9.2 Query and SQL Standards

- Parameterize SQL and dynamic predicates; never concatenate untrusted values.
- Read paths SHOULD project only required columns and avoid tracking when entity mutation is unnecessary.
- Avoid unbounded or materially expensive N+1 patterns and premature materialization. A bounded intentional multiple-query design MAY be retained when its limit and total round-trip, transfer, locking, and provider behavior are justified by evidence.
- Use eager loading, split queries, projections, batching, or purpose-built SQL based on measured query behavior.
- Inspect actual execution plans before claiming an index improves performance. Consider write amplification, selectivity, storage, and maintenance.
- Propagate the caller token to supported request-bound database work before commit. Commit, rollback, cleanup, and authoritative outcome resolution MAY use separately owned bounded tokens as defined in [section 6.3](#63-command-lifecycle) and [resilience-and-workers.md Section 11.5](./resilience-and-workers.md#115-cancellation-and-work-ownership).

### 9.3 Independent Concurrency Decisions

Choose the lowest-complexity combination that guarantees the invariants; decide each independent axis:

- **Invariant/concurrency control:** constraints, unique indexes, atomic conditional statements, ledger invariants, optimistic tokens, or bounded pessimistic locks.
- **Repeat-execution control:** intrinsic idempotency, durable idempotency/deduplication keys, persisted outcomes, and reconciliation for ambiguous or inherently non-repeatable effects.
- **Cross-node coordination:** distributed leases only when coordination cannot be protected by one authoritative datastore.

Controls across axes are complementary. Financial workflows commonly require both authoritative concurrency control and durable repeat-execution protection.

Distributed leases require an ADR, bounded lease duration, fencing tokens or equivalent stale-owner protection, ownership-safe release, renewal limits, clock/failure analysis, and concurrency/failover tests. A lock alone MUST NOT be the source of financial truth.

### 9.4 Isolation Levels, Locking, and Deadlocks

- Select transaction isolation from the invariant, anomaly tolerance, and measured contention; do not raise isolation globally as a default fix.
- Keep lock scope bounded and use consistent lock acquisition ordering across code paths.
- Monitor blocking, lock waits, transaction duration, and deadlock graphs where the datastore exposes them.
- Retry deadlock victims only when the complete operation is safely repeatable or protected by durable idempotency, and keep retry budgets finite.
- Test concurrency behavior using the real persistence engine when correctness depends on provider-specific isolation or locking semantics.

### 9.5 Connection-Pool Governance

- Size pools from measured workload, dependency latency, instance concurrency, and database capacity rather than arbitrary maxima.
- Bound connection acquisition time and propagate cancellation while waiting.
- Monitor active, idle, waiting, timeout, and saturation indicators.
- Protect the database from retry storms and cache-outage fallback traffic with concurrency limits and load shedding.
- Apply tenant-aware noisy-neighbor controls when shared capacity and measured risk justify them.

### 9.6 Clock and Time Semantics

- Persist timestamps in UTC unless a domain contract explicitly requires another representation.
- Use monotonic elapsed-time sources for durations, deadlines, and timeout measurement.
- Model business time zones explicitly and never depend on the server's local time zone.
- Account for clock skew in authentication, leases, and distributed coordination.
- Timestamps alone MUST NOT be treated as a distributed ordering guarantee.

### 9.7 Identity Strategy

Select database-generated identity, UUID v7, ULID, Snowflake-style IDs, or another scheme based on topology, shard independence, index locality, storage cost, exposure/privacy, ordering semantics, and collision guarantees. A system-wide identity change requires an ADR and migration plan. No strategy is universally preferred.

### 9.8 Monetary Representation

For monetary values, represent currency explicitly and define supported precision, scale, ranges, and conversion semantics. Use an appropriate decimal representation or checked integer minor units according to the domain and currency; not every currency has two decimal places. Unsuitable binary floating-point monetary arithmetic is forbidden.

Specify rounding mode and stage, allocation/remainder rules, overflow handling, exchange-rate precision/source/time, and cross-currency conversion explicitly. Align application, serialization, and storage representations so persistence or transport cannot silently truncate, round, or reinterpret amounts. Test boundary values, negative/zero rules, rounding, overflow, and conversions alongside the concurrency, ledger, and durable idempotency requirements; numeric type selection alone does not make money safe.
