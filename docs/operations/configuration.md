# Application Configuration Schema & Validation

> **Document Metadata**:  
> `Status: Draft` | `Owner: DevOps / Backend Leads` | `Last verified: Not verified` | `Evidence: Options schema`

This document defines strongly typed configuration keys, default options, and startup validation rules.

---

## 1. Strongly Typed Options Pattern

The backend service uses the ASP.NET Core `IOptions<TOptions>` or `IOptionsSnapshot<TOptions>` pattern. Configuration keys are bound to immutable records or POCO classes.

Example Configuration Structure:
```csharp
public sealed record DatabaseOptions
{
    public const string SectionName = "Database";

    [Required]
    public string ConnectionString { get; init; } = string.Empty;

    [Range(5, 500)]
    public int MaxPoolSize { get; init; } = 100;

    [Range(1, 60)]
    public int CommandTimeoutSeconds { get; init; } = 30;
}
```

---

## 2. Startup Validation (Fail-Fast)

All required configuration sections MUST be validated at application startup using `.ValidateDataAnnotations()` and `.ValidateOnStart()`:

```csharp
builder.Services.AddOptions<DatabaseOptions>()
    .BindConfiguration(DatabaseOptions.SectionName)
    .ValidateDataAnnotations()
    .ValidateOnStart();
```
*Rule: If a mandatory configuration key is missing or invalid, the service MUST fail immediately at startup rather than throwing runtime exceptions during customer requests.*

---

## 3. Configuration Reference Map (No Secret Values)

| Key Path | Type | Default Value | Required | Purpose |
|---|---|---|---|---|
| `Database:MaxPoolSize` | int | `100` | No | Connection pool upper bound |
| `Database:CommandTimeoutSeconds` | int | `30` | No | Default SQL execution timeout |
| `Cache:DefaultTtlMinutes` | int | `60` | No | Default Redis cache expiration |
| `RateLimiting:PermitLimit` | int | `100` | No | Requests allowed per window |
| `RateLimiting:WindowSeconds` | int | `60` | No | Rate limit sliding window span |
