# Database & Storage Overview

> **Document Metadata**:  
> `Status: Draft` | `Owner: Database Engineering / Architecture` | `Last verified: Not verified` | `Evidence: Storage architecture standard`

This document defines the storage boundaries, relational data models, and persistence technologies used by the backend service.

---

## 1. Storage Boundaries & Technologies

*Note: Verify actual databases and connection profiles from repository manifests.*

| Storage Role | Engine / Technology | Primary Use Case | Isolation Boundary |
|---|---|---|---|
| **Primary Relational Store** | PostgreSQL / SQL Server / MySQL *(TBD)* | Authoritative business entities, transactions, ledgers | Transactional (ACID) |
| **Distributed Cache** | Redis / Valkey *(TBD)* | Read-through caching, session state, rate limit counters | Volatile / Ephemeral |
| **Outbox / Event Storage** | Database Table / Event Log *(TBD)* | Transactional outbox for reliable integration event delivery | Part of DB transaction |

---

## 2. Core Entities & Relationships

*(To be populated as features and entities are implemented)*

- **Entity Model**:
  - `User / Account`: Authentication credentials, roles, profile.
  - `Tenant`: Organizational isolation container.
  - `Transaction / LedgerEntry`: Immutable financial records.
- **Rules**:
  - Primary keys: UUIDv7, ULID, or BIGINT identity (chosen per performance profile).
  - Foreign keys and unique constraints MUST be enforced in the authoritative datastore.

---

## 3. Data Integrity & Monetary Storage

1. **Monetary Representation**:
   - Currency MUST be explicitly modeled (ISO 4217).
   - Numerical values MUST use exact `decimal(18, 4)` or integer minor units (e.g., cents).
   - Binary floating-point types (`float`, `double`, `real`) are FORBIDDEN for money.
2. **Audit & Soft Deletion**:
   - High-value business entities SHOULD track `CreatedAt`, `CreatedBy`, `UpdatedAt`, `UpdatedBy`.
   - Hard deletion is prohibited on financial or audit-sensitive tables.
