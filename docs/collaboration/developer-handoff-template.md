# Developer / AI Handoff Record: [Task or Feature Name]

> **Handoff Metadata**:  
> `Status: Review-Only | Implemented | Partially Implemented | Blocked`  
> `Author / Agent: <Name / Agent ID>`  
> `Reviewer(s): <Human Reviewer / Peer>`  
> `Date: YYYY-MM-DD`  
> `Associated Issue / PR: <Ticket ID or PR #>`

---

## 1. Summary of Changes
Concise technical summary of what was implemented or modified. Include exact modified files and key methods.

- **File 1**: `path/to/File1.cs` - Added validation rule for...
- **File 2**: `path/to/File2.cs` - Implemented handler logic for...

---

## 2. Public Contract & Behavioral Impact
- **API Endpoints Changed**: List routes, status codes, or payload modifications.
- **Breaking Changes**: State `None` or detail approved deprecation/migration steps.
- **Integration Events**: Emitted event schemas and outbox entries.

---

## 3. Configuration & Database Impact
- **Configuration Keys**: New keys added to `appsettings.json` (NO secret values).
- **Database Migrations**: New migrations created, affected tables/columns.

---

## 4. Truthful Verification Report

### 4.1 Executed & Passed Checks
- [ ] `dotnet build`: Exited 0 with 0 errors.
- [ ] `dotnet test --filter ...`: Executed `X` tests, `X` passed, `0` failed.
- [ ] Static inspection: Confirmed no hardcoded secrets or unparameterized queries.

### 4.2 Unexecuted Checks (With Rationale)
- [ ] *Example: End-to-end integration test against live database omitted due to lack of local container runtime.*

---

## 5. Known Limitations, Edge Cases & Residual Risks
- Identified edge cases requiring production monitoring.
- Unresolved dependencies or downstream service assumptions.

---

## 6. Rollout & Rollback Guidance
- **Deployment Prerequisites**: Schema migration must be applied before container rollout.
- **Rollback Instructions**: If p99 latency spikes above threshold, revert image tag to `<previous-tag>`.
