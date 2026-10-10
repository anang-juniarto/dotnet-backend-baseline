using Baseline.Sample.Application.Items;
using Baseline.Sample.Domain.Items;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Baseline.Sample.Persistence.MongoDb;

/// <summary>Names an explicitly selected, externally provisioned MongoDB database and collection.</summary>
public sealed record MongoDbOptions
{
    /// <summary>Gets the database name; no database is initialized by registration.</summary>
    public required string DatabaseName { get; init; }

    /// <summary>Gets the item collection name.</summary>
    public required string CollectionName { get; init; }

    internal void Validate()
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(DatabaseName);
        ArgumentException.ThrowIfNullOrWhiteSpace(CollectionName);
        if (DatabaseName.IndexOfAny(['/', '\\', '.', ' ', '"', '$', '\0']) >= 0 ||
            CollectionName.Contains('$') || CollectionName.Contains('\0'))
        {
            throw new ArgumentException("MongoDB database or collection name is invalid.");
        }
    }
}

/// <summary>Persists immutable items using native-driver inserts and server-side owner filtering.</summary>
public sealed class MongoDbItemStore : IItemStore
{
    private readonly IMongoCollection<BsonDocument> collection;

    /// <summary>Uses a caller-owned client without initializing a database, collection, or index.</summary>
    public MongoDbItemStore(IMongoClient client, MongoDbOptions options)
    {
        ArgumentNullException.ThrowIfNull(client);
        ArgumentNullException.ThrowIfNull(options);
        options.Validate();
        collection = client.GetDatabase(options.DatabaseName).GetCollection<BsonDocument>(options.CollectionName)
            .WithWriteConcern(WriteConcern.WMajority);
    }

    /// <summary>Inserts once using a stable string identifier; duplicate and ambiguous write failures propagate.</summary>
    public Task AddAsync(Item item, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(item);
        cancellationToken.ThrowIfCancellationRequested();
        var document = new BsonDocument
        {
            { "_id", item.Id.ToString("D") },
            { "ownerId", item.OwnerId },
            { "name", item.Name }
        };
        return collection.InsertOneAsync(document, cancellationToken: cancellationToken);
    }

    /// <summary>Queries identifier and exact case-sensitive owner together, forwarding cancellation to the driver.</summary>
    public async Task<Item?> GetAsync(Guid id, string ownerId, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(ownerId);
        cancellationToken.ThrowIfCancellationRequested();
        var filter = Builders<BsonDocument>.Filter.Eq("_id", id.ToString("D")) &
            Builders<BsonDocument>.Filter.Eq("ownerId", ownerId);
        var options = new FindOptions<BsonDocument> { Collation = Collation.Simple, Limit = 1 };
        using var cursor = await collection.FindAsync(filter, options, cancellationToken);
        var document = await cursor.FirstOrDefaultAsync(cancellationToken);
        return document is null ? null : new Item(Guid.Parse(document["_id"].AsString),
            document["ownerId"].AsString, document["name"].AsString);
    }
}

/// <summary>Provides explicit opt-in MongoDB registration without resolving or contacting a server.</summary>
public static class MongoDbRegistration
{
    /// <summary>Registers the authoritative store; the host must independently register and own IMongoClient.</summary>
    public static IServiceCollection AddSampleMongoDb(this IServiceCollection services, MongoDbOptions options)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(options);
        options.Validate();
        if (services.Any(descriptor => descriptor.ServiceType == typeof(IItemStore)))
        {
            throw new InvalidOperationException("An authoritative item store is already selected.");
        }
        services.AddSingleton(options);
        services.AddSingleton<IItemStore, MongoDbItemStore>();
        return services;
    }
}
