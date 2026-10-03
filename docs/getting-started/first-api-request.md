# First API Request Guide

> **Document Metadata**:  
> `Status: Draft` | `Owner: Unknown` | `Last verified: Not verified` | `Evidence: None yet`

This guide assists engineers and automated test agents in verifying that the local service is responding correctly.

---

## 1. Safety & Execution Boundary

- **Target**: Local development environment only (e.g., `http://localhost:<port>`).
- **Data**: Synthetic/test data only.
- **Rule**: NEVER point test requests at production or staging without explicit authorization.

---

## 2. Health & Readiness Probe Check

Before invoking business endpoints, verify process liveness and readiness:

```bash
# Liveness probe (checks process health)
curl -i http://localhost:<port>/health/live

# Readiness probe (checks critical local dependencies)
curl -i http://localhost:<port>/health/ready
```

Expected output:
```http
HTTP/1.1 200 OK
Content-Type: application/json

{"status":"Healthy"}
```

---

## 3. Invoking a Sample Business Request

*Note: Replace `<port>`, `<module>`, and `<resource>` with discovered routes from Swagger/OpenAPI or [`docs/api/README.md`](../api/README.md).*

### Request Example (curl)
```bash
curl -i -X GET http://localhost:<port>/api/v1/<resource> \
  -H "Accept: application/json" \
  -H "Authorization: Bearer <local-synthetic-token>"
```

### Response Validation Checklist
- [ ] Status Code matches expected contract (`200 OK`, `201 Created`, etc.).
- [ ] Headers contain expected `Content-Type: application/json`.
- [ ] Response body matches contract format documented in [`docs/api/response-and-error-contracts.md`](../api/response-and-error-contracts.md).
- [ ] No internal stack traces, connection strings, or server exceptions are leaked in error responses.
