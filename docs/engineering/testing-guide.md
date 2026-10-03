# Testing Strategy & Quality Assurance Guide

> **Document Metadata**:  
> `Status: Draft` | `Owner: QA & Engineering Leads` | `Last verified: Not verified` | `Evidence: Target test portfolio`

This document defines testing standards, test portfolio balance, and verification rules for the backend service.

---

## 1. Risk-Based Test Portfolio

```text
       /\
      /  \       E2E Tests (Few critical customer journeys)
     /----\      Contract Tests (HTTP & message schema compatibility)
    /------\     Integration Tests (Database, cache, transactions)
   /--------\    Unit & Handler Tests (Invariants, pure logic, validation)
```

| Test Type | Scope & SUT | Dependencies | Execution Speed |
|---|---|---|---|
| **Unit Tests** | Domain entities, value objects, utility algorithms | In-memory only; NO database, NO network | Milliseconds |
| **Handler Tests** | Application use-case handlers | Mocked ports (repositories, external clients) | Fast |
| **Integration Tests** | Persistence repositories, DbContext queries, cache | Local test database (Testcontainers or test instance) | Moderate |
| **Contract Tests** | API controllers, serialization, error envelopes | WebApplicationFactory, in-memory test server | Moderate |
| **Concurrency Tests** | Row locks, optimistic tokens, idempotency deduplication | Real database engine (verifying isolation levels) | Slower |

---

## 2. Test Quality Principles

1. **Meaningful Assertions**: Assertion-free tests and `Assert.True(true)` are FORBIDDEN. Assert specific state transitions, outputs, or emitted integration events.
2. **Determinism & Time Control**:
   - Use `TimeProvider` (or repository clock abstraction) instead of `DateTime.UtcNow`.
   - Never rely on `Thread.Sleep()`; use task completion or deterministic test timeouts.
3. **No External Network Calls**: Unit and handler tests MUST NOT call live third-party services. Mock external network I/O.
4. **Isolated Test Data**: Tests MUST NOT share mutable static state. Ensure each test suite cleans up or runs in isolated database transactions.

---

## 3. Running Automated Tests

Run all unit and integration tests:
```bash
dotnet test --configuration Release --verbosity normal
```

Run a targeted test suite:
```bash
dotnet test tests/<Project>.UnitTests/<Project>.UnitTests.csproj --filter "FullyQualifiedName~FeatureName"
```

---

## 4. Verification Reporting Standards

When reporting test results (especially in AI agent handoffs and pull requests):
- **Passed Checks**: State exact test suites executed and passed count.
- **Failed Checks**: Detail failure message, stack trace, and reproduction step.
- **Unexecuted Checks**: Explicitly list tests that could NOT be executed (e.g., missing local database, external credentials unavailable) with rationale.
- **Rule**: NEVER report a build, test, or lint step as passed unless it was executed and exited with zero errors.
