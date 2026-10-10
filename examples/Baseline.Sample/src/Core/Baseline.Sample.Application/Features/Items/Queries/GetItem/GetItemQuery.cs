using Baseline.Sample.Application.Items;
using MediatR;

namespace Baseline.Sample.Application.Features.Items.Queries.GetItem;

/// <summary>Requests an owner-scoped item without mutating storage.</summary>
/// <param name="Id">Requested item identifier.</param>
/// <param name="OwnerId">Trusted authenticated actor identifier.</param>
public sealed record GetItemQuery(Guid Id, string OwnerId) : IRequest<ItemDto?>;
