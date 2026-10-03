# Repository Profile

> **Classification:** `[TEMPLATE]`  
> **Status:** Draft (Populate with verified target repository facts upon adoption)  
> **Adopted Baseline Version:** `1.1.0`  
> **Adoption Date:** `<YYYY-MM-DD>`  
> **Adoption Mode:** `[New project | Existing project merge]`  
> **Target Path:** Copy or rename to `docs/repository-profile.md`  

---

## 1. Runtime & Compiler Baseline

- **.NET SDK Version**: `<Discover via dotnet --info or global.json>`
- **Target Framework**: `<Discover from .csproj <TargetFramework>>`
- **C# Language Version**: `<e.g. 12, 13, latest, default>`
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

---

## 4. Persistence & Data Access Authority

- **Primary Datastore**: `<Discover from configuration and connection strings; e.g. relational SQL, document store, or none>`
- **Data Access Technologies**: `<Discover from package manifests; e.g. ORM, query library, raw client, or none>`
- **Schema Authority**: `<Code-First / Migrations | Database-First | Externally Managed | Hybrid>`
- **Migration Ownership**: `<Repository-owned | Platform-owned | External>`
- **Concurrency Strategy**: `<Optimistic tokens | Pessimistic locks | Atomic conditional writes | None>`
- **Transaction Ownership**: `<Unit of Work | Explicit transactions | Distributed / Outbox | None>`

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
- **Safe Dry-Run / Lint**: `<e.g. dotnet build /warnaserror>`
