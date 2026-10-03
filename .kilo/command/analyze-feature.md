---
description: Analyze requirements and author a structured feature specification
agent: system-analyst
subtask: true
---
Analyze the feature requirements for: $ARGUMENTS

Workflow:
1. Deconstruct the request into business context, goals, and explicit non-goals.
2. Produce a specification following `docs/collaboration/feature-spec-template.md`.
3. Define request/response DTO contracts, invariants, concurrency requirements, and Given/When/Then acceptance criteria.
4. Output the specification in Markdown without generating C# implementation code.
