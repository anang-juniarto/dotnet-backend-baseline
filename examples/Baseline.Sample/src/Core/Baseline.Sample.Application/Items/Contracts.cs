namespace Baseline.Sample.Application.Items;

/// <summary>The public item projection; owner identity is never serialized.</summary>
/// <param name="Id">Stable server-allocated item identifier.</param>
/// <param name="Name">Trimmed item display name.</param>
public sealed record ItemDto(Guid Id, string Name);
