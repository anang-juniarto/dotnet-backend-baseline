---
description: Review changed code against C# standards, async safety, and security guardrails
agent: code-reviewer
subtask: true
---
Review code changes for: $ARGUMENTS

Workflow:
1. Identify modified files from git diff or explicitly provided file list.
2. Evaluate against `docs/engineering/coding-standards.md` and `docs/security/overview.md`.
3. Check async safety (CancellationToken propagation, zero blocking calls), DI lifetimes, and parameterized queries.
4. Verify response contract matches `docs/api/response-and-error-contracts.md`.
5. Output findings with exact `file_path:line`, severity, and concrete remediation.
