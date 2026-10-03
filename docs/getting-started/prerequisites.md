# Prerequisites

> **Document Metadata**:  
> `Status: Draft` | `Owner: Unknown` | `Last verified: Not verified` | `Evidence: None yet`

This guide outlines the system requirements and development tools needed to build, test, and run the backend service.

---

## 1. Required SDKs & Runtimes

*Note: Verify the actual target version from `.csproj` `<TargetFramework>` or `global.json`.*

| Component | Minimum Version | Recommended / Target | Discovery Command |
|---|---|---|---|
| **.NET SDK** | .NET 8.0+ *(verify .csproj)* | .NET 10.0 / Latest LTS | `dotnet --version` |
| **Git** | 2.30+ | Latest | `git --version` |
| **Docker / Podman** | *(If containerized)* | Latest Desktop / Engine | `docker --version` |

---

## 2. Local Database & Infrastructure Dependencies

Inspect `docker-compose.yml`, `appsettings.Development.json`, or infrastructure manifests to confirm dependencies:

- **Relational Database**: PostgreSQL / SQL Server / MySQL *(TBD based on repository profile)*
- **Distributed Cache**: Redis / Valkey *(TBD based on repository profile)*
- **Message Broker**: RabbitMQ / Kafka / Azure Service Bus *(TBD based on repository profile)*

---

## 3. Recommended Development Tools

- **IDE / Editor**: Visual Studio 2022+, JetBrains Rider, or VS Code with C# Dev Kit.
- **API Testing**: HTTP REPL, curl, Bruno, or Postman.
- **Database GUI**: DBeaver, pgAdmin, Azure Data Studio, or DataGrip.

---

## 4. Verification Step

Confirm your local machine is ready:
```bash
dotnet --info
git status
```
If errors occur or SDKs are missing, install the required .NET SDK from [dotnet.microsoft.com](https://dotnet.microsoft.com/download).
