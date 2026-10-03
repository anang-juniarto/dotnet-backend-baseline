# System Architecture Overview

> **Document Metadata**:  
> `Status: Draft` | `Owner: System Architect / Core Team` | `Last verified: Not verified` | `Evidence: Initial system blueprint`

This document defines the high-level architecture, system context, and structural boundaries of the backend system.

---

## 1. System Context & Responsibilities

The backend service serves as the core business logic engine, data authority, and API provider for client applications and external partners.

```text
[ Web / Mobile Clients ] ──▶ [ API Gateway / Reverse Proxy ]
                                        │
                                        ▼
                            [ Backend Service (.NET) ]
                           /            │            \
                          ▼             ▼             ▼
                  [ Relational DB ]  [ Cache ]   [ Message Broker ]
```

- **Upstream Consumers**: Single Page Applications (Angular/React), Mobile Clients, Third-Party Webhooks.
- **Downstream Dependencies**: Primary Database, Distributed Cache, Message Broker, External APIs.

---

## 2. Structural States

Architectural changes MUST distinguish between three distinct states:
1. **Current / Verified State**: The architecture actually running in production and verified by tests.
2. **Approved Transition**: The active migration phase (Expand-Migrate-Contract) currently underway.
3. **Target State**: The long-term architectural goal documented in accepted ADRs.

---

## 3. Core Boundaries & Consistency

- **Consistency Boundaries**: Each aggregate or bounded context enforces internal invariants transactionally.
- **Cross-Service Communication**: Asynchronous messaging with transactional outbox for durable eventual consistency.
- **Tenant Boundaries**: Tenant context is resolved from trusted claims and enforced across database queries, cache partitions, and audit trails.

---

## 4. Scalability Triggers & Strategy

| Trigger | Scaling Strategy | Architecture Requirement |
|---|---|---|
| **High Read Volume** | Read-through caching & read replicas | Cache invalidation strategy; read-your-own-writes consistency |
| **Spiky Ingestion** | Background worker queueing | Backpressure limits, idempotent message handling |
| **Heavy Processing** | Asynchronous decoupled workers | Graceful shutdown handling, bounded concurrency |
