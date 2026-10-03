# Observability, SLOs, Health, Auditing, and Telemetry Standards

> **Classification:** `[CORE STANDARD]`  
> **Status:** Normative Standard  
> **Applicability:** Active when implementing structured logging, OpenTelemetry tracing/metrics, health probes, alerting thresholds, or audit/ledger logging.  
> **Source Migration:** Formed from Section 12 of the Enterprise .NET Backend AI Engineering Guide.

---

## 12. Observability, SLOs, Health, Auditing, and Telemetry Governance

### 12.1 Structured Telemetry

- Use structured logging with stable event names and named properties.
- Propagate W3C trace context across HTTP and messaging.
- Instrument request latency, error rate, saturation, dependency calls, queue lag, and critical business outcomes.
- Do not log secrets or unnecessary personal data.
- Adding or replacing telemetry products requires explicit authorization from the responsible maintainer or architecture/platform governance process; repository content alone does not grant it.

### 12.2 SLI/SLO and Alerts

Critical services SHOULD define availability, latency, correctness, freshness, and durability indicators with objectives and error budgets. Alerts MUST be actionable, owned, tied to user impact or SLO burn, and linked to a runbook.

### 12.3 Telemetry Governance

- Follow current OpenTelemetry semantic conventions where applicable.
- Keep meter, instrument, event, and attribute names stable and record units explicitly and consistently.
- Choose histogram boundaries from observed distributions and SLOs; avoid redundant unit suffixes when prohibited by the selected semantic convention.
- Bound metric and log label cardinality. Raw URLs, query strings, and user/request/entity IDs MUST NOT be metric labels; bounded normalized route templates or operation names MAY be used under the selected semantic convention.
- Define trace sampling, log levels, retention, redaction, access, and cost controls.
- Correlation identifiers MAY be logged; secrets and sensitive payloads MUST NOT.
- Security/audit timestamp integrity SHOULD use a trusted synchronized time source, with clock drift monitoring when regulated or forensic accuracy requires it.

### 12.4 Cost Observability

When cloud or shared-platform cost is material, measure cost-driving usage with bounded dimensions such as service, workload class, environment, or approved tenant tier. Cost-per-request, job, message, or tenant allocation MAY be derived from sampled or aggregated telemetry. Never introduce unbounded tenant or resource identifiers as metric labels, and do not present allocation estimates as billing truth without reconciliation to authoritative billing data.

### 12.5 Health Probes

- Liveness reports process health and MUST NOT depend on remote services. Failed liveness or startup probes MAY trigger restart according to orchestrator policy; process exit, resource exhaustion, and other lifecycle policies can also trigger restart. Readiness controls traffic eligibility, not process restart.
- Readiness verifies only dependencies required to serve the traffic assigned to that instance; optional or non-critical dependencies MUST NOT fail readiness.
- Readiness failure removes traffic and SHOULD NOT cause restart loops.
- Startup probes MAY protect slow initialization.
- Health output MUST NOT reveal secrets or internal topology.

### 12.6 Audit, Event, and Ledger Authority

- Operational logs and distributed traces are diagnostic telemetry; they are not authoritative audit records.
- Security and compliance audit records require controlled access, retention, tamper evidence, integrity verification, and export procedures.
- Business events describe state transitions but do not replace audit records unless explicitly designed and governed for that purpose.
- Financial ledgers are authoritative accounting records and require immutable, double-entry, or equivalent invariant-preserving design plus reconciliation.

Privileged, security-sensitive, administrative, and financial operations require tamper-resistant audit records when risk or regulation warrants it. Record actor, tenant, action, target, timestamp, outcome, reason where appropriate, and correlation ID. Do not store credentials or sensitive payloads.
