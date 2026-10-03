# Runbook: Triaging & Troubleshooting Guide

> **Document Metadata**:  
> `Status: Draft` | `Owner: On-Call Engineering / SRE` | `Last verified: Not verified` | `Evidence: Standard operational runbook`

This runbook provides actionable diagnostic procedures for on-call engineers responding to operational incidents.

---

## 1. Initial Incident Triage (First 5 Minutes)

1. **Verify Impact & Scope**:
   - Check error rate dashboard (HTTP 5xx responses).
   - Check latency percentiles (p95 and p99).
   - Determine if the issue is global or isolated to specific tenants/endpoints.
2. **Inspect Process Health**:
   - Check container restart counts: `kubectl get pods -n <namespace> -l app=<app-name>`
   - Inspect liveness/readiness probe responses.
3. **Capture Trace Correlation**:
   - Identify a failing `TraceId` from customer report or logs.
   - Trace the request across gateway, service, database, and cache in the APM tool.

---

## 2. Common Failure Modes & Diagnostics

### High Latency / Connection Pool Exhaustion
- **Symptom**: Requests timeout; logs contain `TimeoutException: Timeout waiting for a pooled connection`.
- **Diagnostic Query**:
  - Check active DB connections vs. max connection pool size (`Database:MaxPoolSize`).
  - Check database server for unindexed long-running queries or lock contention:
    ```sql
    -- For PostgreSQL:
    SELECT pid, now() - query_start AS duration, query, state 
    FROM pg_stat_activity 
    WHERE state != 'idle' ORDER BY duration DESC LIMIT 10;
    ```
- **Remediation**: Kill long-running blocking query, scale database instances or increase pool size safely if DB capacity permits.

### Database Deadlock Victims
- **Symptom**: Logs contain `NpgsqlOperationException / SqlException: Deadlock detected`.
- **Remediation**:
  - Verify if transaction handles retries idempotently.
  - Review lock acquisition order in offending handler.

### Distributed Cache Outage
- **Symptom**: Latency spike as traffic falls back directly to the primary database.
- **Remediation**: Verify cache cluster connectivity, verify circuit breaker tripped to protect database from stampede, reboot cache node.

---

## 3. Escalation Path

- **L1 / On-Call Engineer**: Triage, assess severity, apply non-destructive restarts if safe.
- **Technical Lead / Database Admin**: Required for database query killing, schema locks, or rollback decisions.
- **Incident Commander**: Declares P1 incident, handles stakeholder communication.
