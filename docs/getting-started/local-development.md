# Local Development Setup

> **Document Metadata**:  
> `Status: Draft` | `Owner: Unknown` | `Last verified: Not verified` | `Evidence: None yet`

This document explains how to set up, configure, and execute the backend service on your local workstation.

---

## 1. Initial Setup

1. **Clone & Verify**:
   ```bash
   git clone <repository-url>
   cd <repository-directory>
   ```
2. **Restore Dependencies**:
   ```bash
   dotnet restore <solution-name>.sln
   ```
3. **Build Solution**:
   ```bash
   dotnet build <solution-name>.sln --no-restore
   ```

---

## 2. Local Configuration & Secrets

Configuration files follow ASP.NET Core precedence:
- `appsettings.json`: Base configuration (committed to repository, NO secrets).
- `appsettings.Development.json`: Local development defaults (NO secrets).
- **User Secrets / Environment Variables**: For passwords, connection strings, and API keys.

Initialize user secrets for the startup API project:
```bash
cd <path-to-startup-project>
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "<local-db-connection-string>"
```

*NEVER commit active credentials, production URLs, or personal access tokens.*

---

## 3. Starting Supporting Services

If the repository includes containerized local infrastructure:
```bash
docker compose -f docker-compose.yml up -d
```
Verify supporting containers (database, cache, broker) are healthy before starting the API.

---

## 4. Running the Application

Start the web API service:
```bash
dotnet run --project <path-to-startup-project>/<project-name>.csproj
```
Watch for startup logs to identify the active listening port:
- Example: `Now listening on: http://localhost:5000` or `https://localhost:5001`.

---

## 5. Running Automated Tests

Execute the test suites:
```bash
dotnet test <solution-name>.sln
```
Ensure all tests exit with status code 0 before making changes.
