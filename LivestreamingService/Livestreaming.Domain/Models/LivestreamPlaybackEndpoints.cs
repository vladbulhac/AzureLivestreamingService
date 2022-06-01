namespace Livestreaming.Domain.Models;

/// <summary>
/// Acccess URLs for the archived livestream.
/// </summary>
public record LivestreamPlaybackEndpoints
{
    public string HlsManifest { get; init; }
    public string DashManifest { get; init; }

    public LivestreamPlaybackEndpoints()
    {
        HlsManifest = "";
        DashManifest = "";
    }
}