# Configuration, Delivery, Supply Chain, and Business Continuity Standards

> **Classification:** `[CORE STANDARD]`  
> **Status:** Normative Standard  
> **Applicability:** Active when defining configuration, feature flags, dependency supply chain, deployment pipelines, capacity planning, or backup/recovery procedures.  
> **Source Migration:** Formed from Section 13 of the Enterprise .NET Backend AI Engineering Guide.

---

## 13. Configuration, Delivery, Supply Chain, and Business Continuity

### 13.1 Configuration

- Use strongly typed options at application boundaries when supported.
- Validate required configuration at startup.
- Keep business code independent of generic configuration providers.
- Never store production secrets in source-controlled settings.

### 13.2 Feature Delivery

Feature flags apply when deployment must be decoupled from release, a kill switch is required, or incremental rollout reduces risk. Define owner, default state, targeting, auditability, failure behavior, and removal date. Monitor stale flags and remove them promptly after rollout, rollback windows, or experiments end. Authorization MUST NOT rely solely on a feature flag.

### 13.3 Software Supply Chain

- Secret scanning MUST apply when required by repository, release, or organizational policy. If required scanning is unavailable, follow the verified release-block, waiver, or alternative-review process; MUST NOT install tooling merely to manufacture evidence.
- Dependency vulnerability scanning SHOULD apply generally and MUST apply when policy or contracts require it. Record unavailable required checks distinctly.
- Generate an SBOM only when policy, contract, regulation, or explicit task scope requires it.
- Preserve dependency reproducibility through verified repository mechanisms such as lock files, checksums, immutable references, or controlled resolution; do not impose an undefined universal pinning scheme.
- Use immutable artifacts and provenance/signing when the threat model or policy requires it.
- Define an approved dependency-license policy, prohibited/restricted licenses, transitive-license review, and third-party notice obligations.
- Assign dependency ownership and an update/review cadence appropriate to exposure and criticality.
- Define patch SLAs by severity.
- Never disable security gates without an eligible approved waiver under [core-governance.md Section 3.3](./core-governance.md#33-formal-waiver-policy), explicit [Section 2.5 approval](./core-governance.md#25-approval-gates), and effective compensating controls; non-waivable protections remain active.

### 13.4 Deployment Safety

- Deploy backward-compatible schema before code that requires it.
- Use immutable artifacts across environments.
- Define automated rollback criteria for reversible changes and forward-fix procedures for irreversible migrations.
- Use canary, blue-green, or progressive delivery when blast radius and SLOs justify it.
- Verify health, telemetry, and critical business transactions after deployment.

### 13.5 Capacity and Availability Planning

For services with material availability or throughput obligations:
- maintain measured capacity headroom and dependency quota awareness;
- choose autoscaling signals from user-impacting demand and saturation metrics, with stabilization/cooldown controls;
- define overload priorities so critical operations are preserved before optional traffic;
- review single points of failure and dependency capacity limits;
- document cost, latency, redundancy, and recovery trade-offs rather than maximizing scale without evidence.

### 13.6 Backup, Recovery, and Continuity

Systems with durability or availability obligations MUST define:
- recovery point objective (RPO) and recovery time objective (RTO);
- backup scope, encryption, retention, and access;
- automated restore verification;
- dependency and regional failure strategy where applicable;
- scheduled recovery exercises with recorded outcomes.

A backup is not considered valid until restoration is tested.
