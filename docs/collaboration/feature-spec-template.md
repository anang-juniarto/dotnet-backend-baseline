# Feature Specification: [Feature Name]

> **Specification Metadata**:  
> `Status: Draft | Approved | In Progress | Completed`  
> `Author(s): <Name / Team>`  
> `Technical Reviewer(s): <Name>`  
> `Target Release: <vX.Y.Z / Unscheduled>`  
> `Alignment: docs/engineering/change-delivery-contract.md`  
> `Last Updated: YYYY-MM-DD`

---

## 1. Problem Statement & Business Context
Describe the customer problem, business driver, or system capability required.

---

## 2. Goals & Non-Goals

### Goals
- Goal 1: ...
- Goal 2: ...

### Non-Goals (Out of Scope)
- Explicitly out of scope: ...

---

## 3. Actors, Tenancy & Security Scope
- **Actors**: End user, administrator, automated webhook caller.
- **Tenant Isolation**: Does this feature operate across multiple tenants? How is tenant scope enforced?
- **Required Permissions**: List permissions required to access or execute this feature.

---

## 4. Functional Behavior & Contract Changes

### 4.1 API Contracts
- New endpoints, methods, and routes.
- Request payload DTO schema and validation rules.
- Response payload DTO schema and error codes.

### 4.2 Data & Persistence Effects
- New tables, columns, indexes, or relationships.
- Does this require an Expand-Migrate-Contract schema migration?

### 4.3 Invariants, Concurrency & Idempotency
- **State Invariants**: What rules must never be violated (e.g., wallet balance cannot be negative)?
- **Concurrency Control**: Optimistic row versions, row locks, or atomic SQL conditions.
- **Idempotency Strategy**: Is an `Idempotency-Key` required? How is deduplication stored?

---

## 5. Failure Modes, Resilience & Ambiguous Outcomes
- What happens if downstream calls timeout after commit?
- What are the retry limits and backoff strategies?

---

## 6. Acceptance Criteria (Given / When / Then)

```gherkin
Scenario: Successful creation with valid payload
  Given an authenticated user with permission "orders:create"
  When a POST request is sent to "/api/v1/orders" with valid payload
  Then return 201 Created with order ID and Location header
  And emit OrderCreatedIntegrationEvent via Transactional Outbox
```

---

## 7. Open Questions & Technical Decisions
- [ ] Question 1: ...
- [ ] Question 2: ...
