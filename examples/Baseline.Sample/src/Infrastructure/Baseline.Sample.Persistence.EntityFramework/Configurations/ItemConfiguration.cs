using Baseline.Sample.Domain.Items;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Baseline.Sample.Persistence.EntityFramework.Configurations;

/// <summary>Defines the provider-independent Items storage contract.</summary>
public sealed class ItemConfiguration : IEntityTypeConfiguration<Item>
{
    /// <summary>Maps application-assigned identity and required normalized values.</summary>
    public void Configure(EntityTypeBuilder<Item> builder)
    {
        builder.ToTable("Items");
        builder.HasKey(item => item.Id);
        builder.Property(item => item.Id).ValueGeneratedNever();
        builder.Property(item => item.OwnerId).IsRequired();
        builder.Property(item => item.Name).HasMaxLength(100).IsRequired();
    }
}
