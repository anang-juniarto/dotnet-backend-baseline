# Secrets Management & Credential Lifecycle Policy

> **Document Metadata**:  
> `Status: Draft` | `Owner: Security Operations / DevOps` | `Last verified: Not verified` | `Evidence: Secrets management policy`

This document defines rules for storing, injecting, and rotating secrets across local development, CI/CD, and production environments.

---

## 1. Zero-Secrets-in-Source Policy

Committing passwords, API keys, private certificates, encryption keys, or production connection strings to version control is **STRICTLY FORBIDDEN**.

- **Detection**: Automated pre-commit hooks and CI secret scanning (e.g., Gitleaks, GitHub Secret Scanning) MUST be active.
- **Remediation**: If a secret is committed, it MUST be treated as compromised:
  1. Immediately revoke and rotate the secret at the provider.
  2. Invalidate affected sessions or tokens.
  3. Purge the secret from Git history if repository policy mandates.

---

## 2. Secrets Storage & Injection by Environment

| Environment | Secret Store Mechanism | Injection Method |
|---|---|---|
| **Local Development** | .NET Secret Manager (`user-secrets`) | Loaded into `IConfiguration` automatically in `Development` |
| **CI / Automated Tests** | GitHub Actions Secrets / CI Secret Vault | Injected as environment variables into test runners |
| **Staging / Production**| Azure Key Vault / AWS Secrets Manager / HashiCorp Vault | Managed Identity / Workload Identity provider |

---

## 3. Local Development Secrets Configuration

To configure secrets locally without modifying tracked files:
```bash
# Initialize user secrets in the startup project directory
dotnet user-secrets init

# Set database password or third-party API key
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost;Database=appdb;User Id=postgres;Password=<strong-local-password>;"
dotnet user-secrets set "ExternalPayment:ApiKey" "<sandbox-api-key>"
```

---

## 4. Credential Rotation & Overlap

- Credentials and API keys SHOULD support dual-key rotation:
  1. Provision secondary key.
  2. Update consuming clients/services to secondary key.
  3. Verify zero requests with primary key in telemetry.
  4. Revoke primary key.
- Webhook verification secrets MUST support an overlapping transition window so in-flight requests are not rejected during rotation.
