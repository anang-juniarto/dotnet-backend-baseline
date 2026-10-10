# API Governance & Contract Authority Guide

> **Document Metadata**:  
> `Status: Draft` | `Owner: API Governance Team` | `Last verified: Not verified` | `Evidence: API contract guidelines`

This document defines the contract authority, specification maintenance, and synchronization workflows for all exposed APIs. The selected greenfield default is .NET 10 REST controllers with MediatR CQRS feature slices, using the canonical Core/Infrastructure/Presentation modular structure; preserve verified brownfield contracts/runtime. gRPC and SignalR are optional transports, not implied dependencies. The isolated sample demonstrates only volatile in-memory Items, not external integrations.

---

## 1. Authoritative Contract Source

To avoid contract drift, every repository MUST declare its authoritative API contract mechanism:

- [ ] **Code-First / Generated OpenAPI**: ASP.NET Core source controllers and DTOs generate OpenAPI/Swagger schemas at build/runtime. The generated OpenAPI document is the authoritative representation.
- [ ] **Contract-First / Spec-Driven**: An independently maintained OpenAPI (`openapi.yaml`) or Protobuf spec is the authoritative source; code is generated or validated against it.

*Rule: If OpenAPI is generated from code, document the exact generation procedure. NEVER maintain a competing manual duplicate copy of generated schemas.*

For selected .NET 10 generation, verify the installed OpenAPI package/toolchain and consumer support. Built-in .NET 10 generation defaults to OpenAPI 3.1; do not assume .NET 11's 3.2 default or copy unversioned APIs. Explicitly record the emitted version and schema dialect, and test downstream generators/validators. Generation is optional and not evidence that the sample exposes OpenAPI. Native gRPC uses Protobuf authority; hub events need separately maintained contracts.

---

## 2. Artifact Conflict Resolution Workflow

When documentation, OpenAPI specifications, tests, or runtime implementations conflict:

1. **Identify the Discrepancy**: Document the conflicting versions with exact source citations.
2. **Consult Declared Authority**: Check whether OpenAPI spec or source code is the designated authority.
3. **Preserve Runtime Compatibility**: Do NOT alter runtime code merely to match unverified prose, and do NOT alter published contracts merely to match accidental implementation defects.
4. **Characterize with Tests**: Write characterization tests verifying what existing clients actually receive.
5. **Escalate Unresolved Ambiguities**: If client impact is material, escalate to the API/Product Owner before modifying behavior.

---

## 3. API Documentation Index

- [`conventions.md`](./conventions.md): Global rules for routes, dates, pagination, headers, and sorting.
- [`authentication.md`](./authentication.md): Token schemes, claims resolution, tenant scoping, and CSRF protection.
- [`response-and-error-contracts.md`](./response-and-error-contracts.md): Envelope formats, RFC 9457 Problem Details, error codes.
- [`compatibility-and-versioning.md`](./compatibility-and-versioning.md): Backward compatibility, deprecation, and sunset policies.
- [`grpc.md`](./grpc.md): Optional Protobuf contract authority, deadlines, authorization and retries.
- [`signalr.md`](./signalr.md): Optional hub/event contracts, member authorization and reconnect/history recovery.
- [`../engineering/chat-and-notifications.md`](../engineering/chat-and-notifications.md): Independent chat/inbox selections and durable delivery invariants.
- [`../operations/push-notifications.md`](../operations/push-notifications.md): Proposed provider selection, security, retries and separately scoped clients.
- [`templates/endpoint-template.md`](./templates/endpoint-template.md): Standard template for documenting individual endpoint operations.
- [`postman/README.md`](./postman/README.md): Guidelines for executable API request collections.
