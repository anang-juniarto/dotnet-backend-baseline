# Collaboration, Specifications & Handoff Governance

> **Document Metadata**:  
> `Status: Draft` | `Owner: Engineering Leads` | `Last verified: Not verified` | `Evidence: Collaboration standards`

This document defines how engineers and AI agents author feature specifications, document architectural decisions, and perform structured developer handoffs.

---

## 1. Specification Lifecycle

Before writing code for non-trivial features:
1. **Author Feature Spec**: Use [`feature-spec-template.md`](./feature-spec-template.md) to define goals, non-goals, actors, contracts, and invariants.
2. **Review & Approve**: Product and technical stakeholders review and approve the specification.
3. **Implementation**: Code and tests are authored following [`docs/engineering/how-to-add-feature.md`](../engineering/how-to-add-feature.md).
4. **Handoff**: Use [`developer-handoff-template.md`](./developer-handoff-template.md) to summarize implemented work, executed tests, and operational considerations.

---

## 2. Available Templates

- [`feature-spec-template.md`](./feature-spec-template.md): Structured blueprint for new business features.
- [`developer-handoff-template.md`](./developer-handoff-template.md): Structured handoff record between engineers, AI agents, and code reviewers.
