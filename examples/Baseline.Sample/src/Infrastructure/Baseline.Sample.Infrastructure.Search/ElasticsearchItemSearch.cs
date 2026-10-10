using Baseline.Sample.Domain.Items;
using Elastic.Clients.Elasticsearch;
using Microsoft.Extensions.DependencyInjection;

namespace Baseline.Sample.Infrastructure.Search;

/// <summary>Bounds operations against an externally provisioned Elasticsearch alias and mapping.</summary>
public sealed record ElasticsearchOptions
{
    /// <summary>Gets a single write/search alias; provisioning must map ownerId.keyword as exact, unnormalized keyword.</summary>
    public required string IndexAlias { get; init; }

    /// <summary>Gets the maximum number of results returned per search.</summary>
    public int MaximumResults { get; init; } = 50;

    internal void Validate()
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(IndexAlias);
        if (IndexAlias.Length > 200 || !char.IsAsciiLetterLower(IndexAlias[0]) ||
            IndexAlias.Any(character => !char.IsAsciiLetterLower(character) && !char.IsAsciiDigit(character) && character is not '-' and not '_') ||
            MaximumResults is < 1 or > 100)
        {
            throw new ArgumentException("A bounded result count and single lowercase index alias are required.");
        }
    }
}

/// <summary>Represents a non-authoritative search projection; the item store remains the source of truth.</summary>
public sealed record SearchItemDocument
{
    /// <summary>Gets the source item identifier.</summary>
    public required Guid Id { get; init; }

    /// <summary>Gets the case-sensitive owner used for query filtering and defensive result verification.</summary>
    public required string OwnerId { get; init; }

    /// <summary>Gets the item display text.</summary>
    public required string Name { get; init; }
}

/// <summary>Exposes explicit indexing and bounded owner-scoped search, separate from persistence.</summary>
public interface IItemSearch
{
    /// <summary>Updates a projection through a pre-existing write alias, without creating an index.</summary>
    Task IndexAsync(Item item, CancellationToken cancellationToken);

    /// <summary>Searches plain display text for one owner; no raw query syntax is accepted.</summary>
    Task<IReadOnlyList<SearchItemDocument>> SearchAsync(string ownerId, string text, int limit, CancellationToken cancellationToken);
}

/// <summary>Uses a host-owned Elasticsearch client without initialization or implicit provisioning.</summary>
public sealed class ElasticsearchItemSearch : IItemSearch
{
    private readonly ElasticsearchClient client;
    private readonly ElasticsearchOptions options;

    /// <summary>Accepts an existing client and validates adapter-local limits without making requests.</summary>
    public ElasticsearchItemSearch(ElasticsearchClient client, ElasticsearchOptions options)
    {
        ArgumentNullException.ThrowIfNull(client);
        ArgumentNullException.ThrowIfNull(options);
        options.Validate();
        this.client = client;
        this.options = options;
    }

    /// <summary>Indexes a stable identifier, requiring the alias to exist; unsuccessful responses throw.</summary>
    public async Task IndexAsync(Item item, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(item);
        cancellationToken.ThrowIfCancellationRequested();
        var document = new SearchItemDocument { Id = item.Id, OwnerId = item.OwnerId, Name = item.Name };
        var response = await client.IndexAsync(document, descriptor => descriptor
            .Index(options.IndexAlias).Id(item.Id.ToString("D")).RequireAlias(true), cancellationToken);
        if (!response.IsValidResponse)
        {
            throw new InvalidOperationException("Elasticsearch did not acknowledge indexing the item projection.");
        }
    }

    /// <summary>Combines an exact owner filter with a match query and verifies every returned owner.</summary>
    public async Task<IReadOnlyList<SearchItemDocument>> SearchAsync(string ownerId, string text, int limit, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(ownerId);
        ArgumentException.ThrowIfNullOrWhiteSpace(text);
        if (ownerId.Length > 512 || text.Length > 200 || limit < 1 || limit > options.MaximumResults)
        {
            throw new ArgumentOutOfRangeException(nameof(limit), "Search input or result bounds were exceeded.");
        }
        cancellationToken.ThrowIfCancellationRequested();
        var response = await client.SearchAsync<SearchItemDocument>(descriptor => descriptor
            .Indices(options.IndexAlias).Size(limit).AllowPartialSearchResults(false)
            .Query(query => query.Bool(boolean => boolean
                .Filter(filter => filter.Term(term => term.Field("ownerId.keyword").Value(ownerId)))
                .Must(must => must.Match(match => match.Field("name").Query(text))))), cancellationToken);
        if (!response.IsValidResponse || response.TimedOut || response.Shards.Failed > 0)
        {
            throw new InvalidOperationException("Elasticsearch did not return a complete successful search response.");
        }
        var documents = response.Documents.ToArray();
        if (documents.Any(document => !string.Equals(document.OwnerId, ownerId, StringComparison.Ordinal)))
        {
            throw new InvalidOperationException("Search mapping did not enforce exact owner isolation.");
        }
        return documents;
    }
}

/// <summary>Provides explicit opt-in registration with no client construction or server requests.</summary>
public static class ElasticsearchRegistration
{
    /// <summary>Validates local settings; the host must separately register and own ElasticsearchClient.</summary>
    public static IServiceCollection AddSampleElasticsearch(this IServiceCollection services, ElasticsearchOptions options)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(options);
        options.Validate();
        services.AddSingleton(options);
        services.AddSingleton<IItemSearch, ElasticsearchItemSearch>();
        return services;
    }
}
