# Runbook: Backup, Recovery & Disaster Restoration

> **Document Metadata**:  
> `Status: Draft` | `Owner: Database Engineering / SRE` | `Last verified: Not verified` | `Evidence: Disaster recovery blueprint`

This runbook establishes backup policies, Recovery Point / Time Objectives (RPO/RTO), and procedures for verifying database restores.

---

## 1. Objectives & Responsibility Model

- **Recovery Point Objective (RPO)**: Target maximum data loss window (e.g., `< 1 hour` via continuous WAL / transaction log backups).
- **Recovery Time Objective (RTO)**: Target maximum outage duration until operational restore (e.g., `< 4 hours`).

### Responsibility Division
- **Cloud / Platform Provider**: Physical storage redundancy, automated daily snapshots, point-in-time snapshot archiving.
- **Application Engineering**: Data integrity verification, application-level reconciliation scripts, outbox recovery.

---

## 2. Backup Verification Standard

> *Rule: A backup is NOT considered valid until restoration has been executed and verified in an isolated test environment.*

Automated scheduled restore drills SHOULD run monthly in staging/isolated test environments:
1. Restore backup snapshot into an isolated sandbox database.
2. Run database integrity check (e.g., `CHECKDB` or table count assertions).
3. Execute smoke test suite connecting to restored database instance.
4. Record drill outcome and elapsed restoration time.

---

## 3. Restoration Procedure (Point-in-Time Recovery)

*Note: Replace `<db-instance>`, `<backup-id>`, and timestamps with incident specifics.*

1. **Stop Application Ingestion**:
   Scale application deployments to 0 to prevent dual-writes during restoration:
   ```bash
   kubectl scale deployment/<app-name> --replicas=0 -n <namespace>
   ```
2. **Execute Point-in-Time Restore**:
   - In cloud console or via CLI, restore database to target timestamp prior to corruption event.
3. **Verify Restored Invariants**:
   - Assert critical ledger balances and user record counts.
4. **Reconnect & Resume**:
   - Update connection string if restored to a new database cluster endpoint.
   - Scale application pods back to active count.
   - Verify health probes and resume traffic.
