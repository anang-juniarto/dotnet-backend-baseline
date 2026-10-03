# Repository & Project Map

> **Document Metadata**:  
> `Status: Draft` | `Owner: Unknown` | `Last verified: Not verified` | `Evidence: Project template structure`

This document maps the project directory structure, assemblies/projects, and permissible dependency directions for the repository.

---

## 1. Discovered Architecture Profile

*Note: Verify which architectural pattern is selected by the solution:*
- [ ] Clean Architecture (Domain, Application, Infrastructure, Presentation)
- [ ] Vertical Slice Architecture (Features as self-contained slices)
- [ ] Modular Monolith (Distinct domain modules within single deployment)
- [ ] Standard Layered Architecture

---

## 2. Project Directory & Responsibility Map

Below is the standard reference mapping for .NET backend solutions:

```text
/
├── src/ (or root project directories)
│   ├── <Project>.Domain/             # Core entities, value objects, domain invariants, domain errors
│   ├── <Project>.Application/        # Use cases, commands, queries, DTOs, interfaces, validation
│   ├── <Project>.Infrastructure/     # Persistence (EF Core / Dapper), external API clients, caching
│   └── <Project>.API/ (or Services)  # Controllers, endpoints, middlewares, Program.cs bootstrap
│
└── tests/
    ├── <Project>.UnitTests/          # Pure unit tests (Domain & Application handlers)
    ├── <Project>.IntegrationTests/   # Persistence, database, and repository tests
    └── <Project>.ContractTests/      # HTTP and message contract compatibility tests
```

---

## 3. Dependency Direction Rules

Dependencies MUST flow strictly inward:

```text
Presentation / API ───────▶ Application ───────▶ Domain
         │                       ▲
         ▼                       │
   Infrastructure ───────────────┘
```

1. **Domain**:
   - MUST NOT depend on any other project or infrastructure package (no EF Core, no ASP.NET Core).
   - Contains pure business logic and entity invariants.
2. **Application**:
   - Depends ONLY on `Domain`.
   - Defines use cases, interfaces (ports), and business workflows.
   - MUST NOT reference `Infrastructure` or web presentation packages.
3. **Infrastructure**:
   - Depends on `Application` and `Domain`.
   - Implements interfaces defined in Application (repositories, email senders, message publishers).
4. **Presentation / API**:
   - Depends on `Application`.
   - Depends on `Infrastructure` strictly at the composition root (`Program.cs`) for Dependency Injection wiring.

---

## 4. Boundary Violation Guardrails

- Controllers MUST NOT access `DbContext` or write direct SQL.
- Application use cases MUST NOT return transport-specific HTTP types (`IActionResult`, `HttpResponse`).
- Singletons MUST NOT capture scoped dependencies or database contexts.
