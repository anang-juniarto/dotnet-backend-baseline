---
description: Diagnose an issue or runtime failure with evidence before proposing fixes
agent: team-lead
subtask: true
---
Diagnose the following issue: $ARGUMENTS

Workflow:
1. Collect reproduction steps, observed behavior, and expected behavior.
2. Follow `docs/operations/runbooks/troubleshooting.md` to establish evidence-based hypotheses.
3. Inspect relevant logs, telemetry traces, or code paths citing `file_path:line_number`.
4. Report root cause analysis with verified facts, assumptions, and unknowns.
5. DO NOT apply speculative code modifications until the root cause is proven.
