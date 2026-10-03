# Safety Prohibitions and Profile-Specific Anti-Patterns Standards

> **Classification:** `[CORE STANDARD]`  
> **Status:** Normative Standard  
> **Applicability:** Active during architecture reviews, pull request evaluations, and code reviews.  
> **Source Migration:** Formed from Section 17 of the Enterprise .NET Backend AI Engineering Guide.

---

## 17. Safety Prohibitions and Profile-Specific Anti-Patterns

Apply each row only within its stated technology or architecture context. Clean Architecture/handler constraints depend on [architecture-and-use-cases.md](./architecture-and-use-cases.md) activation; they are not non-waivable universal bans. Other rows preserve the applicable safety properties defined in their source sections, including any explicitly controlled exception. [core-governance.md Section 3.3](./core-governance.md#33-formal-waiver-policy) governs waivers; existing violations do not authorize broader refactors or repetition.

| Anti-Pattern | Resolution |
|---|---|
| God controllers/endpoints | Keep responsibilities cohesive; under the Clean Architecture profile, dispatch use cases and map transport results only. Do not introduce a mediator for simple CRUD by default. |
| Handler chaining in the selected handler profile | Extract reusable domain/application behavior; required decoupled effects need durable commit-safe delivery, not merely an in-process event. |
| Transport envelopes in new clean Application handlers | Return transport-neutral results; adapt in Presentation. Preserve legacy contracts until migration. |
| In-memory filtering/paging of unbounded data | Push predicates, projection, ordering, and limits to the datastore. |
| Blocking async | Use async end-to-end and propagate cancellation. |
| Unbounded queues/buffers/retries | Apply bounded capacity, backpressure, and finite retry budgets. |
| Service locator | Inject explicit dependencies at composition boundaries. |
| Hard-coded secrets | Use approved runtime secret injection. |
| Cache as source of truth | Keep authoritative state in a durable system and define cache failure behavior. |
| Distributed lock as financial truth | Enforce invariants in the authoritative datastore and use leases only for coordination. |
| Blind retries of non-idempotent writes | Add durable idempotency/reconciliation or do not retry. |
| Breaking schema/API/event change in one deployment | Use compatible expansion, consumer migration, and delayed contraction. |
| High-cardinality telemetry labels | Use bounded dimensions and logs/traces for per-entity detail. |
| Hidden cross-tenant bypass | Require explicit privileged path, audit, safeguards, and tests. |
| Telemetry used as authoritative audit evidence | Store governed audit records independently with integrity and retention controls. |
| Volatile cache used as financial idempotency truth | Persist idempotency state and committed results in an authoritative durable store. |
| Timestamp treated as distributed ordering | Declare ordering scope/partition and use broker or datastore ordering guarantees. |
| Ambiguous shared schema ownership | Assign explicit object, migration, compatibility, and deployment ownership. |
| Cross-boundary cache mutation | Route invalidation/update through the cache owner's approved contract. |
