# Observability, Structured Telemetry & Health Probes

> **Document Metadata**:  
> `Status: Draft` | `Owner: SRE / Platform Engineering` | `Last verified: Not verified` | `Evidence: OpenTelemetry baseline`

This document defines standards for structured logging, distributed tracing, OpenTelemetry metrics, and container health probes.

---

## 1. Structured Logging Standards

All application logs MUST be emitted as structured JSON with named message templates:

- **Message Templates**: Use parameterized message templates; string interpolation in log messages is FORBIDDEN:
  ```csharp
  // Correct:
  _logger.LogInformation("Processing payment for OrderId: {OrderId} Amount: {Amount}", order.Id, order.Amount);

  // FORBIDDEN:
  _logger.LogInformation($"Processing payment for OrderId: {order.Id}");
  ```
- **Context Properties**: Standard log scopes MUST include:
  - `TraceId` / `SpanId` (W3C standard)
  - `TenantId` (when executing in tenant context)
  - `Environment`
- **Zero Sensitive Data**: Passwords, authorization tokens, credit card numbers, or full raw payloads MUST NOT be logged.

---

## 2. Metrics & Cardinality Governance

- Follow OpenTelemetry semantic conventions for HTTP, database, and messaging meters.
- **Cardinality Protection**: NEVER use unbounded identifiers (`userId`, `orderId`, raw request URLs with query parameters) as metric tag/label dimensions. Doing so exhausts metric storage and crashes telemetry backends.
- Use bounded route templates (e.g., `/api/v1/orders/{id}`) and HTTP status codes for metric dimensions.

---

## 3. Health Probe Architecture

ASP.NET Core Health Checks MUST be exposed on dedicated internal endpoints:

| Endpoint | Probe Type | Purpose | Dependencies Checked |
|---|---|---|---|
| `/health/live` | **Liveness** | Verifies process is running and not deadlocked | Internal process responsiveness only. NO remote calls. |
| `/health/ready`| **Readiness**| Verifies service can accept user traffic | Critical database & cache connectivity. |
| `/health/startup`| **Startup** | Protects slow initialization | Warm-up tasks, initial cache loading. |

*Rule: An optional or non-critical downstream service failure MUST NOT cause readiness probes to fail and remove traffic.*
