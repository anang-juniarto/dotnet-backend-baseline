# Testing, Performance, Recovery, and Quality Gate Standards

> **Classification:** `[CORE STANDARD]`  
> **Status:** Normative Standard  
> **Applicability:** Active when designing, authoring, or evaluating automated tests, performance validation, and recovery quality gates.  
> **Source Migration:** Formed from Section 15 of the Enterprise .NET Backend AI Engineering Guide.

---

## 15. Testing, Performance, Recovery, and Quality Gates

### 15.1 Test Strategy

Use a risk-based test portfolio:
- unit tests for invariants and pure behavior;
- handler/use-case tests for orchestration;
- integration tests for persistence, cache, broker, external adapters, and concurrency;
- contract tests for published HTTP and message contracts;
- end-to-end tests for a small set of critical journeys;
- architecture tests where automated boundaries provide value.

Use existing approved frameworks. Adding test infrastructure requires authorization. Where applicable, tests also cover conditional HTTP preconditions, privilege escalation, event schema compatibility, connection-pool saturation, saga compensation and recovery, reconciliation/replay safety, and clock-skew-sensitive behavior.

### 15.2 Required Behavioral Coverage

For a changed use case, cover applicable scenarios:
- valid and invalid input;
- authorization and cross-tenant denial;
- not found;
- no-change/idempotent execution;
- successful mutation;
- rollback and partial-failure prevention;
- external failure before/after commit;
- cancellation, timeout, concurrency conflict, duplicate delivery, and replay.

### 15.3 Determinism

Control time through the verified repository abstraction or `TimeProvider` when available on the selected runtime; do not infer runtime support from [core-governance.md Section 4.2](./core-governance.md#42-repository-discovery). Isolate mutable state, avoid external network dependencies in unit tests, and make random identifiers deterministic when assertions require it.

### 15.4 Performance Validation

Critical throughput paths require budgets and reproducible evidence. Select load, spike, stress, soak, concurrency, and benchmark tests according to risk. Record dataset, environment, concurrency, duration, warm-up, percentiles, resource usage, and acceptance thresholds. Never generalize local microbenchmarks into production capacity claims.

### 15.5 Contract, Event, and Recovery Tests

- Validate response shapes, status codes, error codes, headers, and compatibility.
- Validate message backward/forward compatibility and duplicate/replay handling.
- Test backup restoration, failover, and disaster procedures at a cadence proportional to business impact.
