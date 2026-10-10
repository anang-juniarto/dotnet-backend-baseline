using Baseline.Sample.Persistence.EntityFramework.Context;
using Microsoft.EntityFrameworkCore;

namespace Baseline.Sample.Persistence.SqlServer;

/// <summary>Owns SQL Server mappings and its independent migrations model.</summary>
public sealed class SqlServerDbContext(DbContextOptions<SqlServerDbContext> options) : ApplicationDbContext(options)
{
    /// <summary>Uses binary owner comparison independently of the database default collation.</summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SqlServerDbContext).Assembly);
    }
}
