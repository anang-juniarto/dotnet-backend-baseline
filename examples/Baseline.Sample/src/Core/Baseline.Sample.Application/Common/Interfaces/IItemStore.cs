using Baseline.Sample.Domain.Items;

namespace Baseline.Sample.Application.Items;

/// <summary>Stores items and performs owner-filtered lookups without exposing storage details.</summary>
public interface IItemStore
{
    /// <summary>Accepts a new immutable item; cancellation is checked before mutation.</summary>
    Task AddAsync(Item item, CancellationToken cancellationToken);

    /// <summary>Returns the item only when its owner matches, otherwise null.</summary>
    Task<Item?> GetAsync(Guid id, string ownerId, CancellationToken cancellationToken);
}
