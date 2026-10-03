# API Compatibility & Versioning Policy

> **Document Metadata**:  
> `Status: Draft` | `Owner: API Governance Team` | `Last verified: Not verified` | `Evidence: Versioning policy`

This document defines rules for evolving API contracts compatibly and managing the deprecation lifecycle.

---

## 1. Compatible Evolution Rules (Non-Breaking Changes)

The following changes are backward-compatible and DO NOT require a major version bump:
- Adding new optional request query parameters or request body properties.
- Adding new properties to response JSON payloads (clients SHOULD tolerate unknown fields).
- Adding new distinct endpoints or HTTP verbs to existing resources.
- Adding new optional HTTP request headers.

---

## 2. Breaking Changes & Major Versioning

The following changes are breaking and REQUIRE explicit approval and major versioning:
- Removing or renaming an existing route, parameter, or response property.
- Changing the data type or semantic meaning of an existing field.
- Introducing a new required request parameter or body property.
- Restricting permissible values or changing default values.
- Altering published machine-readable error codes.

---

## 3. Versioning Strategy

The repository selects its versioning strategy via architecture decision:
- **URI Path Versioning (Default)**: `/api/v1/orders`, `/api/v2/orders`.
- **Header Versioning**: `Accept: application/vnd.company.v2+json`.

---

## 4. Deprecation & Sunset Lifecycle

When retiring an endpoint or major version:
1. **Notice Phase**: Minimum 90 days notice before scheduled removal.
2. **Deprecation Headers**: Include standard RFC 8594 headers on deprecated responses:
   ```http
   Deprecation: @1767225600
   Sunset: Fri, 31 Dec 2026 23:59:59 GMT
   Link: <https://api.example.com/docs/migration-v2>; rel="deprecation"
   ```
3. **Telemetry & Monitoring**: Track remaining client traffic on deprecated routes before executing complete shutdown.
