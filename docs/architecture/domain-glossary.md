# Domain Glossary & Ubiquitous Language

> **Document Metadata**:  
> `Status: Draft` | `Owner: Domain Experts / Engineering Team` | `Last verified: Not verified` | `Evidence: Initial glossary template`

This glossary establishes the shared, unambiguous business terminology (Ubiquitous Language) used across code, database schemas, APIs, and documentation.

---

## 1. Core Domain Concepts

| Term | Context / Module | Definition & Invariants | Synonyms / Deprecated Terms |
|---|---|---|---|
| **Account** | Identity & Access | Represents an authenticated user profile and security principal. | User, UserAccount |
| **Tenant** | Multi-Tenancy | Isolated organizational boundary owning data, users, and resources. | Organization, Workspace |
| **Transaction** | Financial / Ledger | An immutable, balance-affecting monetary debit or credit entry. | PaymentEntry, Transfer |
| **Wallet** | Financial / Billing | Authoritative monetary balance holder for a specific user/tenant. | BalanceAccount |
| **Webhook** | Integration | Asynchronous HTTP notification sent to or received from external partners. | Callback, IPN |

---

## 2. Invariant & Precision Glossary

- **Idempotency Key**: A unique client-provided or hash-generated key ensuring a non-idempotent operation is executed at most once.
- **Monetary Amount**: A numerical value representing money, stored as exact decimal or integer minor units, paired with an ISO 4217 currency code. Binary floating point is FORBIDDEN.
- **Optimistic Concurrency Token**: A row version or timestamp verifying that an entity has not mutated concurrently prior to update.

---

## 3. Maintenance Rules

- When introducing a new domain concept or entity in code, add its entry to this table.
- Code identifiers (classes, methods, database tables) MUST align with the terms defined in this glossary.
