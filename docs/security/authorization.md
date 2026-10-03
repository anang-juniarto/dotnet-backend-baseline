# Authorization Policies & Multi-Tenant Isolation

> **Document Metadata**:  
> `Status: Draft` | `Owner: Security & Identity Team` | `Last verified: Not verified` | `Evidence: Authorization matrix`

This document defines authorization models, policy evaluation mechanisms, and multi-tenant isolation guarantees.

---

## 1. Authorization Model

The service enforces **Policy-Based Authorization** paired with **Object-Level Ownership**:

1. **Role / Permission Check**:
   - Evaluated by authorization handlers before use-case execution.
   - Example permissions: `orders:read`, `orders:create`, `billing:payout`.
2. **Resource Ownership Check**:
   - Evaluated within the application/domain layer:
     ```csharp
     if (order.TenantId != currentActor.TenantId || order.UserId != currentActor.UserId)
     {
         return Result.Failure(DomainErrors.Common.Forbidden);
     }
     ```

---

## 2. Multi-Tenant Isolation Guarantees

In multi-tenant deployments, cross-tenant data leakage is a critical security vulnerability:

- **Tenant Resolution**: Tenant ID is extracted from verified token claims or authenticated API keys. Client-supplied route or header `X-Tenant-ID` is untrusted unless authenticated.
- **Query Scoping**: Every database read and write MUST explicitly include the tenant filter:
  - `WHERE tenant_id = @currentTenantId`
  - Global query filters (e.g., in EF Core) serve as defense-in-depth, NOT a replacement for explicit tenant-scoped domain queries.
- **Cache Isolation**: All distributed cache keys MUST be tenant-prefixed:
  - Format: `tenant:{tenantId}:{entity}:{key}`
- **Message Isolation**: Integration events and queue messages carry `TenantId` metadata; consumers validate tenant context before processing.

---

## 3. Administrative & Break-Glass Access

- Impersonation or administrative bypass mechanisms require:
  - Time-bounded authorization token.
  - Mandatory audit log recording the authentic operator identity, target entity, and business justification.
  - Alert notification dispatched to security monitoring.
