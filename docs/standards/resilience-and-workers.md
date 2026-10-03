# Resilience, Timeouts, Retries, Idempotency, and Workers Standards

> **Classification:** `[CORE STANDARD]`  
> **Status:** Normative Standard  
> **Applicability:** Active when designing timeout budgets, retry policies, durable idempotency, background job processing, CancellationToken handling, or graceful degradation.  
> **Source Migration:** Formed from Section 11 of the Enterprise .NET Backend AI Engineering Guide.

---

## 11. Resilience, Timeouts, Retries, Idempotency, and Workers

### 11.1 Timeout Budgets

Derive timeouts from end-to-end SLOs, remaining request budget, dependency latency distribution, and cancellation behavior. Illustrative starting ranges—not universal mandates—may be:
- cache: hundreds of milliseconds;
- ordinary database reads: low single-digit seconds;
- external HTTP: low single-digit seconds;
- inbound request: tens of seconds.

Child operations MUST leave time for upstream handling and cleanup. Record exceptions for long-running jobs, streaming, and batch workloads.

### 11.2 Retry Safety

- Retry only transient failures on operations that are safely repeatable or protected by durable idempotency.
- Do not retry validation, authorization, business rejection, or other permanent failures.
- Do not automatically retry ambiguous non-idempotent writes unless reconciliation can determine the committed outcome.
- Bound attempts and elapsed time; use exponential backoff with jitter and honor server retry hints.
- Use circuit breaking, concurrency limiting, and load shedding when failure could cascade.
- A resilience library is an implementation choice, not a universal package mandate.

### 11.3 Idempotency

For high-risk non-idempotent operations, define an idempotency contract containing tenant/actor scope, key, canonical request fingerprint, in-flight behavior, durable result, retention, and conflict semantics.

For payments, payouts, wallet debits/credits, ledger postings, and comparable financial operations, idempotency records MUST reside in a durable authoritative store and persist scope, fingerprint, in-flight state, committed result, timestamps, and retention policy. A volatile cache lock MAY coordinate requests but MUST NOT be the financial source of truth. Ambiguous outcomes require reconciliation before a retry is accepted.

### 11.4 Background Workers

When background processing applies:
- bound concurrency and queues;
- propagate cancellation and support graceful shutdown/drain;
- make processing safely repeatable through intrinsic idempotency, durable deduplication/provider keys, persisted outcome state, and reconciliation for inherently non-repeatable effects;
- distinguish retryable and permanent failures;
- quarantine poison messages/jobs and alert an owner;
- expose lag, age, throughput, failure, retry, and dead-letter metrics;
- document replay and recovery procedures.

### 11.5 Cancellation and Work Ownership

Propagate request cancellation through supported request-bound I/O and stop unnecessary work before commit. Workers use their own operation deadlines and host shutdown/drain tokens rather than retaining a caller's request token. Required work MUST NOT run as untracked fire-and-forget; use an owned, bounded worker with durable recovery where required.

Rollback, cleanup, and reconciliation may need a separate bounded token when the request token is already canceled. Establish ownership, timeout, and recovery if cleanup cannot finish; do not replace cancellation globally with an unbounded non-cancelable operation. Classify expected caller/shutdown cancellation separately from operational failure without hiding timeouts, unknown commit outcomes, or failed cleanup. Preserve [persistence-and-concurrency.md Section 6.3's](./persistence-and-concurrency.md#63-command-lifecycle) confirmed-commit, confirmed-rollback, and unknown-outcome distinctions: cancellation never proves rollback after commit begins, and required post-commit effects survive request cancellation through durable mechanisms.

### 11.6 Graceful Degradation

Degrade only non-critical capabilities. Stale data requires a defined maximum age, visible freshness metadata or standard warning semantics, and must never be used where stale state could violate authorization, financial, inventory, or safety invariants.
