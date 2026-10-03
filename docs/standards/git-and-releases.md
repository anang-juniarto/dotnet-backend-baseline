# Git, Release, and Repository Metadata Standards

> **Classification:** `[CORE STANDARD]`  
> **Status:** Normative Standard  
> **Applicability:** Active when proposing Git branches, commit messages, pull requests, release staging, or changelog updates.  
> **Source Migration:** Formed from Section 16 of the Enterprise .NET Backend AI Engineering Guide.

---

## 16. Git, Release, and Repository Metadata

Discover actual repository-specific branch, commit, pull-request, and release-note conventions. Git advice is needed only when requested or required by applicable policy; a task need not produce a branch or commit recommendation. If advice is requested and conventions are absent, Conventional Commits and concise kebab-case branch names without personal data MAY be proposed, not imposed.

For any repository, source, schema, runtime configuration, CI, or API changes update its existing required release artifacts when policy requires it. Paths such as `docs/releases/unreleased.md` and `CHANGELOG.md` are examples, not proof those files exist or a mandate to create them. Extensive deployment artifacts are proportional to actual release risk, not required for every small edit.

Recommendations do not authorize Git mutation. Branch creation/switching, commits, amendments, pushes, and pull requests still require explicit authorization under [core-governance.md Section 2.1](./core-governance.md#21-role-and-communication).
