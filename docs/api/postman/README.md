# Executable Request Collections & Client Guidance

> **Document Metadata**:  
> `Status: Draft` | `Owner: QA & API Governance` | `Last verified: Not verified` | `Evidence: Collection governance standard`

This document defines safety guidelines, environment variable isolation, and execution boundaries for interactive API collections (Postman, Bruno, curl, HTTP REPL).

---

## 1. Deferred Artifact Notice

In accordance with anti-hallucination guardrails:
- **Collections & Environments**: Deferred until verified endpoints and contracts exist in the codebase.
- **Do Not Scaffolding Fictitious Requests**: Concrete Postman collections, environment JSON files, and runner test data will be added only after real API endpoints are implemented and tested.

---

## 2. Universal Execution Safety Gates

1. **Production Isolation**: Production MUST NOT be the default target in any collection, script, or environment template.
2. **Safe Defaults**: Default collection runs MUST exclude destructive, financial, balance-mutating, or chargeable endpoints.
3. **Synthetic Data Only**: All saved examples, runner fixtures, and sample IDs MUST use synthetic, non-production data.

---

## 3. Secret Isolation & Environment Sanitation

Before committing any collection, environment, or exported script:
- All credential variables (`apiKey`, `bearerToken`, `clientSecret`, `password`) MUST be empty strings:
  ```json
  { "key": "bearerToken", "value": "", "enabled": true }
  ```
- Store active credentials outside version control using Postman Vault, environment overrides, or local `.env` files.
- Inspect exports thoroughly for session cookies, active JWTs, production URLs, or personal data. Never rely solely on UI masking checkboxes.

---

## 4. Postman Integration Workflow (When Adopted)

When collections are adopted under `collections/`:
1. **Naming & Grouping**: Group requests by functional module matching [`docs/architecture/repository-map.md`](../../architecture/repository-map.md).
2. **Base URL**: Use `{{baseUrl}}` variable across all requests.
3. **Assertions**: Test scripts should assert status codes and schema shapes matching [`docs/api/response-and-error-contracts.md`](../response-and-error-contracts.md).
