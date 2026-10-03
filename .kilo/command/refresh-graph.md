---
description: Refresh the Graphify structural dependency index in .kilo/cache/graphify/
agent: team-lead
subtask: true
---
Refresh the repository's Graphify structural dependency index.

## Workflow
1. Execute the index generation script:
   ```powershell
   & ".kilo\scripts\graphify-index.ps1" -TargetRoot (Get-Location).Path
   ```
2. Read generated `.kilo/cache/graphify/summary.json` status.
3. Report status to user:
   - If `indexed`: State node count, edge count, and generated timestamp.
   - If `pending_source`: Report that no project manifests were found to index.
   - If `unavailable`: Report that Graphify CLI is not installed; manifest fallback remains active.
