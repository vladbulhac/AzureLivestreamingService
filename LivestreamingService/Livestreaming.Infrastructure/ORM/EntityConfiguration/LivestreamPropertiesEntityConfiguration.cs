using Livestreaming.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Livestreaming.Infrastructure.ORM.EntityConfiguration;

public class LivestreamPropertiesEntityConfiguration : IEntityTypeConfiguration<LivestreamProperties>
{
    public void Configure(EntityTypeBuilder<LivestreamProperties> builder)
    {
        builder.HasKey(sp => sp.LivestreamId);
        builder.Property(sp => sp.UserId)
               .IsRequired();
        builder.Property(sp => sp.Title)
               .HasMaxLength(100)
               .IsRequired();
        builder.Property(sp => sp.Description)
               .HasMaxLength(500)
               .IsRequired();
        builder.Property(sp => sp.RecordingDuration)
               .IsRequired();
        builder.Property(sp => sp.LiveStartDate)
               .IsRequired();
        builder.Property(sp => sp.LiveEventName)
               .HasMaxLength(32)
               .IsRequired();
        builder.Property(sp => sp.AssetName)
               .IsRequired();
        builder.Property(sp => sp.LiveOutputName)
               .IsRequired();
        builder.Property(sp => sp.DrvStreamingLocatorName)
               .IsRequired();
        builder.Property(sp => sp.DrvAssetFilterName)
               .IsRequired();
        builder.Property(sp => sp.ArchiveStreamingLocatorName)
               .IsRequired();
        builder.Property(sp => sp.StreamingLocatorName)
               .IsRequired();
        builder.Property(sp => sp.StreamingLocatorName)
               .HasMaxLength(24)
               .IsRequired();
        builder.Property(sp => sp.StreamingProtocol)
               .IsRequired();
        builder.Property(sp => sp.EncodingType)
               .IsRequired();

        builder.Property(sp => sp.Status)
               .HasConversion(p => p.ToString(),
                              p => (LivestreamStatus)Enum.Parse(typeof(LivestreamStatus), p))
               .IsRequired();

        builder.OwnsOne(lp => lp.RunningEndpoints, s =>
                {
                    s.Property(s => s.HlsManifest).HasColumnName("HlsManifest");
                    s.Property(s => s.DashManifest).HasColumnName("DashManifest");
                    s.Property(s => s.IngestUrl).HasColumnName("IngestUrl");
                    s.Property(s => s.PreviewUrl).HasColumnName("PreviewUrl");
                });
        builder.OwnsOne(lp => lp.PlaybackEndpoints, s =>
                {
                    s.Property(s => s.HlsManifest).HasColumnName("PlaybackHlsManifest");
                    s.Property(s => s.DashManifest).HasColumnName("PlaybackDashManifest");
                });
    }
}