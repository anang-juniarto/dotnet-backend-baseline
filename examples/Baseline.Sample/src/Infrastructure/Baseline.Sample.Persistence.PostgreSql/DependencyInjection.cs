using Baseline.Sample.Application.Items;
using Baseline.Sample.Persistence.EntityFramework.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Baseline.Sample.Persistence.PostgreSql;

/// <summary>Registers the explicitly selected PostgreSQL item store.</summary>
public static class DependencyInjection
{
    /// <summary>Replaces the item store with a scoped PostgreSQL adapter without connecting or migrating.</summary>
    public static IServiceCollection AddPostgreSqlItemStore(this IServiceCollection services, string connectionString)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentException.ThrowIfNullOrWhiteSpace(connectionString);
        services.AddDbContext<PostgreSqlDbContext>(options => options.UseNpgsql(connectionString,
            provider => provider.MigrationsAssembly(typeof(PostgreSqlDbContext).Assembly.GetName().Name)));
        services.RemoveAll<IItemStore>();
        services.AddScoped<IItemStore, EntityFrameworkItemStore<PostgreSqlDbContext>>();
        return services;
    }
}
