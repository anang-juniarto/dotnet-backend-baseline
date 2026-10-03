# Schema Change Process & Zero-Downtime Migration Policy

> **Document Metadata**:  
> `Status: Draft` | `Owner: Database Engineering Team` | `Last verified: Not verified` | `Evidence: Migration workflow standard`

This document defines the mandatory process for modifying database schemas while ensuring zero-downtime, rolling release safety, and data integrity.

---

## 1. Expand-Migrate-Contract Pattern

Destructive schema changes in a single deployment are FORBIDDEN. All changes requiring rolling application compatibility MUST follow the three-phase Expand-Migrate-Contract lifecycle:

```text
Phase 1: EXPAND          Phase 2: MIGRATE          Phase 3: CONTRACT
(Additive Schema)      (Data & Dual Write)        (Cleanup Deprecated)

Add new column/table   Deploy code reading new    Drop old column/table
Old & new code safe    Backfill historical data   Old readers retired
```

### Phase 1: Expand (Additive)
- Add new tables, nullable columns, or backward-compatible indexes.
- Old versions of the application continue reading/writing without failure.

### Phase 2: Migrate (Transition)
- Deploy application version that writes to the new schema and falls back to old data if needed.
- Execute bounded asynchronous backfill scripts for historical records.

### Phase 3: Contract (Cleanup)
- Once all application instances and external readers are verified running on the new version:
- Drop obsolete columns, constraints, or tables in a separate subsequent deployment.

---

## 2. Destructive Migration Safeguards

Before any column drop, table rename, or constraint restriction:
- [ ] Verified backup taken and restoration tested.
- [ ] Confirmed zero queries referencing the legacy column in logs/telemetry for at least 14 days.
- [ ] Explicit written authorization from Database Owner / Technical Lead.
- [ ] Rollback or forward-fix script verified in staging.
