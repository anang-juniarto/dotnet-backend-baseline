using Baseline.Sample.Application.Items;
using Baseline.Sample.Domain.Items;
using Baseline.Sample.Persistence.EntityFramework.Context;
using Microsoft.EntityFrameworkCore;

namespace Baseline.Sample.Persistence.EntityFramework.Repositories;

/// <summary>Persists immutable items and restricts reads to the requested owner.</summary>
public sealed class EntityFrameworkItemStore<TContext>(TContext context) : IItemStore
    where TContext : ApplicationDbContext
{
    /// <summary>Inserts a new item and commits it without automatically retrying ambiguous writes.</summary>
    public async Task AddAsync(Item item, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(item);
        cancellationToken.ThrowIfCancellationRequested();
        var entry = context.Add(item);
        try
        {
            await context.SaveChangesAsync(cancellationToken);
        }
        finally
        {
            entry.State = EntityState.Detached;
        }
    }

    /// <summary>Uses an owner-filtered, untracked query and verifies ordinal owner equality.</summary>
    public async Task<Item?> GetAsync(Guid id, string ownerId, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(ownerId);
        cancellationToken.ThrowIfCancellationRequested();
        var item = await context.Set<Item>()
            .AsNoTracking()
            .SingleOrDefaultAsync(value => value.Id == id && value.OwnerId == ownerId, cancellationToken);
        return item is not null && string.Equals(item.OwnerId, ownerId, StringComparison.Ordinal)
            ? item
            : null;
    }
}
