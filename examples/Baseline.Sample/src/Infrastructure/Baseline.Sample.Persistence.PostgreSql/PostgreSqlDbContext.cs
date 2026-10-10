using Baseline.Sample.Persistence.EntityFramework.Context;
using Microsoft.EntityFrameworkCore;

namespace Baseline.Sample.Persistence.PostgreSql;

/// <summary>Owns PostgreSQL mappings and its independent migrations model.</summary>
public sealed class PostgreSqlDbContext(DbContextOptions<PostgreSqlDbContext> options) : ApplicationDbContext(options)
{
    /// <summary>Uses deterministic case-sensitive owner comparison rather than inherited locale rules.</summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PostgreSqlDbContext).Assembly);
    }
}
