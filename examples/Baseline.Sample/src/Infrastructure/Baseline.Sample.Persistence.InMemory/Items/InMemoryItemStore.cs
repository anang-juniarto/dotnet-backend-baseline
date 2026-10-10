using System.Collections.Concurrent;
using Baseline.Sample.Application.Items;
using Baseline.Sample.Domain.Items;

namespace Baseline.Sample.Persistence.InMemory.Items;

/// <summary>Thread-safe, process-local demo storage. All items disappear when the host stops.</summary>
public sealed class InMemoryItemStore : IItemStore
{
    /// <summary>Immutable records shared only within this host instance; not durable or distributed.</summary>
    private readonly ConcurrentDictionary<Guid, Item> _items = new();

    /// <inheritdoc />
    public Task AddAsync(Item item, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        if (!_items.TryAdd(item.Id, item))
        {
            throw new InvalidOperationException("The item identifier is already present.");
        }

        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task<Item?> GetAsync(Guid id, string ownerId, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var found = _items.TryGetValue(id, out var item) &&
            string.Equals(item.OwnerId, ownerId, StringComparison.Ordinal);
        return Task.FromResult(found ? item : null);
    }
}
