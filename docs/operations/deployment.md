# Deployment Pipeline & Verification Standards

> **Document Metadata**:  
> `Status: Draft` | `Owner: DevOps / Release Management` | `Last verified: Not verified` | `Evidence: Deployment pipeline design`

This document defines deployment stages, schema-before-code sequencing, artifact immutability, and post-deployment verification procedures.

---

## 1. Deployment Sequencing (Schema Before Code)

To support zero-downtime rolling upgrades, deployment stages MUST execute in this exact order:

```text
1. Pre-Deployment Verification (Unit/integration tests, build image)
   │
   ▼
2. Apply Backward-Compatible Schema Migrations (Expand Phase)
   │
   ▼
3. Rolling Deployment of New Application Containers (Canary / Blue-Green)
   │
   ▼
4. Post-Deployment Smoke Tests & Health Probe Checks
   │
   ▼
5. Traffic Shift (Route 100% traffic to new release)
```

---

## 2. Immutable Release Artifacts

- Build once, deploy anywhere: Container images (Docker / OCI) or binaries MUST be built once in CI, tagged with immutable version/commit SHA, and deployed unchanged across Staging and Production.
- Environment differences MUST be injected strictly via environment variables or secret stores, never by rebuilding artifacts.

---

## 3. Post-Deployment Verification (Smoke Checks)

Immediately upon completing deployment:
1. Verify liveness and readiness probe returns `200 OK`:
   ```bash
   curl -f http://<internal-service-host>/health/ready
   ```
2. Execute automated synthetic smoke test against a non-destructive read endpoint.
3. Monitor error rates, latency (p95 / p99), and log anomaly detectors for 15 minutes.
