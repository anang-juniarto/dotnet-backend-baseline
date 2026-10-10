using Baseline.Sample.Domain.Items;
using Baseline.Sample.Persistence.PostgreSql;
using Baseline.Sample.Persistence.SqlServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Xunit;

namespace Baseline.Sample.IntegrationTests;

/// <summary>Validates provider models without opening a database connection.</summary>
public sealed class EntityMappingTests
{
    /// <summary>The SQL Server model maps the sole Domain entity and retains shared rules and binary ownership.</summary>
    [Fact]
    public void SqlServerMapsDomainEntity()
    {
        using var context = new SqlServerDbContext(new DbContextOptionsBuilder<SqlServerDbContext>()
            .UseSqlServer("Server=localhost;Database=MappingOnly;Integrated Security=true;TrustServerCertificate=true").Options);
        AssertMapping(context, "Latin1_General_100_BIN2");
    }

    /// <summary>The PostgreSQL model maps the same Domain entity and preserves provider-specific ownership comparison.</summary>
    [Fact]
    public void PostgreSqlMapsDomainEntity()
    {
        using var context = new PostgreSqlDbContext(new DbContextOptionsBuilder<PostgreSqlDbContext>()
            .UseNpgsql("Host=localhost;Database=MappingOnly;Username=unused;Password=unused").Options);
        AssertMapping(context, "C");
    }

    private static void AssertMapping(DbContext context, string collation)
    {
        var model = context.GetService<IDesignTimeModel>().Model;
        var entity = Assert.Single(model.GetEntityTypes());
        Assert.Equal(typeof(Item), entity.ClrType);
        Assert.Equal("Items", entity.GetTableName());
        Assert.Equal(nameof(Item.Id), Assert.Single(entity.FindPrimaryKey()!.Properties).Name);
        Assert.Equal(ValueGenerated.Never, entity.FindProperty(nameof(Item.Id))!.ValueGenerated);
        Assert.False(entity.FindProperty(nameof(Item.Name))!.IsNullable);
        Assert.Equal(100, entity.FindProperty(nameof(Item.Name))!.GetMaxLength());
        Assert.False(entity.FindProperty(nameof(Item.OwnerId))!.IsNullable);
        Assert.Equal(collation, entity.FindProperty(nameof(Item.OwnerId))!.GetCollation());
        // Forces constructor binding/materialization pipeline compilation without executing SQL.
        Assert.Contains("Items", context.Set<Item>().Where(item => item.OwnerId == "alice").ToQueryString());
    }
}
