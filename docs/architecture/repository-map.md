# Repository & Project Map

> **Classification:** `[PROFILE]`
> **Status:** Draft
> **Owner:** Architecture / Engineering
> **Last verified:** Not verified
> **Evidence:** User canonical `gemini-code-1791622287764.txt`; selectable target blueprint, not integration certification

## 1. Reference, sample and target boundaries

The root repository remains a portable, non-executable engineering reference plus the isolated optional `examples/Baseline.Sample`. Approved greenfield generation follows the [.NET 10 MediatR CQRS profile](./net10-baseline-profile.md) and the canonical Core/Infrastructure/Presentation schema below. This is not a claim that every project, class, package or deployment asset exists.

Brownfield reference-only adoption preserves discovered runtime, project boundaries, direct handlers/services, persistence and published contracts. Adding a capability does not authorize architecture migration. Missing profile documents fall back to relevant manifests/source and applicable standards.

The user-owned `gemini-code-1791622287764.txt` remains unchanged. Maintained guidance uses .NET 10 (`net10.0`) and RFC 9457 Problem Details rather than the raw schema's older runtime/RFC labels. Names and example business entities in the schema are illustrative, not required scaffolding.

## 2. Canonical selectable superset

Generate only approved exact files and selected modules. Separate provider/capability assemblies are the greenfield topology; do not collapse them into a single Infrastructure assembly by default. Omit unselected projects, references, packages, containers, settings, schemas, hosted processes and probes. Package/server versions, licensing and compatibility require verification before implementation; migrations come from approved provider tooling, never invented files.

```text
<Service>/
├── src/
│   ├── Core/
│   │   ├── <Service>.Domain/
│   │   │   ├── Common/                         # only justified entity/event/value-object primitives
│   │   │   ├── Entities/
│   │   │   ├── Enums/
│   │   │   ├── Events/
│   │   │   ├── Exceptions/
│   │   │   ├── Repositories/                   # domain aggregate contracts only when needed
│   │   │   └── ValueObjects/
│   │   └── <Service>.Application/
│   │       ├── Common/
│   │       │   ├── Behaviors/                  # MediatR validation/logging/tracing/performance
│   │       │   ├── Exceptions/
│   │       │   ├── Interfaces/                 # selected persistence/cache/search/bus/job ports
│   │       │   └── Models/                     # transport-neutral results and pagination
│   │       ├── Features/<Feature>/
│   │       │   ├── Commands/<UseCase>/         # request, MediatR handler, validator, result DTO
│   │       │   ├── Queries/<UseCase>/          # request, MediatR handler, projected read DTO
│   │       │   └── EventHandlers/              # selected domain-event orchestration
│   │       └── DependencyInjection.cs          # MediatR and approved behaviors/validation
│   ├── Infrastructure/
│   │   ├── <Service>.Persistence.EntityFramework/
│   │   │   ├── Common/                         # selected audit/outbox interceptors
│   │   │   ├── Configurations/                 # provider-neutral mappings of Domain entities
│   │   │   ├── Context/                        # registers mappings; no inline entity rules
│   │   │   ├── Repositories/                   # selected aggregate/port implementations
│   │   │   └── DependencyInjection.cs
│   │   ├── <Service>.Persistence.SqlServer/    # optional provider assembly
│   │   │   ├── Configurations/                 # SQL Server-specific overrides
│   │   │   ├── Migrations/                     # SQL Server-specific migration history
│   │   │   └── DependencyInjection.cs
│   │   ├── <Service>.Persistence.PostgreSql/   # optional provider assembly
│   │   │   ├── Configurations/                 # PostgreSQL-specific overrides
│   │   │   ├── Migrations/                     # PostgreSQL-specific migration history
│   │   │   └── DependencyInjection.cs
│   │   ├── <Service>.Persistence.MySql/        # optional; EF10 compatibility gate
│   │   │   ├── Migrations/                     # MySQL-specific migration history
│   │   │   └── DependencyInjection.cs
│   │   ├── <Service>.Persistence.MongoDb/      # optional authoritative documents/read model
│   │   │   ├── Configurations/                 # class maps and index ownership
│   │   │   ├── Context/
│   │   │   ├── Repositories/
│   │   │   └── DependencyInjection.cs
│   │   ├── <Service>.Infrastructure.Search/    # optional Elasticsearch projections
│   │   │   ├── Documents/
│   │   │   ├── Services/
│   │   │   └── DependencyInjection.cs
│   │   ├── <Service>.Infrastructure.Caching/   # optional Redis cache, not durable authority
│   │   │   ├── Services/
│   │   │   ├── Serialization/
│   │   │   └── DependencyInjection.cs
│   │   ├── <Service>.Infrastructure.Messaging/ # optional selected MassTransit + RabbitMQ
│   │   │   ├── Consumers/
│   │   │   ├── Messages/                       # versioned integration contracts
│   │   │   ├── Services/
│   │   │   ├── Outbox/                         # required durable publication recovery
│   │   │   └── DependencyInjection.cs
│   │   ├── <Service>.Infrastructure.BackgroundJobs/ # optional Hangfire
│   │   │   ├── Dashboard/                      # authorization policy
│   │   │   ├── Jobs/
│   │   │   ├── Services/
│   │   │   └── DependencyInjection.cs
│   │   ├── <Service>.Infrastructure.Observability/  # selected OTel/Seq/Sentry routes
│   │   │   ├── Logging/
│   │   │   ├── OpenTelemetry/
│   │   │   └── DependencyInjection.cs
│   │   └── <Service>.Infrastructure.Push/     # optional extension beyond canonical base
│   │       ├── Providers/<SelectedProvider>/
│   │       ├── Delivery/                       # durable intent/attempt dispatcher
│   │       └── DependencyInjection.cs
│   └── Presentation/
│       ├── <Service>.WebApi/
│       │   ├── Controllers/v1/                # HTTP DTO → MediatR request
│       │   ├── Middlewares/                   # RFC 9457 errors and correlation
│       │   ├── Extensions/                    # selected provider and API-doc registration
│       │   ├── Authorization/
│       │   ├── Hubs/                          # optional authenticated SignalR adapters
│       │   ├── Contracts/                     # safe HTTP/realtime transport DTOs
│       │   ├── HealthChecks/                  # critical selected dependencies only
│       │   ├── appsettings.json
│       │   ├── appsettings.Development.json
│       │   ├── Program.cs                     # composition root
│       │   └── DependencyInjection.cs
│       ├── <Service>.Grpc/                    # optional separate host, not a WebApi folder
│       │   ├── Protos/
│       │   ├── Services/                      # protobuf → MediatR request
│       │   ├── Interceptors/                  # RPC errors, auth/tracing, deadlines
│       │   ├── appsettings.json
│       │   ├── appsettings.Development.json
│       │   ├── Program.cs                     # independent composition root
│       │   └── DependencyInjection.cs
│       └── <Service>.Worker/                  # optional independently hosted jobs/consumers
├── tests/
│   ├── <Service>.UnitTests/
│   │   ├── Domain/
│   │   └── Application/                       # handlers, behaviors, validation
│   ├── <Service>.IntegrationTests/
│   │   ├── Common/                            # host factory and selected engine fixtures
│   │   ├── Persistence/
│   │   ├── Messaging/
│   │   ├── WebApi/
│   │   └── Grpc/                              # selected transport contracts
│   └── <Service>.ArchitectureTests/           # project/layer dependency enforcement
├── docs/                                      # selected target guidance and verified profile
├── .editorconfig
├── .gitignore
├── README.md
├── AGENTS.md
├── Directory.Build.props
├── Directory.Packages.props                   # only when CPM selected
├── global.json                                # verified SDK policy
├── <Service>.sln                              # or .slnx
├── .github/                                   # optional selected CI provider
├── deploy/                                    # optional approved Docker/IaC assets
└── docker-compose.yml                         # optional selected local services only
```

The raw schema's workflow, Kubernetes/Helm, Terraform/cloud and all-services Compose branches are optional deployment/tooling examples, not mandatory generated assets. No cloud credentials, production targets, service startup or package installation follows from this blueprint. Preserve destination identity/history/license and full MIT attribution under [BASELINE.md](../../BASELINE.md).

### Chat, inbox and push extensions

- Add selected Chat and Notifications feature slices under Application and only necessary domain entities/value objects. Durable chat and inbox schemas belong to explicitly selected persistence assemblies; never use SignalR or push as the system of record.
- WebApi owns SignalR hubs/adapters implementing Application realtime ports. Chat requires identity, authoritative history storage and SignalR; inbox can exist without SignalR/push. External push can exist without chat/inbox but requires recoverable delivery intent where the business contract demands it.
- Infrastructure.Push owns selected FCM/APNs/Web Push adapters and delivery attempts; Firebase authentication/storage, Redis, RabbitMQ and Hangfire are not implied. Client service workers/mobile SDKs need separate scope.
- Worker never references WebApi. Use an explicit managed publication route or durable signal to an API broadcaster; a Redis backplane is not an arbitrary-worker publishing API. Single-node SignalR has no Redis requirement.

### Isolated executable sample

The sample now uses the canonical `src/Core/Baseline.Sample.Domain`, `src/Core/Baseline.Sample.Application`, `src/Infrastructure/Baseline.Sample.Persistence.InMemory`, `src/Presentation/Baseline.Sample.WebApi` and `tests/Baseline.Sample.{UnitTests,IntegrationTests,ArchitectureTests}` topology. Its [README](../../examples/Baseline.Sample/README.md) and manifests, not this target blueprint, establish current implementation/evidence. Its owner-scoped Items use case uses volatile memory; it does not prove production persistence, chat/inbox/push durability or external-provider behavior. Optional provider/capability assemblies and separate gRPC/realtime transports compile in the aggregate solution; live engines/delivery are not certified by compilation. `examples/**` remains excluded from baseline transfer; adapt or omit this source-only sample link in destination copies.

## 3. Dependency direction and assembly contracts

```text
Presentation hosts ──▶ Application ──▶ Domain
       │                    ▲
       └──▶ selected Infrastructure assemblies
                            │
                            └──▶ Application / Domain
Provider assemblies ──▶ shared Persistence.EntityFramework
```

- Domain has no Application, Infrastructure, MediatR, EF, transport or vendor SDK dependency. No unused DDD primitives or repository wrappers are required.
- Application references Domain and explicitly selected MediatR/application-level packages. It owns use cases and transport-neutral outbound ports, not concrete database clients, protobuf messages, hubs or vendor models.
- Infrastructure adapters implement Application/domain ports. Provider assemblies reference shared EF persistence for provider-specific registration and migration isolation; contexts, tracking objects and SDK types stay behind these boundaries.
- Presentation references Application and only selected Infrastructure projects for composition-root registration. Controllers/services dispatch MediatR requests and propagate cancellation; they do not access DbContext or execute SQL.
- Messaging uses MassTransit/RabbitMQ only when approved. Required committed publication uses durable intent/outbox and idempotent consumers; no cross-store atomicity or exactly-once promise.
- Commands have explicit authorization/invariant/transaction ownership; distinguish confirmed commit, rollback and unknown outcome. Queries are bounded, read-only and projected; CQRS does not require separate databases.
- Architecture tests enforce inward project references and provider boundaries. Singletons must not capture scoped services or DbContext. Worker operations create scopes per delivery/job.

See the [optional catalog](./optional-stack-catalog.md) for selection dependencies and real-engine certification gates. A successful build or in-memory test is not external-stack certification.
