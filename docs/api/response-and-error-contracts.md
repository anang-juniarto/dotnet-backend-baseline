# API Response & Error Contracts

> **Document Metadata**:  
> `Status: Draft` | `Owner: API Governance Team` | `Last verified: Not verified` | `Evidence: Contract specifications`

This document defines standard response formats, error envelopes, and machine-readable error codes.

---

## 1. Response Profiles

The repository MUST adhere to one consistent response profile across a given major API version:

### Profile A: Plain Success DTOs + RFC 9457 Problem Details (Recommended)
- **Successful GET / POST / PUT**: Returns the pure domain DTO directly.
- **Errors**: Formatted strictly according to RFC 9457 `application/problem+json`.

```json
{
  "type": "https://api.example.com/errors/insufficient-funds",
  "title": "Insufficient Funds",
  "status": 400,
  "detail": "Wallet balance 50000 IDR is insufficient for requested withdrawal of 100000 IDR.",
  "instance": "/api/v1/wallets/w-123/withdraw",
  "errorCode": "WALLET_INSUFFICIENT_FUNDS",
  "traceId": "00-4bf92f3577b34da6a3ce929d0e0e4736-00f067aa0ba902b7-01"
}
```

### Profile B: Uniform Envelopes (`ResponseModel<T>`)
If existing consumers require a wrapper envelope:
```json
{
  "success": true,
  "data": { ... },
  "message": "Operation completed successfully.",
  "errors": null,
  "traceId": "00-4bf92f3577b34da6a3ce929d0e0e4736-00f067aa0ba902b7-01"
}
```
*Rule: Do NOT migrate an existing legacy envelope profile without an approved contract migration plan.*

---

## 2. Standard Machine-Readable Error Codes

Clients MUST bind business error handling to stable machine-readable `errorCode` strings, NEVER to human-readable `detail` messages:

| HTTP Status | Error Code | Description |
|---|---|---|
| `400 Bad Request` | `VALIDATION_FAILED` | Request payload failed syntax or schema constraints. |
| `401 Unauthorized` | `UNAUTHENTICATED` | Missing, expired, or invalid credential. |
| `403 Forbidden` | `PERMISSION_DENIED` | Authenticated actor lacks privilege for this resource. |
| `404 Not Found` | `RESOURCE_NOT_FOUND` | Target entity does not exist or belongs to another tenant. |
| `409 Conflict` | `CONCURRENCY_CONFLICT` | Application state/version conflict without a failed HTTP precondition; duplicate resources use a distinct stable code. |
| `412 Precondition Failed` | `PRECONDITION_FAILED` | An evaluated HTTP precondition such as `If-Match` is false. |
| `429 Too Many Requests`| `RATE_LIMIT_EXCEEDED` | Request quota exhausted; consult `Retry-After`. |
| `500 Server Error` | `INTERNAL_SERVER_ERROR` | Unexpected failure; internal details redacted from response. |

---

## 3. Validation and Transport Boundaries

- Selected greenfield .NET 10 uses controllers; `[ApiController]` model validation can produce `400` ValidationProblemDetails with field-keyed `errors`. Characterize the actual configured response.
- Normalize manual and automatic validation to the published contract; `errorCode`/`traceId` extensions are not automatically guaranteed by every framework error path.
- .NET 10 Minimal API validation is a separate opt-in path with version/package-specific registration; do not assume controller behavior, experimental API stability, or automatic handler/business validation.
- Validate application invariants in typed handlers regardless of transport. Preserve an existing published `422` or envelope contract unless migration is approved.
- `If-Match` failure maps to `412`; an application concurrency conflict without a failed HTTP condition maps to `409`. Document required-but-missing preconditions separately if selected.
- OpenAPI metadata must match actual validation/error responses and the selected emitted OpenAPI version; generation alone does not prove runtime enforcement.
- Native gRPC status/errors and SignalR invocation errors are separately versioned contracts, not HTTP Problem Details envelopes.
- The in-memory Items sample does not prove durable concurrency, ETag handling or optional transport behavior.

## 4. Security Redaction & Information Leakage Prevention

Error responses MUST NOT reveal:
- SQL statements or database error messages.
- Server filesystem paths or assembly names.
- Raw exception type names or unhandled stack traces.
- Connection strings, API keys, or infrastructure IP addresses.
