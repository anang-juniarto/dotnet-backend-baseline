# Build artifacts and reference validation

These assets belong only to the isolated educational sample. They do not provision infrastructure, deploy the API, publish images, or certify optional providers.

## Active and reference GitHub workflows

The repository-root [baseline-validation.yml](../../../../.github/workflows/baseline-validation.yml) is active CI configuration for path-scoped pull requests, pushes, and manual dispatch. It restores, builds, and tests the default `Baseline.Sample.slnx` profile in Release, then runs `dotnet format Baseline.Sample.slnx --verify-no-changes --no-restore`. SDK selection comes from the sample's `global.json`; no SDK version is duplicated in the workflows. CI jobs log the resolved SDK (`dotnet --version` and `dotnet --info`), which may roll forward to a later patch under `latestPatch`, while the artifact Dockerfile remains pinned to exact SDK image `10.0.201`.

A separate `optional-host-compile` matrix restores and builds only `src/Presentation/Baseline.Sample.WebApi/Baseline.Sample.WebApi.csproj` in Release for five profiles: SQL Server, PostgreSQL, observability, realtime, and all four together. Each matrix entry has its own job workspace; no optional build shares outputs with the default integration tests, which assume optional flags remain false. These jobs do not start hosts or services, test external providers, publish artifacts, or deploy anything.

The root `reference-validation` job runs only `git diff --check` against scoped changed files under `docs`, `examples/Baseline.Sample`, `.github`, `.kilo`, root Markdown files, `.baseline-version`, and `.gitignore`. It compares the pull-request base or push-before SHA to `HEAD`; manual runs and initial pushes compare against the empty tree. This is a whitespace check, not a Markdown/link linter, schema validator, or security scan.

Both workflows use read-only repository tokens, checkout with `persist-credentials: false`, and bounded job timeouts. Action pins were verified using read-only `git ls-remote` against upstream tags: `actions/checkout` v7 resolves to `3d3c42e5aac5ba805825da76410c181273ba90b1`, and `actions/setup-dotnet` v6 resolves to `a98b56852c35b8e3190ac28c8c2271da59106c68`. Reverify upstream tags before updating pins. No deployment credentials or Sonar secrets are required.

The nested [pr-validation.yml](../../.github/workflows/pr-validation.yml) mirrors the default and optional-host jobs as reference material. GitHub discovers workflows only in the repository-root `.github/workflows` directory, so this nested workflow **does not run automatically from this repository root**, including its manual-dispatch trigger. Do not promote it alongside the active root workflow and duplicate validation. If the sample becomes its own repository, change the working directories to `.`, the SDK path to `global.json`, and the pull-request path filter to `**`.

## Docker build-only artifact export

The Dockerfile uses SDK `10.0.201`, matching the current `global.json`; update them together. While CI workflows log the resolved SDK and may roll forward to a later patch under `latestPatch`, the artifact Dockerfile remains pinned to exact SDK image `10.0.201`. It restores, builds, tests the solution, and publishes the API project at `src/Presentation/Baseline.Sample.WebApi`. The final `scratch` stage contains framework-dependent publish files only. It has **no runtime, ENTRYPOINT, CMD, or EXPOSE**: attempting to run it without an explicit command fails closed rather than starting the API.

From `examples/Baseline.Sample`, an explicitly authorized local build can export files with:

```sh
docker buildx build --file deploy/docker/Dockerfile --target artifacts --output type=local,dest=artifacts .
```

Use the sample directory, not the repository root, as the build context so its `.dockerignore` applies. The ignore file excludes local build outputs, common private-key/secret files, editor state, and generated graph data; it is not a substitute for keeping credentials out of the source tree. Builds may download the SDK image and NuGet packages. This command is documentation only and was not executed as part of adding these assets.

The API intentionally binds loopback and provides only educational, Development-gated demo authentication. Mapping container ports would not make that host externally reachable, and changing its listener or authentication here would violate its safety boundary. Run the sample locally according to its main README; runtime containerization requires a separately approved host/authentication design. Published artifacts are not a production deployment package or proof of safe operation.

## Deliberately omitted schema assets

- Runtime Dockerfile/image, Compose services, port mappings, and container health checks: no approved container-safe host or external services.
- Kubernetes, Helm, Terraform, cloud account configuration, release/deployment workflows, and registry pushes: no deployment target selected.
- Sonar configuration, secret placeholders, and security-gate claims: no scanner/account policy selected. No vulnerability workflow is included; package advisory reporting alone is not a failing security gate.

Validation of these files must distinguish local static checks from actual GitHub execution and Docker builds. Neither nested workflow execution nor container/provider certification is implied by a successful local .NET build.
