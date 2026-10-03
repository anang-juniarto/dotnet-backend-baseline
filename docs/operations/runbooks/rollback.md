# Runbook: Rollback & Forward-Fix Procedures

> **Document Metadata**:  
> `Status: Draft` | `Owner: DevOps / Release Management` | `Last verified: Not verified` | `Evidence: Rollback procedure standard`

This runbook defines the decision matrix and technical procedures for rolling back a deployment or choosing a forward-fix.

---

## 1. Decision Matrix: Rollback vs. Forward-Fix

| Condition | Recommended Action | Rationale |
|---|---|---|
| **P1 regression detected within 15 min of deploy; schema was purely additive** | **Immediate Rollback** | Reverting container image to previous tag is instantaneous and safe. |
| **New version wrote destructive schema change (data dropped)** | **Forward-Fix Only** | Rolling back application code will fail because schema cannot support old code. |
| **Simple configuration or environment variable error** | **Config Patch** | Fast rollout of corrected environment variable without image redeployment. |
| **Data migration partially failed halfway** | **Halt Traffic & Reconcile** | Reconcile corrupt records before deciding rollout direction. |

---

## 2. Application Container Rollback Procedure

When code rollback is safe:

1. **Revert Deployment in Orchestrator**:
   ```bash
   # Kubernetes example:
   kubectl rollout undo deployment/<deployment-name> -n <namespace>
   
   # Or deploy previous known-good immutable container image tag:
   # kubectl set image deployment/<deployment-name> app=<image-registry>/<app>:<previous-tag> -n <namespace>
   ```
2. **Monitor Pod Replacement**:
   ```bash
   kubectl rollout status deployment/<deployment-name> -n <namespace>
   ```
3. **Verify Health**:
   - Check `/health/ready` returns `200 OK`.
   - Validate error rate drops back to baseline.

---

## 3. Database Migration Rollback Limitations

- If migrations followed the **Expand-Migrate-Contract** policy (Phase 1 Expand), rolling back application code does NOT require rolling back database schema; the expanded schema is backward-compatible with the old application.
- Never execute destructive down-migrations in production without a verified point-in-time recovery backup.
