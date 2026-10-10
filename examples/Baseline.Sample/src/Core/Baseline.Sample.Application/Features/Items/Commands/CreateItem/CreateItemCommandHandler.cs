using Baseline.Sample.Application.Items;
using Baseline.Sample.Domain.Items;
using MediatR;

namespace Baseline.Sample.Application.Features.Items.Commands.CreateItem;

/// <summary>Stores a new domain-validated item through its application port.</summary>
public sealed class CreateItemCommandHandler(IItemStore store) : IRequestHandler<CreateItemCommand, ItemDto>
{
    /// <summary>Creates one item; retries create additional items rather than deduplicating writes.</summary>
    public async Task<ItemDto> Handle(CreateItemCommand command, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var item = new Item(Guid.NewGuid(), command.OwnerId, command.Name);
        await store.AddAsync(item, cancellationToken);
        return new ItemDto(item.Id, item.Name);
    }
}
