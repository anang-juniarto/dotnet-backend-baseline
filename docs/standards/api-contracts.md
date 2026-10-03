# HTTP/API Contracts and Resource Governance Standards

> **Classification:** `[CORE STANDARD]`  
> **Status:** Normative Standard  
> **Applicability:** Active for any task that defines, modifies, or deprecates HTTP endpoints, API contracts, serialization shapes, pagination, or API specifications.  
> **Source Migration:** Formed from Section 7 of the Enterprise .NET Backend AI Engineering Guide.

---

## 7. HTTP/API Contracts and Resource Governance

### 7.1 HTTP Semantics

- Follow RFC 9110 semantics for safety, idempotency, conditional requests, status codes, and caching.
- Use resource-oriented, stable, lower-case routes unless an existing contract requires otherwise.
- Creation SHOULD return `201 Created` with `Location`; successful deletion commonly returns `204 No Content`.
- Validate route, query, header, and body inputs with explicit size/range limits.

### 7.2 Response and Error Contract Profiles

Valid profiles include:
- plain success DTOs plus RFC 9457 Problem Details for errors;
- repository-defined success/error envelopes;
- compatibility envelopes required by existing consumers.

Presentation owns HTTP mapping. Existing published contracts take precedence. Clients bind to stable machine-readable error codes, not human-readable messages. Responses MUST NOT expose stack traces, SQL, internal hostnames, connection strings, or framework exception types.

### 7.3 Versioning and Deprecation

- Select URI, header, media-type, or compatible evolution through an ADR/API governance decision.
- Avoid a new version when an additive backward-compatible change suffices.
- The `Deprecation` header and RFC 8594 `Sunset` header have distinct specifications; validate their syntax against current standards.
- Deprecation requires consumer notice, migration documentation, telemetry on remaining usage, and a published removal date.

### 7.4 Pagination, Filtering, and Sorting

- Apply filtering, ordering, projection, and pagination in the data store.
- Enforce maximum page sizes and sorting allow-lists.
- Add a unique deterministic tie-breaker.
- Prefer keyset/cursor pagination when measured deep-offset cost or consistency requirements justify it.

### 7.5 Conditional Requests and API Concurrency

When clients can concurrently update resources or cache validation materially reduces transfer cost:
- use `ETag` and `If-None-Match` for representation validation where supported;
- use `If-Match` or an equivalent version precondition to prevent lost updates;
- return `412 Precondition Failed` when a required precondition does not match;
- keep database concurrency tokens and HTTP validators consistent without exposing sensitive internal state;
- preserve legacy behavior until a contract-tested migration is approved.

### 7.6 API Specification Governance

When an API specification such as OpenAPI is a published contract:
- keep routes, operation identifiers, schemas, status codes, headers, and stable error codes synchronized with implementation;
- run compatibility or breaking-change detection in CI when approved tooling exists;
- require explicit approval and consumer migration for breaking specification changes;
- distinguish public, partner, and internal operations so sensitive/internal endpoints are not published unintentionally;
- validate generated documentation and examples without exposing secrets or internal topology.

### 7.7 Inbound Resource Governance

When an endpoint is public, expensive, abuse-prone, or multi-tenant:
- apply rate limits partitioned by authenticated identity, tenant, API key, or trusted client rather than IP alone;
- enforce concurrency and queue limits with backpressure;
- cap request bodies, uploads, collection sizes, query complexity, and execution duration;
- return `429 Too Many Requests` and `Retry-After` where appropriate;
- document exemptions and trusted internal traffic through policy, not hard-coded bypasses.
