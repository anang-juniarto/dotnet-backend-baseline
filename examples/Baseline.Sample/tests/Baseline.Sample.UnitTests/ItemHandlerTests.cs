using Baseline.Sample.Application.Features.Items.Commands.CreateItem;
using Baseline.Sample.Application.Features.Items.Queries.GetItem;
using Baseline.Sample.Application.Items;
using Baseline.Sample.Domain.Items;
using Xunit;

namespace Baseline.Sample.UnitTests;

/// <summary>Verifies application orchestration with a recording port rather than a persistence adapter.</summary>
public sealed class ItemHandlerTests
{
    /// <summary>Create projects the normalized domain item and propagates cancellation to its port.</summary>
    [Fact]
    public async Task CreateProjectsItemAndPropagatesToken()
    {
        var store = new RecordingStore();
        var token = TestContext.Current.CancellationToken;
        var result = await new CreateItemCommandHandler(store).Handle(new CreateItemCommand("alice", " item "), token);
        Assert.NotNull(store.Added);
        Assert.Equal(store.Added.Id, result.Id);
        Assert.Equal("item", result.Name);
        Assert.Equal("alice", store.Added.OwnerId);
        Assert.Equal(token, store.Token);
    }

    /// <summary>Invalid domain input cannot reach the mutation port even when a handler is invoked directly.</summary>
    [Fact]
    public async Task InvalidCreateDoesNotCallStore()
    {
        var store = new RecordingStore();
        await Assert.ThrowsAnyAsync<ArgumentException>(() =>
            new CreateItemCommandHandler(store).Handle(new CreateItemCommand("alice", " "), TestContext.Current.CancellationToken));
        Assert.Null(store.Added);
    }

    /// <summary>Query passes the exact resource, case-sensitive owner and cancellation token to storage.</summary>
    [Fact]
    public async Task QueryPropagatesLookupAndProjectsResult()
    {
        var item = new Item(Guid.NewGuid(), "Alice", "item");
        var store = new RecordingStore { Result = item };
        var token = TestContext.Current.CancellationToken;
        var result = await new GetItemQueryHandler(store).Handle(new GetItemQuery(item.Id, "Alice"), token);
        Assert.NotNull(result);
        Assert.Equal(item.Id, result.Id);
        Assert.Equal(item.Name, result.Name);
        Assert.Equal(item.Id, store.RequestedId);
        Assert.Equal("Alice", store.RequestedOwner);
        Assert.Equal(token, store.Token);
    }

    /// <summary>A missing port result remains absent rather than creating a synthetic DTO.</summary>
    [Fact]
    public async Task QueryPreservesMissingResult()
    {
        Assert.Null(await new GetItemQueryHandler(new RecordingStore()).Handle(
            new GetItemQuery(Guid.NewGuid(), "alice"), TestContext.Current.CancellationToken));
    }

    /// <summary>Records calls without supplying persistence behavior.</summary>
    private sealed class RecordingStore : IItemStore
    {
        /// <summary>Gets the item passed to the mutation port.</summary>
        public Item? Added { get; private set; }
        /// <summary>Gets or sets the query result supplied by the test.</summary>
        public Item? Result { get; init; }
        /// <summary>Gets the cancellation token received by the latest call.</summary>
        public CancellationToken Token { get; private set; }
        /// <summary>Gets the resource identifier passed to the lookup port.</summary>
        public Guid RequestedId { get; private set; }
        /// <summary>Gets the exact owner passed to the lookup port.</summary>
        public string? RequestedOwner { get; private set; }

        /// <summary>Records the immutable item and token without storing data.</summary>
        public Task AddAsync(Item item, CancellationToken cancellationToken)
        {
            Added = item;
            Token = cancellationToken;
            return Task.CompletedTask;
        }

        /// <summary>Records lookup inputs and returns the configured response.</summary>
        public Task<Item?> GetAsync(Guid id, string ownerId, CancellationToken cancellationToken)
        {
            RequestedId = id;
            RequestedOwner = ownerId;
            Token = cancellationToken;
            return Task.FromResult(Result);
        }
    }
}
