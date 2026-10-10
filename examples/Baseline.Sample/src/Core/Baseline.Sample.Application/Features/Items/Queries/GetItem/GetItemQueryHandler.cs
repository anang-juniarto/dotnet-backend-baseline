using Baseline.Sample.Application.Items;
using MediatR;

namespace Baseline.Sample.Application.Features.Items.Queries.GetItem;

/// <summary>Retrieves an owner-filtered projection without exposing ownership.</summary>
public sealed class GetItemQueryHandler(IItemStore store) : IRequestHandler<GetItemQuery, ItemDto?>
{
    /// <summary>Returns null for both missing items and items owned by another actor.</summary>
    public async Task<ItemDto?> Handle(GetItemQuery query, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(query.OwnerId);
        var item = await store.GetAsync(query.Id, query.OwnerId, cancellationToken);
        return item is null ? null : new ItemDto(item.Id, item.Name);
    }
}
