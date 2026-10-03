# [METHOD] /api/vX/<resource-route>

> **Endpoint Metadata**:  
> `Status: Draft | Verified | Deprecated`  
> `Owner: <Module Team / Feature Owner>`  
> `Module: <e.g., Orders, Identity, Billing>`  
> `Last verified: <Date or Commit>`  
> `Implementation: <path-to-controller/handler>`  
> `Contract Test: <path-to-contract-test>`

---

## 1. Overview & Purpose
Brief description of the business capability provided by this endpoint and intended callers.

---

## 2. Request Contract

### 2.1 Route & Parameters
- **HTTP Method**: `POST | GET | PUT | PATCH | DELETE`
- **Route**: `/api/v1/<resource>`
- **Authentication**: `Bearer JWT | API Key | Session Cookie | Anonymous`
- **Required Permissions**: `orders:create`
- **Tenant Scope**: Tenant context enforced from token claim.

### 2.2 Headers
| Header | Required | Type | Description |
|---|---|---|---|
| `Authorization` | Yes | string | `Bearer <token>` |
| `Idempotency-Key` | Conditional | string | Required for non-idempotent mutations |

### 2.3 Query / Path Parameters
| Parameter | In | Required | Type | Description |
|---|---|---|---|---|
| `id` | Path | Yes | string / UUID | Unique resource identifier |

### 2.4 Request Body
```json
{
  "amount": 100000.00,
  "currency": "IDR",
  "note": "Optional transaction note"
}
```

---

## 3. Response Contract

### 3.1 Success Response (`201 Created` / `200 OK`)
```json
{
  "id": "ord_01HXYZ1234567890",
  "status": "Created",
  "amount": 100000.00,
  "currency": "IDR",
  "createdAt": "2026-10-03T12:00:00Z"
}
```

### 3.2 Error Responses
| HTTP Status | Error Code | Triggering Condition |
|---|---|---|
| `400 Bad Request` | `VALIDATION_FAILED` | Amount is zero or negative |
| `401 Unauthorized` | `UNAUTHENTICATED` | Token expired or invalid |
| `403 Forbidden` | `PERMISSION_DENIED` | Caller lacks `orders:create` permission |
| `409 Conflict` | `CONCURRENCY_CONFLICT`| Simultaneous mutation or duplicate idempotency key |

---

## 4. Side Effects, Invariants & Concurrency

- **Database Mutations**: Inserts new order entity and audit log entry atomically.
- **Integration Events**: Emits `OrderCreatedIntegrationEvent` via Transactional Outbox post-commit.
- **Idempotency Guarantee**: Deduplication on `Idempotency-Key` stored in database.
- **Ambiguous Outcomes**: If network timeout occurs during commit, query order status by idempotency key before retrying.
