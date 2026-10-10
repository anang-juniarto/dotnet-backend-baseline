using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Baseline.Sample.Infrastructure.Caching;

/// <summary>Bounds an optional, non-authoritative Redis byte cache.</summary>
public sealed record RedisCacheOptions
{
    /// <summary>Gets the dedicated application key prefix.</summary>
    public required string KeyPrefix { get; init; }

    /// <summary>Gets the maximum cached value size in bytes.</summary>
    public int MaximumValueBytes { get; init; } = 65536;

    /// <summary>Gets the maximum relative lifetime permitted for every write.</summary>
    public TimeSpan MaximumLifetime { get; init; } = TimeSpan.FromMinutes(15);

    /// <summary>Gets the local wait bound; cancellation does not retract a Redis command already submitted.</summary>
    public TimeSpan OperationTimeout { get; init; } = TimeSpan.FromSeconds(5);

    /// <summary>Gets the explicitly selected Redis database number.</summary>
    public int Database { get; init; }

    internal void Validate()
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(KeyPrefix);
        if (KeyPrefix.Length > 100 || MaximumValueBytes is < 1 or > 1048576 || Database < 0 ||
            MaximumLifetime <= TimeSpan.Zero || MaximumLifetime > TimeSpan.FromDays(1) ||
            OperationTimeout <= TimeSpan.Zero || OperationTimeout > TimeSpan.FromSeconds(30))
        {
            throw new ArgumentOutOfRangeException(nameof(RedisCacheOptions), "Redis cache bounds are invalid.");
        }
    }
}

/// <summary>Exposes only expiring cache operations, never authoritative item persistence.</summary>
public interface IByteCache
{
    /// <summary>Returns cached bytes or null on a real miss; operational failures propagate.</summary>
    Task<byte[]?> GetAsync(string key, CancellationToken cancellationToken);

    /// <summary>Writes a bounded value with a required bounded lifetime.</summary>
    Task SetAsync(string key, byte[] value, TimeSpan lifetime, CancellationToken cancellationToken);

    /// <summary>Removes a cache entry if present.</summary>
    Task RemoveAsync(string key, CancellationToken cancellationToken);
}

/// <summary>Adapts a host-owned Redis connection to a size- and time-bounded byte cache.</summary>
public sealed class RedisByteCache : IByteCache
{
    private readonly IDatabase database;
    private readonly RedisCacheOptions options;

    /// <summary>Uses an existing connection without opening a connection or changing server configuration.</summary>
    public RedisByteCache(IConnectionMultiplexer connection, RedisCacheOptions options)
    {
        ArgumentNullException.ThrowIfNull(connection);
        ArgumentNullException.ThrowIfNull(options);
        options.Validate();
        this.options = options;
        database = connection.GetDatabase(options.Database);
    }

    /// <summary>Fetches at most the configured bound plus one byte, detecting oversized external entries.</summary>
    public async Task<byte[]?> GetAsync(string key, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var redisKey = GetKey(key);
        var value = await database.StringGetRangeAsync(redisKey, 0, options.MaximumValueBytes)
            .WaitAsync(options.OperationTimeout, cancellationToken);
        if (value.IsNullOrEmpty)
        {
            return null;
        }
        var bytes = (byte[])value!;
        if (bytes.Length > options.MaximumValueBytes)
        {
            throw new InvalidOperationException("Cached value exceeds the configured byte limit.");
        }
        return bytes;
    }

    /// <summary>Stores nonempty bytes with an explicit expiry; failures are not reported as success.</summary>
    public async Task SetAsync(string key, byte[] value, TimeSpan lifetime, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(value);
        if (value.Length < 1 || value.Length > options.MaximumValueBytes)
        {
            throw new ArgumentOutOfRangeException(nameof(value));
        }
        if (lifetime <= TimeSpan.Zero || lifetime > options.MaximumLifetime)
        {
            throw new ArgumentOutOfRangeException(nameof(lifetime));
        }
        cancellationToken.ThrowIfCancellationRequested();
        var stored = await database.StringSetAsync(GetKey(key), value, lifetime)
            .WaitAsync(options.OperationTimeout, cancellationToken);
        if (!stored)
        {
            throw new InvalidOperationException("Redis did not acknowledge the cache write.");
        }
    }

    /// <summary>Deletes only the named cache entry; cancellation bounds waiting rather than server execution.</summary>
    public async Task RemoveAsync(string key, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        await database.KeyDeleteAsync(GetKey(key)).WaitAsync(options.OperationTimeout, cancellationToken);
    }

    private RedisKey GetKey(string key)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(key);
        if (key.Length > 512)
        {
            throw new ArgumentOutOfRangeException(nameof(key));
        }
        return options.KeyPrefix + ":" + Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(key)));
    }
}

/// <summary>Registers cache capability only when explicitly invoked by the composition root.</summary>
public static class RedisCacheRegistration
{
    /// <summary>Validates local options; a caller-owned IConnectionMultiplexer must be registered separately.</summary>
    public static IServiceCollection AddSampleRedisCache(this IServiceCollection services, RedisCacheOptions options)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(options);
        options.Validate();
        services.AddSingleton(options);
        services.AddSingleton<IByteCache, RedisByteCache>();
        return services;
    }
}
