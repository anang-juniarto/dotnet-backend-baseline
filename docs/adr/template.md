# [ADR-XXXX] Title of Architectural Decision

> **ADR Metadata**:  
> `Status: Proposed | Accepted | Rejected | Superseded`  
> `Date: YYYY-MM-DD`  
> `Author(s): <Name / Team>`  
> `Approver(s): <Designated Architecture Owners>`  
> `Supersedes: [ADR-YYYY] (if applicable)` | `Superseded by: [ADR-ZZZZ] (if applicable)`

---

## 1. Context & Problem Statement

Describe the context, technical driver, scale, regulatory obligation, or limitation motivating this decision. Cite relevant measurements, incident history, or requirements.

---

## 2. Decision

State the specific architectural choice made. Describe the target topology, components, and responsibilities.

> *Important: An accepted decision establishes target architectural intent. It does NOT automatically authorize breaking changes or immediate retirement of active published contracts.*

---

## 3. Alternatives Considered

| Alternative | Pros | Cons / Why Rejected |
|---|---|---|
| **Option A (Chosen)** | ... | ... |
| **Option B** | ... | ... |
| **Option C** | ... | ... |

---

## 4. Consequences & Trade-Offs

- **Positive Consequences**: Benefits, performance improvements, reduced complexity, enhanced safety.
- **Negative Consequences / Trade-Offs**: Introduced operational dependencies, learning curve, migration cost.

---

## 5. Compatibility, Security & Migration Impact

- **Public & Event Contract Compatibility**: Does this decision introduce breaking changes? If yes, document the backward-compatible expansion and deprecation timeline.
- **Data & Persistence Migration**: Does this require database schema updates? Define the Expand-Migrate-Contract plan.
- **Security & Tenancy**: How does this impact tenant isolation, authentication, data classification, or compliance?

---

## 6. Rollout, Rollback & Observability Plan

- **Rollout Strategy**: Canary, feature flag, phased module deployment.
- **Rollback Criteria**: Measurable conditions triggering rollback vs. forward-fix.
- **Telemetry & Verification**: Required logs, OpenTelemetry metrics, health probe changes, and automated verification tests.
