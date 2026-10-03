# Security Architecture & Threat Boundary Overview

> **Document Metadata**:  
> `Status: Draft` | `Owner: Security Engineering / AppSec` | `Last verified: Not verified` | `Evidence: Security architecture baseline`

This document defines the security architecture, threat boundaries, and baseline controls for the backend system.

---

## 1. Threat Boundaries & External Interfaces

```text
[ Untrusted Internet ] ──▶ ( TLS 1.3 / Reverse Proxy / WAF )
                                    │
                                    ▼
                         [ Edge / API Controller ]
                                    │
                                    ▼  (Authenticated / Scoped Context)
                         [ Application Domain Core ]
                                    │
                                    ▼
               [ Isolated Datastore / Encrypted Secrets ]
```

---

## 2. OWASP API Security Baseline

1. **Broken Object Level Authorization (BOLA / IDOR)**:
   - Possession of a primary key (e.g., `orderId`) NEVER implies permission to view or mutate it.
   - Every operation MUST verify ownership and tenant tenancy explicitly.
2. **Server-Side Request Forgery (SSRF) Prevention**:
   - Outbound requests to user-supplied URLs MUST be validated against normalized IP/DNS allow-lists.
   - Private ranges (`10.0.0.0/8`, `172.16.0.0/12`, `192.168.0.0/16`, `127.0.0.1`, cloud metadata `169.254.169.254`) are strictly blocked.
3. **Mass Assignment Prevention**:
   - Internal domain entities MUST NOT be bound directly from HTTP requests.
   - Always use dedicated, strictly typed Request DTOs.
4. **Injection Prevention**:
   - All database queries use parameterized SQL.
   - File uploads are validated by content stream (magic numbers), renamed, and stored in isolated object storage. Never use client-supplied filenames on disk.

---

## 3. Cryptography & Key Management

- Cryptographic primitives MUST come from the .NET base class library (`System.Security.Cryptography`) or approved platform mechanisms.
- Custom implementations of cryptographic algorithms or hashing are FORBIDDEN.
- Password hashing: Use Argon2id, PBKDF2 with SHA-256 (minimum iterations per current NIST/OWASP guidance), with unique per-credential salt. Fast hashes (MD5, SHA-1) are FORBIDDEN.
- Data in transit: TLS 1.2+ mandatory across all public and inter-service boundaries.
