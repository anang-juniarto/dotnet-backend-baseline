using Baseline.Sample.Application.Items;
using Baseline.Sample.Persistence.EntityFramework.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Baseline.Sample.Persistence.SqlServer;

/// <summary>Registers the explicitly selected SQL Server item store.</summary>
public static class DependencyInjection
{
    /// <summary>Replaces the item store with a scoped SQL Server adapter without connecting or migrating.</summary>
    public static IServiceCollection AddSqlServerItemStore(this IServiceCollection services, string connectionString)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentException.ThrowIfNullOrWhiteSpace(connectionString);
        services.AddDbContext<SqlServerDbContext>(options => options.UseSqlServer(connectionString,
            provider => provider.MigrationsAssembly(typeof(SqlServerDbContext).Assembly.GetName().Name)));
        services.RemoveAll<IItemStore>();
        services.AddScoped<IItemStore, EntityFrameworkItemStore<SqlServerDbContext>>();
        return services;
    }
}
