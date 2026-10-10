namespace Baseline.Sample.Infrastructure.Messaging.Contracts.V1;

/// <summary>Version-one integration notification containing identifiers only, not owner details or item names.</summary>
/// <param name="EventId">Stable identifier reused by the caller on retries; receivers must deduplicate it.</param>
/// <param name="ItemId">Identifier of the created item.</param>
/// <param name="OccurredAtUtc">UTC timestamp supplied by the originating operation.</param>
public sealed record ItemCreatedV1(Guid EventId, Guid ItemId, DateTimeOffset OccurredAtUtc);
