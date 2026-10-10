using Microsoft.EntityFrameworkCore;

namespace Baseline.Sample.Persistence.EntityFramework.Context;

/// <summary>Registers shared mappings while concrete provider contexts own provider configuration.</summary>
public abstract class ApplicationDbContext : DbContext
{
    /// <summary>Initializes the context with explicitly selected provider options.</summary>
    protected ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    /// <summary>Applies shared entity configuration before provider-specific overrides.</summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}
