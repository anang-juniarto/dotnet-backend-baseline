# Repository Profile

> **Classification:** `[TEMPLATE]`  
> **Status:** Draft (Populate with verified target repository facts upon adoption)  
> **Adopted Baseline Version:** `<Read the approved source .baseline-version at adoption time>`
> **Adoption Date:** `<YYYY-MM-DD>`  
> **Adoption Mode:** `<New | Existing-reference | Existing-capabilities | Existing-migration>`
> **Reference Scope:** `<core-reference | explicit full-reference>`
> **Owner / Last verified / Evidence:** `<Owner> / <Not verified or date> / <Manifest and executed checks>`
> **Target Path:** Copy or rename to `docs/repository-profile.md`  

---

This template is not evidence of installed capabilities. Approved greenfield defaults are .NET 10, Clean Architecture/MediatR CQRS, REST controllers and modular Core/Infrastructure/Presentation assemblies per the [canonical schema](./architecture/repository-map.md); brownfield reference-only adoption preserves discovered facts. Runtime capability choices do not change reference-transfer scope. Source `examples/**` is excluded, and the local in-memory sample does not certify external stacks.

## 1. Runtime & Compiler Baseline

- **.NET SDK Version**: `<Discover via dotnet --info or global.json>`
- **Target Framework**: `<Discover from .csproj <TargetFramework>>`
- **SDK Pin / Roll-forward**: `<Verified stable SDK in global.json and explicit policy>`
- **C# Language Version**: `<SDK default for greenfield; discovered explicit version for existing>`
- **Nullable Context**: `<enable | disable | warnings>`
- **Implicit Usings**: `<enable | disable>`
- **Central Package Management (CPM)**: `<Enabled (Directory.Packages.props) | Disabled>`

---

## 2. Solution & Project Structure

- **Solution File**: `<Discover at repository root (.sln or .slnx)>`
- **Core / Domain Projects**: `<List project paths or state Not Applicable>`
- **Application / Use Case Projects**: `<List project paths or state Not Applicable>`
- **Infrastructure / Persistence Projects**: `<List project paths or state Not Applicable>`
- **Presentation / API Projects**: `<List project paths or state Not Applicable>`
- **Test Projects**: `<List test project paths>`
- **CI / Build Definition**: `<Discover from repository workflows or pipelines>`

---

## 3. Architecture & Organization Profile

- **Selected Architectural Pattern**: `<Discover from project structure; e.g. Clean Architecture, Vertical Slice, Modular Monolith, Minimal CRUD>`
- **Layer Boundary Enforcement**: `<Compiler dependencies, architectural tests, or conventions only>`
- **Feature Organization**: `<Vertical slices, single-file slices, or horizontal layers>`
- **Inward Dependency Flow**: `<Verified compliant | Not applicable | Deviations documented>`
- **Accepted ADRs**: `<List accepted ADR numbers from docs/adr/ or state None yet>`
- **CQRS Dispatch**: `<MediatR for approved greenfield | verified existing direct handlers/mediator | not applicable>`
- **Assembly Topology**: `<Core/Domain and Application; Infrastructure/provider and capability assemblies; Presentation/WebApi and selected separate Grpc | verified brownfield topology>`
- **Composition Root**: `<WebApi; separately selected Grpc/Worker hosts; verified existing bootstrap>`
- **Migration Boundary**: `<None for reference-only | separately approved scope/cutover/rollback>`

---

## 4. Persistence & Data Access Authority

- **Primary Datastore**: `<Discover from configuration and connection strings; e.g. relational SQL, document store, or none>`
- **Data Access Technologies**: `<Discover from package manifests; e.g. ORM, query library, raw client, or none>`
- **Schema Authority**: `<Code-First / Migrations | Database-First | Externally Managed | Hybrid>`
- **Migration Ownership**: `<Repository-owned | Platform-owned | External>`
- **Concurrency Strategy**: `<Optimistic tokens | Pessimistic locks | Atomic conditional writes | None>`
- **Transaction Ownership**: `<Explicit authoritative transaction owner | None>`
- **Publication / Split-Outcome Recovery**: `<Atomic outbox where supported | documented best effort | reconciliation | not applicable; no assumed distributed transaction>`
- **Provider / Server / Migration Sets**: `<Exact selected versions and ownership; MySQL EF10 support verified or deferred>`

---

## 5. API & Integration Contracts

- **API Style**: `<REST / HTTP JSON | Minimal APIs | Controller-based | gRPC | GraphQL | None>`
- **Contract Specification Authority**: `<OpenAPI / Swagger spec | Code-first generated | Hand-maintained markdown>`
- **Public Response Envelope**: `<Plain DTOs with RFC 9457 Problem Details | Custom envelope | Compatibility envelope>`
- **API Versioning Strategy**: `<URI segment (/v1/) | Header | Query parameter | Media type>`
- **Integration Events & Broker**: `<Discover from package manifests; e.g. message broker, cloud bus, in-memory, or none>`
- **Outbox Pattern**: `<Transactional Outbox table | Direct commit-publish | None>`

---

## 6. Security, Tenancy & Secrets

- **Authentication Scheme**: `<Discover from authentication configuration; e.g. JWT Bearer, Cookie/OIDC, API Key, Mutual TLS, None>`
- **Authorization Governance**: `<Policy-based requirements | Role-based | Claims-based | Fine-grained ACLs>`
- **Multi-Tenancy Model**: `<Single-tenant | Shared DB with TenantId column | Separate Schema | Separate Database | None>`
- **Secret Management**: `<User Secrets (local) + Key Vault / Cloud Secret Manager / None>`

---

## 7. Operations & Observability

- **Configuration Source**: `<appsettings.json + Environment Variables + Secret Store>`
- **Structured Logging**: `<Discover from package manifests and Program.cs; e.g. structured provider or standard logging>`
- **Distributed Tracing & Metrics**: `<Discover from package manifests; e.g. OpenTelemetry or vendor SDK>`
- **Health Check Endpoints**: `<e.g. /health/live, /health/ready | None>`
- **Containerization**: `<Dockerfile | .NET SDK PublishContainer | docker-compose | None>`

---

## 8. Verified Repository Commands

*(Only record commands that have been verified against the active environment)*

- **Restore Command**: `<e.g. dotnet restore>`
- **Build Command**: `<e.g. dotnet build --configuration Release --no-restore>`
- **Test Command**: `<e.g. dotnet test --configuration Release --no-build>`
- **Code Formatting / Analyzers**: `<e.g. dotnet format --verify-no-changes>`
- **Local Run Command**: `<e.g. dotnet run --project src/Api/Api.csproj>`
- **Safe Dry-Run / Lint**: `<Record actual verified command; no legacy warning-policy change implied>`

---

## 9. Optional Capability Selection and Evidence

Select at generation time; `none` means no package, schema, container, required setting, health probe, hosted process or network dependency. Record omitted and deferred choices explicitly; do not populate every adapter because this table exists.

| Capability | Selected choice / role | Exact package/server/edition versions and license | Evidence state / date / checks / limitations |
|---|---|---|---|
| Primary store | `<none / sqlserver / mysql / postgresql / mongodb>` | `<Verified or unresolved>` | `<proposed / verified / unavailable / deferred>` |
| Additional document store / search | `<none / explicit MongoDB role / Elasticsearch projection>` | `<Versions>` | `<Recovery/staleness tests>` |
| Cache | `<none / redis>` | `<Versions>` | `<Expiry/invalidation/outage>` |
| Messaging | `<none / MassTransit with RabbitMQ for selected greenfield messaging / verified existing broker stack>` | `<Versions>` | `<Confirms/outbox/duplicate/restart>` |
| RPC | `<none / grpc>` | `<Versions>` | `<HTTP2/TLS/deadlines/contracts>` |
| Jobs / storage / hosting | `<none / hangfire; separately selected storage; separate or approved in-process worker>` | `<Versions/license>` | `<Durability/retry/dashboard>` |
| Telemetry | `<none / opentelemetry>` | `<Versions/export path>` | `<Trace/metric ownership and outage>` |
| Logs / errors | `<console-json; optional seq / optional sentry>` | `<Versions/routes>` | `<Single capture/export and redaction>` |
| Realtime / scale-out | `<none / signalr; single-node / redis-backplane / managed service>` | `<Versions>` | `<Affinity/proxy/reconnect/revocation>` |
| Chat / inbox | `<independently selected with identity/store ownership>` | `<Schema/ports>` | `<Membership/dedup/order/unread tests>` |
| Web / mobile push | `<none / one explicit route per platform>` | `<Provider/SDK/platform matrix/license>` | `<Registration/rotation/privacy/retry; live tests separately authorized>` |

- **Realtime publishing ownership**: `<API-owned hub adapter; explicit worker-to-broadcaster or managed route, no Worker→API reference>`
- **Push delivery ownership**: `<Durable intent/attempt store and dispatcher; RabbitMQ/Hangfire optional>`
- **Push credentials / destination lifecycle**: `<Secret sources, redaction, owner verification, logout/rotation/cleanup; no secret values>`
- **Client responsibilities**: `<Separately scoped service worker, permission UX, mobile SDKs and devices; not backend transfer>`
- **Certification matrix**: `<Exact executed restore/build/real-engine and critical-pair tests; failed and unexecuted checks with reasons>`

A successful in-memory demo or build is not external-provider certification. No default Firebase authentication/storage, Redis, broker or job engine is inferred from notification selection.
