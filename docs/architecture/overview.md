# System Architecture Overview

> **Document Metadata**:  
> `Status: Draft` | `Owner: System Architect / Core Team` | `Last verified: Not verified` | `Evidence: Initial system blueprint`

This is a reference blueprint, not a verified deployed system. Approved greenfield generation defaults to the [.NET 10 Clean Architecture/CQRS profile](./net10-baseline-profile.md): REST controllers and MediatR CQRS, Core Domain/Application, modular Infrastructure provider/capability assemblies, and Presentation WebApi plus separately selected Grpc hosts. Each host owns composition-root wiring; only approved adapters are selected. The [canonical selectable schema](./repository-map.md) is a blueprint, not an implemented-module inventory. Brownfield reference-only adoption preserves discovered runtime, boundaries and contracts; capability additions and migrations need separate approval. The isolated optional `examples/Baseline.Sample` is a local in-memory demo, not external-integration certification.

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
- **Downstream Dependencies**: Only approved selected stores/adapters; the diagram is illustrative, not a mandatory topology. A stateless service is valid.
- **Optional Capabilities**: Select independently from the [catalog](./optional-stack-catalog.md). SignalR transport, durable chat, notification inbox and external push have distinct roles; no mandatory Redis/Firebase/RabbitMQ/Hangfire stack follows from them.

---

## 2. Structural States

Architectural changes MUST distinguish between three distinct states:
1. **Current / Verified State**: The architecture actually running in production and verified by tests.
2. **Approved Transition**: The active migration phase (Expand-Migrate-Contract) currently underway.
3. **Target State**: The long-term architectural goal documented in accepted ADRs.

---

## 3. Core Boundaries & Consistency

- **Consistency Boundaries**: Each aggregate or bounded context enforces internal invariants transactionally.
- **Cross-Service Communication**: Select synchronous or asynchronous transport for actual requirements. Business-required recoverable publication uses durable intent/outbox where supported and idempotent consumers; no cross-store atomicity or exactly-once promise.
- **Tenant Boundaries**: When tenancy is selected, resolve context from trusted identity and enforce it across queries, cache partitions, messages and audit trails.
- **Realtime and Notifications**: Persist durable chat/inbox state before acceptance, separate send/receipt/read states, and recover through authorized history/inbox queries. Push providers are delivery adapters, not the system of record.

---

## 4. Scalability Triggers & Strategy

| Trigger | Scaling Strategy | Architecture Requirement |
|---|---|---|
| **High Read Volume** | Read-through caching & read replicas | Cache invalidation strategy; read-your-own-writes consistency |
| **Spiky Ingestion** | Background worker queueing | Backpressure limits, idempotent message handling |
| **Heavy Processing** | Asynchronous decoupled workers | Graceful shutdown handling, bounded concurrency |
