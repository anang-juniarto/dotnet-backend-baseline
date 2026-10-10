using System.Security.Claims;
using Baseline.Sample.Application.Features.Items.Commands.CreateItem;
using Baseline.Sample.Application.Features.Items.Queries.GetItem;
using Baseline.Sample.Application.Items;
using Baseline.Sample.WebApi.Contracts.Items;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Baseline.Sample.WebApi.Controllers;

/// <summary>Authenticated educational CRUD transport, with owner isolation enforced by the application store.</summary>
[ApiController]
[Authorize]
[Route("api/items")]
public sealed class ItemsController(ISender sender) : ControllerBase
{
    /// <summary>Creates one item for this actor; retries create additional items.</summary>
    [HttpPost]
    public async Task<ActionResult<ItemDto>> Create(CreateItemRequest request, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new CreateItemCommand(User.FindFirstValue(ClaimTypes.NameIdentifier)!, request.Name!), cancellationToken);
        return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
    }

    /// <summary>Returns this actor's item, or the same not-found response used for missing items.</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ItemDto>> Get(Guid id, CancellationToken cancellationToken)
    {
        var item = await sender.Send(new GetItemQuery(id, User.FindFirstValue(ClaimTypes.NameIdentifier)!), cancellationToken);
        return item is null ? NotFound() : Ok(item);
    }
}
