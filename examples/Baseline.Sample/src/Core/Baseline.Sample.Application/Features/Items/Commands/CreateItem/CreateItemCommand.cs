using Baseline.Sample.Application.Items;
using MediatR;

namespace Baseline.Sample.Application.Features.Items.Commands.CreateItem;

/// <summary>Requests a new item for a trusted server-resolved actor.</summary>
/// <param name="OwnerId">Authenticated actor identifier, never supplied by the request body.</param>
/// <param name="Name">Display name to validate and normalize.</param>
public sealed record CreateItemCommand(string OwnerId, string Name) : IRequest<ItemDto>;
