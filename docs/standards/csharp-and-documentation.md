# C# Implementation and Documentation Governance Standards

> **Classification:** `[CORE STANDARD]`  
> **Status:** Normative Standard  
> **Applicability:** Active for C# code styling, naming, asynchronous patterns, and documentation authoring/lifecycle governance.  
> **Source Migration:** Formed from Section 14 of the Enterprise .NET Backend AI Engineering Guide.

---

## 14. C# and Documentation Standards

C# rules apply only to verified C# projects and supported features. Documentation rules in sections 14.1–14.5 apply only when the repository has adopted a documentation profile or the authorized task explicitly changes an already-maintained artifact. They MUST NOT create, split, relocate, or expand documentation outside scope. The tree below is a non-normative example appendix, not an active path standard.

- Use naming consistent with current .NET conventions: public members `PascalCase`, locals/parameters `camelCase`, interfaces prefixed with `I`, and private fields `_camelCase` when the repository uses fields.
- Use `var` when type is apparent and explicit type when it improves comprehension.
- Prefer `is null`, `is not null`, and `string.IsNullOrWhiteSpace` where appropriate.
- Async I/O methods normally end in `Async`; exceptions include framework-defined interface members such as MediatR `Handle`.
- Never block asynchronous work with `.Result`, `.Wait()`, or `.GetAwaiter().GetResult()` in request/worker paths.
- Prefer the simplest correct, secure, readable, and compatible implementation within verified repository boundaries. Avoid speculative abstractions and boilerplate; simple CRUD does not automatically require CQRS, MediatR, interfaces, extra repositories, or new layers.
- Add or update concise XML documentation on newly created or changed handwritten classes (including custom Attribute classes), controller actions or named endpoint handlers, properties, and fields, regardless of visibility. A change to one member does not require documenting every untouched declaration in its containing type.
- Use a brief `<summary>` explaining intent or contract rather than repeating the declaration name. Add `<param>`, `<returns>`, `<exception>`, or `<remarks>` only when relevant; use `<inheritdoc/>` when inherited documentation accurately describes the declaration and supported tooling resolves it.
- Property/field documentation describes member meaning, constraints, units, or side effects as relevant. Explain significant annotations such as `[Authorize]` on the annotated declaration when useful; annotations do not require separate XML comments.
- Exclude generated/vendor code and untouched legacy declarations. Do not add boilerplate to trivial private helper methods; document non-obvious invariants, failure behavior, or side effects where needed. Follow compatible repository analyzer conventions without weakening this touched-declaration policy.
- Verify coverage by inspecting touched declarations; compiler warnings for public XML documentation alone do not establish coverage of private members.

### 14.1 Non-Normative Documentation Structure Example

The following topic-based tree illustrates roles a repository MAY map into its adopted structure. Concrete paths are placeholders, not requirements or evidence that files exist. It does not authorize artifact creation, runtime or dependency changes, source relocation, or contract/schema changes.

```text
/
├── README.md                              # Project overview, quick start, documentation links
├── AGENTS.md                              # AI guardrails, repository profile, required workflow
├── CONTRIBUTING.md                        # Contribution, review, and verification workflow
├── CHANGELOG.md                           # Release-level change history
│
└── docs/
    ├── README.md                          # Documentation index and recommended reading paths
    │
    ├── getting-started/
    │   ├── prerequisites.md               # SDKs, tools, and required access
    │   ├── local-development.md           # Local setup and application startup
    │   └── first-api-request.md           # First request using CLI or Postman
    │
    ├── architecture/
    │   ├── overview.md                    # System context, components, and dependency flow
    │   ├── repository-map.md              # Actual project structure and responsibilities
    │   └── domain-glossary.md             # Shared business terminology and definitions
    │
    ├── adr/
    │   ├── README.md                      # Decision index, statuses, and approval process
    │   └── template.md                    # Context, decision, alternatives, consequences
    │
    ├── engineering/
    │   ├── coding-standards.md            # Naming, formatting, and implementation standards
    │   ├── how-to-add-feature.md          # End-to-end feature implementation playbook
    │   └── testing-guide.md               # Test strategy, commands, and required coverage
    │
    ├── api/
    │   ├── README.md                      # Endpoint index, contract authority, usage guidance
    │   ├── conventions.md                 # Dates, pagination, sorting, nulls, and identifiers
    │   ├── authentication.md              # Authentication flow and credential handling
    │   ├── response-and-error-contracts.md # Existing envelopes and stable error codes
    │   ├── compatibility-and-versioning.md # Compatible evolution and deprecation policy
    │   │
    │   ├── endpoints/
    │   │   └── <module>/
    │   │       ├── README.md              # Module workflows and endpoint index
    │   │       ├── create-<resource>.md    # Request, response, validation, and side effects
    │   │       ├── get-<resource>.md
    │   │       └── list-<resources>.md
    │   │
    │   ├── examples/
    │   │   └── <module>/
    │   │       └── create-<resource>/
    │   │           ├── request.json
    │   │           ├── response-success.json
    │   │           └── response-validation-error.json
    │   │
    │   ├── postman/
    │   │   ├── README.md                  # Import, setup, authentication, and safe execution
    │   │   ├── collections/
    │   │   │   └── backend-api.postman_collection.json
    │   │   ├── environments/
    │   │   │   ├── local.postman_environment.json
    │   │   │   └── staging.postman_environment.json
    │   │   └── data/                      # Optional synthetic collection-run input
    │   │       └── sample-inputs.json
    │   │
    │   └── templates/
    │       └── endpoint-template.md       # Required endpoint documentation sections
    │
    ├── data/
    │   ├── database-overview.md           # Data model, relationships, and storage boundaries
    │   ├── schema-ownership.md            # Schema authority and deployment ownership
    │   └── schema-change-process.md       # Approved schema change and compatibility workflow
    │
    ├── security/
    │   ├── overview.md                    # Security boundaries and applicable controls
    │   ├── authorization.md               # Permissions, ownership, and tenant isolation
    │   └── secrets-management.md          # Secret provisioning, storage, and rotation
    │
    ├── operations/
    │   ├── configuration.md               # Configuration keys without secret values
    │   ├── deployment.md                  # Deployment sequence and verification
    │   ├── observability.md               # Logs, metrics, traces, health, and alert ownership
    │   └── runbooks/
    │       ├── troubleshooting.md         # Symptoms, diagnosis, and recovery steps
    │       ├── rollback.md                # Rollback limits and forward-fix procedures
    │       └── backup-and-restore.md       # Recovery procedure and restore verification
    │
    ├── collaboration/
    │   ├── README.md                      # How to use specs and handoff records
    │   ├── developer-handoff-template.md  # Status, integration requirements, risks, and tests
    │   └── feature-spec-template.md       # Scope, behavior, acceptance criteria, open decisions
    │
    ├── releases/
    │   ├── unreleased.md                  # Changes pending release
    │   └── release-template.md            # Deployment, compatibility, and verification notes
    │
    └── assets/
        └── diagrams/                     # Architecture, sequence, and ERD assets with sources
```

- New documentation names SHOULD use lowercase kebab-case, preserving conventional filenames such as `README.md` and `AGENTS.md`.
- `<module>`, `<resource>`, and `<resources>` are placeholders, not literal names. Use actual domain terminology and add other operations only when applicable. The collection basename MAY use the repository name instead of `backend-api`.
- Create documents only when applicable and supported by meaningful content. Do not create empty folders or invented API examples to satisfy the tree. Postman runner data is optional.
- If the repository adopts this example, map its documentation-index role to the approved location. A missing example path does not mean guidance is absent and does not authorize creating it.

### 14.2 Documentation Ownership and Source of Truth

- When the illustrated documentation structure is adopted, `AGENTS.md` owns critical AI guardrails, the repository profile, and mandatory workflow; the root `README.md` owns onboarding links and quick start; `CONTRIBUTING.md` owns contribution and review procedures. Otherwise preserve the verified existing locations and authorized scope, including a single-file guide; this ownership model does not authorize creating or splitting files.
- Architecture documents describe verified system structure; engineering documents define implementation standards; ADRs record decisions and consequences; operational runbooks describe executable diagnosis and recovery procedures. Preserve the authority hierarchy in [core-governance.md Section 3.1](./core-governance.md#31-authority-hierarchy).
- Documentation MUST distinguish verified implementation, proposed behavior, and target standards. Owner, status, verification date, and implementation/test metadata apply only under an approved documentation lifecycle. MUST NOT invent ownership or advance a broad verification date from a partial review; record scope and unknowns explicitly.
- Under an adopted profile, the API-index role MUST identify the authoritative contract source when a specification is maintained. For generated specifications, document the source and verified generation procedure rather than maintaining a competing manual copy.
- Under an adopted profile, explanatory prose, fixtures, and executable request collections MUST agree with the authoritative contract. Prefer generated or validated contract reuse; maintain separate Markdown only when behaviorally useful. When artifacts conflict, identify every source, determine authority, characterize actual behavior, and escalate consumer-impacting ambiguity. MUST NOT alter runtime merely to match prose or alter a published contract merely to match accidental implementation.
- Feature specifications record scope, expected behavior, acceptance criteria, and unresolved decisions. Handoffs record implementation status, consumer impact, integration requirements, executed checks, remaining risks, and recovery guidance.
- If adopted, keep release-summary and detailed-release roles distinct according to [git-and-releases.md](./git-and-releases.md); do not duplicate full narratives across artifacts.

### 14.3 Endpoint Request and Response Documentation

When an adopted profile already maintains operation documentation, update affected operations in their verified location. Prefer generated/validated contract reuse; add separate Markdown only when the profile requires it and it adds behavioral value. The maintained form SHOULD cover the following where applicable:

1. Status, ownership, purpose, intended caller, implementation references, and relevant contract tests.
2. Actual HTTP method and route, content types, authentication scheme, required permissions, ownership checks, and tenant scope.
3. Request headers, path parameters, query parameters, and body fields: types, required/optional status, defaults, bounds, accepted formats, and validation constraints. Distinguish omitted fields, explicit `null`, and empty values when behavior differs.
4. A cURL or equivalent runnable request example using environment variables for credentials, plus synthetic payload examples. Use valid JSON fixtures under `examples/` when reuse warrants separate files; do not fabricate a body for bodyless requests or responses.
5. Successful and relevant failure responses: actual HTTP status, headers, serialized body, field meanings, nullable fields, existing stable error codes, triggering conditions, and client handling guidance.
6. Side effects, idempotency, concurrency/preconditions, consistency expectations, and retry safety, including ambiguous outcomes where applicable. Document unsupported capabilities honestly.
7. Verification scenarios for success, invalid input, authorization, not-found, duplicate/concurrent requests, and failures as applicable; identify checks actually performed versus planned.
8. Compatibility, deprecation, and migration notes when relevant.

Examples MUST reflect verified controller, DTO, serializer, validation, and test behavior. Preserve the repository's actual published success and error contracts, including compatibility envelopes where they exist. Do not invent routes, fields, status codes, permissions, or error codes to fit the template. Clearly label unverified proposals as `Draft`; never present illustrative endpoints as implemented.

### 14.4 Executable Request Collections and Environments

For maintained interactive clients, scripts, examples, load tools, test collections, or vendor-specific collections:

- Group collection requests by actual module. Use `{{baseUrl}}` and appropriate variables; inherit authentication where compatible with the endpoint. Include verified method, route, headers, parameters, body, a description, and a reference to the endpoint documentation.
- Include synthetic saved success/error examples and meaningful assertions against the actual contract where applicable. Saved examples are not proof that live requests or tests passed.
- Under an adopted executable-collection profile, its usage guidance MUST explain setup/import, authentication, dependent ordering, safe execution, examples, export where applicable, and update procedures.
- Commit only secret-free environment templates. Credential variables MUST be empty; use verified safe local addresses or placeholders for destinations and synthetic identifiers for example resources. Store active credentials outside version-controlled artifacts using an approved secret mechanism.
- Inspect exported collections, environments, scripts, saved responses, and runner data for active tokens, passwords, API keys, cookies, production payloads, sensitive internal topology, and personal data before committing. Do not rely on a UI masking option to sanitize an export.
- Production MUST NOT be an implicit target for any interactive client, script, example, load tool, or test collection. Destructive, financial, notification-sending, or chargeable requests MUST be excluded from safe defaults and require deliberate execution against an authorized environment. Labels alone are not safeguards.
- MUST NOT install runners, add dependencies, configure new CI infrastructure, or execute external requests merely to fulfill this documentation profile. Follow existing approval and execution-safety gates.

### 14.5 Incremental Adoption and Documentation Validation

- Preserve existing documentation paths until relocation is explicitly scoped. Existing documents remain authoritative within the [core-governance.md Section 3.1](./core-governance.md#31-authority-hierarchy) hierarchy until deliberately superseded. Update indexes, inbound links, and tooling references atomically when moving documentation; avoid leaving conflicting copies.
- Documentation changes MUST preserve the verified runtime/profile, source layout, public contracts, and schema authority. Any schema-process document describes the approved workflow; it does not mandate EF Core migrations or a new database technology.
- Without a separate reminder, synchronize affected maintained documentation in the same change as API behavior, configuration, operations, schema, or developer-guidance updates. Update repository-owned artifacts in authorized scope using existing locations and verified update procedures. MUST NOT manually edit externally owned, generated, or vendor outputs; use an authorized generation procedure when applicable, otherwise report ownership/scope blockers and required follow-up. Report no documentation impact when applicable. This does not authorize unrelated scaffolding, new watchers, pipelines, or dependencies.
- Before completing documentation work, review accuracy, paths, links/anchors, Markdown structure, duplication, and secret-free synthetic examples. Parse changed JSON artifacts and use existing schema/contract validation tooling where available; do not introduce a new tool dependency without approval.
- Separate target paths shown in this guide from references to existing files. Only create navigable links to available documents. Record checks actually executed and any missing validation; do not claim live API, Postman, deployment, or restore verification from a static documentation review.
