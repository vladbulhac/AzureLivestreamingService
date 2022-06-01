using Livestreaming.Domain.Models;
using Livestreaming.Infrastructure.ORM.EntityConfiguration;
using Microsoft.EntityFrameworkCore;

namespace Livestreaming.Infrastructure.ORM;

public class LivestreamContext : DbContext
{
    public DbSet<LivestreamProperties> Livestreams { get; set; }

    public LivestreamContext(DbContextOptions<LivestreamContext> options) : base(options)
    {
        if (Database.ProviderName != "Microsoft.EntityFrameworkCore.InMemory")
            Database.Migrate();
    }

    public LivestreamContext()
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new LivestreamPropertiesEntityConfiguration());
    }
}