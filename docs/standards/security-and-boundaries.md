# Security, Authorization, Tenancy, Cryptography, and Boundary Standards

> **Classification:** `[CORE STANDARD]`  
> **Status:** Normative Standard  
> **Applicability:** Active for authentication, authorization, tenancy isolation, cryptography, webhooks, edge security, file handling, and subprocess execution.  
> **Source Migration:** Formed from Section 8 of the Enterprise .NET Backend AI Engineering Guide.

---

## 8. Security, Authorization, Tenancy, Cryptography, and Privacy

### 8.1 Security Baseline

- Apply OWASP API Security controls using a threat model appropriate to exposure and data sensitivity.
- Validate credentials according to their verified scheme. For signed bearer tokens, validate signature, issuer, audience, lifetime, algorithm/key selection, and rotation/revocation expectations; opaque tokens, sessions, API keys, and mutual authentication use their authoritative validation and binding rules.
- Authorize every sensitive object operation; never rely on possession of a primary key.
- Prevent mass assignment with explicit request contracts.
- Prevent SSRF by allow-listing normalized schemes, hosts, ports, and effective destinations; reject userinfo and ambiguous/unsupported encodings; normalize IPv4/IPv6 forms; block loopback, link-local, private, multicast, metadata, and other special ranges unless explicitly required; revalidate DNS results and the connection destination to resist rebinding; and apply a restrictive redirect policy with bounded outbound clients.
- Keep secrets out of source, logs, traces, errors, and generated documentation; use an approved secret store.

### 8.2 Authorization Governance

- Use default-deny authorization for sensitive capabilities.
- Prefer explicit policy/permission checks over scattered role-name comparisons.
- Define ownership, approval, review, and retirement for roles and permissions.
- Authenticate and authorize service-to-service calls using an approved workload identity mechanism.
- Test privilege escalation, horizontal/vertical authorization, and cross-tenant denial.
- Administrative impersonation and break-glass access require explicit scope, strong authentication, expiration, tamper-resistant audit, alerting, and post-use review.

### 8.3 Operational Access Governance

For access to production databases, brokers, caches, secret stores, observability platforms, orchestration consoles, and administrative tools:
- enforce least privilege and separation of duties;
- prefer just-in-time, time-bounded access over standing privilege;
- require strong authentication, approval, and reason capture for high-risk access;
- record and protect administrative sessions or commands when risk or regulation requires it;
- review access periodically and revoke it promptly when roles change;
- prohibit shared operator credentials and direct unmanaged production access.

### 8.4 Tenant Isolation

For multi-tenant systems:
- resolve tenant context from an authenticated, trusted source;
- use tenant-qualified repository/query APIs and authorization checks;
- partition cache keys, messages, jobs, and audit records by tenant;
- isolate tenant-visible telemetry through authorization and, where required, separate storage or access boundaries; tenant isolation does not require a `tenantId` metric label. Use bounded aggregate dimensions and MUST NOT introduce unbounded tenant identifiers as metric labels;
- protect raw SQL and administrative bypass paths;
- test cross-tenant read, write, cache, and message isolation;
- use ORM global filters only as defense-in-depth; database row-level security MAY be added when the threat model justifies it.

### 8.5 Cryptography

- Use platform or organization-approved cryptographic libraries; never implement cryptographic primitives manually.
- Field encryption applies when classification, regulation, or threat modeling requires protection beyond storage encryption. Use authenticated encryption and managed key rotation/envelope encryption.
- Password hashing parameters MUST follow current OWASP/NIST guidance, be calibrated to the deployment environment, use unique salts and versioned parameters, and support rehash-on-authentication migration.
- MD5, SHA-1, and unsalted fast hashes are forbidden for credentials.
- Selecting Argon2id, PBKDF2, or another approved algorithm depends on platform support and requires verified package availability.

### 8.6 Transport, Browser, and Edge Security

- Encrypt data in transit using TLS according to current organizational security policy; protocol and cipher requirements MUST follow current approved baselines rather than hard-coded historical values.
- Define certificate issuance, rotation, expiration monitoring, revocation, and trust ownership.
- Authenticate high-risk internal communication using mutual TLS or an approved service/workload identity mechanism when the threat model requires it. Workload identity does not replace TLS transport encryption or peer validation; assess authentication and transport protection independently.
- Encryption is REQUIRED across process, host, cluster, or trust-zone boundaries. Plaintext MAY be used only on a verified local or equivalently protected channel after a documented threat assessment establishes isolation, peer authenticity where needed, and compensating controls; an “internal network” alone is insufficient.
- Configure HSTS and CORS at the authoritative edge/application layer according to deployment topology.
- Never combine wildcard origins with credentials.
- CSP and frame protections apply when browser-rendered content exists; they are not automatically meaningful for a JSON-only API.
- Trust forwarded headers only from explicit proxy or network allow-lists; reject or ignore forwarding headers from untrusted sources.
- Validate the original scheme and host, enable host filtering where applicable, and document TLS termination ownership.

### 8.7 Conditional Boundary Controls

- For cookie-authenticated browser requests or other ambient credentials, protect state-changing operations against CSRF using the verified framework mechanism; apply appropriate `Secure`, `HttpOnly`, and `SameSite` cookie settings. CORS is not CSRF protection. Determine applicability from credential transport and browser behavior, not merely whether the API returns JSON.
- For webhooks, verify authenticity over the provider-required payload/bytes before accepting effects, use constant-time verification where applicable, and follow verified provider contracts for timestamp/replay checks. Persist event deduplication and idempotent processing; define bounded replay windows and key/token rotation with controlled overlap. An authenticity check alone does not prevent duplicate effects; IP allow-lists alone do not establish authenticity.
- For filesystem paths and archives, canonicalize and constrain destinations to an authorized root, reject traversal and unsafe names, and account for symlink/reparse-point and extraction behavior. Never trust an upload filename as a storage path.
- For untrusted serialized data, use verified safe parsing with explicit permitted types, size/depth limits, and validation. Do not enable arbitrary type activation or unsafe polymorphic deserialization.
- For subprocesses, prefer structured argument APIs verified on the selected runtime; allow-list commands/options and prevent option injection. Do not concatenate untrusted input into shell commands. Apply least privilege, time/output bounds, cancellation, and [core-governance.md Section 2.8 execution gates](./core-governance.md#28-execution-safety).

These controls are capability-dependent; do not prescribe an unverified library, algorithm API, provider header, or framework configuration.

### 8.8 Data Lifecycle and Privacy

For personal, regulated, or confidential data, define:
- classification, lawful basis or authorized business purpose, purpose limitation, and data minimization;
- consent lifecycle when consent is the applicable basis;
- retention, legal hold, deletion, and anonymization;
- data-subject request and export processes where applicable;
- residency, cross-border constraints, processor/subprocessor inventory, and data-sharing controls;
- breach detection, assessment, escalation, and notification obligations;
- privacy access controls, auditability, and log, trace, backup, and analytics retention;
- prohibition on copying production data into non-production environments without approval, minimization, and effective masking; prefer synthetic data;
- audited exports with explicit authorization, classification-appropriate watermarking when useful, expiration, download limits, and revocation where technically supported;
- classification-driven encryption, access, logging, backup, and retention controls;
- encryption/key deletion effects and verified disposal.
