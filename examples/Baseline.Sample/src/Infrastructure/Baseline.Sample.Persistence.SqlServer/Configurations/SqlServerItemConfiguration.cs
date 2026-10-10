using Baseline.Sample.Domain.Items;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Baseline.Sample.Persistence.SqlServer.Configurations;

/// <summary>Overrides owner comparison with SQL Server's explicit binary collation.</summary>
public sealed class SqlServerItemConfiguration : IEntityTypeConfiguration<Item>
{
    /// <summary>Preserves case-sensitive ownership independently of the database default.</summary>
    public void Configure(EntityTypeBuilder<Item> builder)
    {
        builder.Property(item => item.OwnerId).UseCollation("Latin1_General_100_BIN2");
    }
}
