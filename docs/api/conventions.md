# Global API Conventions & Protocol Standards

> **Document Metadata**:  
> `Status: Draft` | `Owner: API Governance Team` | `Last verified: Not verified` | `Evidence: API convention standards`

This document defines standardized conventions for HTTP request/response payloads, headers, pagination, and data representations.

---

## 1. HTTP Methods & Route Semantics

- **Routes**: Use lowercase, hyphenated (kebab-case) resource routes (e.g., `/api/v1/user-profiles`).
- **HTTP Verbs**: Follow RFC 9110 semantics:
  - `GET`: Safe, idempotent read operations.
  - `POST`: Create new resources or execute non-idempotent operations. Returns `201 Created` with `Location` header or `200 OK`.
  - `PUT`: Complete idempotent resource replacement.
  - `PATCH`: Partial resource modification.
  - `DELETE`: Idempotent resource removal. Returns `204 No Content` or `200 OK`.

---

## 2. Data Types & Formats

- **Date & Time**: Represent timestamps in ISO 8601 UTC format (e.g., `2026-10-03T12:00:00Z`). Local timestamps without offset are FORBIDDEN.
- **Monetary Values**: Pair numerical amount with ISO 4217 currency code:
  ```json
  {
    "amount": 150000.00,
    "currency": "IDR"
  }
  ```
- **Null vs Omitted**:
  - Omitted fields in PATCH: Field is unchanged.
  - Explicit `null`: Field is cleared.

---

## 3. Pagination, Filtering & Sorting

- **Pagination Parameters**:
  - `page`: 1-based page index (default: `1`).
  - `pageSize`: Number of items per page (default: `20`, maximum: `100`).
- **Keyset / Cursor Pagination**: Preferred for high-volume or deep-offset collections (`afterCursor=<opaque-token>`).
- **Sorting**: Allow-list sorting fields; always append a deterministic tie-breaker (e.g., `id asc`).

---

## 4. Standard Headers & Governance

| Header | Direction | Description |
|---|---|---|
| `X-Correlation-Id` | Request / Response | W3C / Correlation ID propagated across distributed traces. |
| `Idempotency-Key` | Request | Unique key ensuring safe replay of mutating POST requests. |
| `If-Match` / `ETag` | Request / Response | Optimistic concurrency token preventing lost updates (`412 Precondition Failed`). |
| `Retry-After` | Response | Seconds or date to wait after receiving `429 Too Many Requests`. |
