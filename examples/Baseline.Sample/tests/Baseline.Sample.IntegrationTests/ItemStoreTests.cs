using Baseline.Sample.Application.Features.Items.Commands.CreateItem;
using Baseline.Sample.Application.Features.Items.Queries.GetItem;
using Baseline.Sample.Domain.Items;
using Baseline.Sample.Persistence.InMemory.Items;
using Xunit;

namespace Baseline.Sample.IntegrationTests;

/// <summary>Exercises application handlers against the deliberately volatile owner-scoped storage adapter.</summary>
public sealed class ItemStoreTests
{
    /// <summary>Create and query handlers share storage while enforcing case-sensitive ownership.</summary>
    [Fact]
    public async Task HandlersCreateAndIsolateOwners()
    {
        var store = new InMemoryItemStore();
        var created = await new CreateItemCommandHandler(store).Handle(new CreateItemCommand("alice", " item "), TestContext.Current.CancellationToken);
        var query = new GetItemQueryHandler(store);
        Assert.NotEqual(Guid.Empty, created.Id);
        Assert.Equal("item", created.Name);
        Assert.Equal(created, await query.Handle(new GetItemQuery(created.Id, "alice"), TestContext.Current.CancellationToken));
        Assert.Null(await query.Handle(new GetItemQuery(created.Id, "bob"), TestContext.Current.CancellationToken));
        Assert.Null(await query.Handle(new GetItemQuery(created.Id, "Alice"), TestContext.Current.CancellationToken));
        Assert.Null(await query.Handle(new GetItemQuery(Guid.NewGuid(), "alice"), TestContext.Current.CancellationToken));
    }

    /// <summary>Creating the same input twice does not promise command idempotency.</summary>
    [Fact]
    public async Task CreateAllocatesDistinctIdentifiers()
    {
        var handler = new CreateItemCommandHandler(new InMemoryItemStore());
        var first = await handler.Handle(new CreateItemCommand("alice", "x"), TestContext.Current.CancellationToken);
        var second = await handler.Handle(new CreateItemCommand("alice", "x"), TestContext.Current.CancellationToken);
        Assert.NotEqual(first.Id, second.Id);
    }

    /// <summary>A canceled operation cannot write into the demo store.</summary>
    [Fact]
    public async Task CancellationPreventsMutation()
    {
        var store = new InMemoryItemStore();
        using var cancellation = new CancellationTokenSource();
        cancellation.Cancel();
        var item = new Item(Guid.NewGuid(), "alice", "x");
        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => store.AddAsync(item, cancellation.Token));
        Assert.Null(await store.GetAsync(item.Id, "alice", TestContext.Current.CancellationToken));
        await Assert.ThrowsAnyAsync<OperationCanceledException>(() =>
            new CreateItemCommandHandler(store).Handle(new CreateItemCommand("alice", "x"), cancellation.Token));
    }

    /// <summary>A duplicate identifier cannot overwrite an existing owner's item.</summary>
    [Fact]
    public async Task DuplicatePreservesOriginal()
    {
        var store = new InMemoryItemStore();
        var id = Guid.NewGuid();
        await store.AddAsync(new Item(id, "alice", "original"), TestContext.Current.CancellationToken);
        await Assert.ThrowsAsync<InvalidOperationException>(() => store.AddAsync(new Item(id, "bob", "changed"), TestContext.Current.CancellationToken));
        Assert.Equal("original", (await store.GetAsync(id, "alice", TestContext.Current.CancellationToken))!.Name);
        Assert.Null(await store.GetAsync(id, "bob", TestContext.Current.CancellationToken));
    }

    /// <summary>A different store instance has no persistent or shared state.</summary>
    [Fact]
    public async Task NewInstanceLosesState()
    {
        var store = new InMemoryItemStore();
        var item = new Item(Guid.NewGuid(), "alice", "x");
        await store.AddAsync(item, TestContext.Current.CancellationToken);
        Assert.Null(await new InMemoryItemStore().GetAsync(item.Id, "alice", TestContext.Current.CancellationToken));
    }
}
