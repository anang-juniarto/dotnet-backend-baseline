# API Authentication & Authorization Infrastructure

> **Document Metadata**:  
> `Status: Draft` | `Owner: Security & Identity Team` | `Last verified: Not verified` | `Evidence: Security and auth standards`

This document defines how API requests are authenticated, how identity and tenant context are resolved, and how authorization boundaries are enforced.

---

## 1. Supported Authentication Schemes

| Scheme | Target Consumer | Validation Mechanism | Credential Transport |
|---|---|---|---|
| **JWT (Bearer Token)** | Mobile, Web SPA, API Consumers | Cryptographic signature, issuer, audience, and expiry validation | `Authorization: Bearer <jwt>` |
| **Opaque Reference Token** | Partner APIs, High-Security Sessions | Server-side introspection cache or database lookup | `Authorization: Bearer <token>` |
| **API Keys** | System-to-System, Webhooks | Secure hash lookup against authorized tenant registry | `X-Api-Key: <key>` |
| **Session Cookies** | Browser Clients (SSR / Admin) | Secure, HttpOnly, SameSite cookie with explicit CSRF token | Cookie header |
| **Workload Identity (mTLS)**| Internal Microservices | Mutual TLS certificate or cloud workload identity federation | Transport layer |

---

## 2. Token Validation & Claims Resolution

For JWT Bearer Tokens, the API gateway or authentication middleware MUST validate:
1. **Signature & Signing Key**: Validated against JWKS endpoint; untrusted algorithms (`none`) are FORBIDDEN.
2. **Issuer (`iss`) & Audience (`aud`)**: Must match repository configuration exactly.
3. **Lifetime (`exp`, `nbf`)**: Rejects expired tokens with configurable clock skew (max 5 minutes).
4. **Tenant Resolution**: Tenant identifier (`tenant_id` claim) MUST be resolved and injected into the request execution context.

---

## 3. Browser Security & Anti-CSRF

When cookie authentication is used for browser clients:
- Cookies MUST be marked `HttpOnly`, `Secure`, and `SameSite=Strict` (or `Lax`).
- State-changing HTTP methods (`POST`, `PUT`, `PATCH`, `DELETE`) MUST validate an anti-forgery token (`X-CSRF-TOKEN`).
- *CORS headers alone are NOT sufficient protection against CSRF.*

---

## 4. Default-Deny Authorization

- All endpoints are closed by default (`[Authorize]` policy applied globally).
- Public endpoints MUST be explicitly marked with `[AllowAnonymous]`.
- Enforce resource ownership checks (e.g., verifying user owns the requested entity ID) in the application/domain layer, not solely by checking role membership.
