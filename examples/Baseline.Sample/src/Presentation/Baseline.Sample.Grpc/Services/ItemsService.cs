using System.Security.Claims;
using Baseline.Sample.Application.Features.Items.Commands.CreateItem;
using Baseline.Sample.Application.Features.Items.Queries.GetItem;
using Baseline.Sample.Grpc.Contracts;
using FluentValidation;
using Grpc.Core;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace Baseline.Sample.Grpc.Services;

/// <summary>Translates owner-scoped protobuf calls into application requests without accepting client ownership.</summary>
[Authorize]
public sealed class ItemsService(ISender sender) : Contracts.Items.ItemsBase
{
    /// <summary>Retrieves an item visible to the authenticated actor, concealing other owners' records.</summary>
    public override async Task<ItemReply> Get(GetItemRequest request, ServerCallContext context)
    {
        var ownerId = GetOwner(context);
        if (!Guid.TryParse(request.Id, out var id) || id == Guid.Empty)
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "A non-empty item UUID is required."));
        }

        var item = await sender.Send(new GetItemQuery(id, ownerId), context.CancellationToken);
        if (item is null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, "Item not found."));
        }

        return new ItemReply { Id = item.Id.ToString("D"), Name = item.Name };
    }

    /// <summary>Creates an item for the authenticated actor and forwards cancellation to the application pipeline.</summary>
    public override async Task<ItemReply> Create(CreateItemRequest request, ServerCallContext context)
    {
        var ownerId = GetOwner(context);
        if (request.Name.Length > 100)
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Name must contain 1 to 100 characters."));
        }

        try
        {
            var item = await sender.Send(new CreateItemCommand(ownerId, request.Name), context.CancellationToken);
            return new ItemReply { Id = item.Id.ToString("D"), Name = item.Name };
        }
        catch (ValidationException)
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Name must contain 1 to 100 non-whitespace characters."));
        }
    }

    private static string GetOwner(ServerCallContext context)
    {
        var user = context.GetHttpContext().User;
        var ownerId = user.FindFirstValue(ClaimTypes.NameIdentifier);
        if (user.Identity?.IsAuthenticated != true || string.IsNullOrWhiteSpace(ownerId))
        {
            throw new RpcException(new Status(StatusCode.Unauthenticated, "Authentication is required."));
        }

        return ownerId;
    }
}
