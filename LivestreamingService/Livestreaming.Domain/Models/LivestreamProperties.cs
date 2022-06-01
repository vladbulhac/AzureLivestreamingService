using Microsoft.Azure.Management.Media.Models;

namespace Livestreaming.Domain.Models;

public class LivestreamProperties
{
    public string UserId { get; init; }
    public string LivestreamId { get; init; }
    public LivestreamStatus Status { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public DateTime LiveStartDate { get; private set; }

    /// <summary>
    /// Duration of the stream's copy that will be saved.
    /// Content that falls outside of ArchiveWindowLength is continuously discarded from storage and is non-recoverable.
    /// </summary>
    /// <remarks>
    /// A value in hour(s), between 1(hour,inclusive) and 25(hours,inclusive).
    /// </remarks>
    public int RecordingDuration { get; private set; }

    /// <remarks>
    /// Max length is 32 characters and must follow the regex pattern: ^[a-zA-Z0-9]+(-*[a-zA-Z0-9])*$
    /// </remarks>
    public string LiveEventName { get; private set; }

    public string AssetName { get; init; }
    public string LiveOutputName { get; init; }
    public string DrvStreamingLocatorName { get; init; }
    public string DrvAssetFilterName { get; init; }
    public string ArchiveStreamingLocatorName { get; init; }

    public string StreamingLocatorName { get; init; }

    /// <remarks>
    /// Max length is 24 characters and must follow the regex pattern: ^[a-zA-Z0-9]+(-*[a-zA-Z0-9])*$
    /// </remarks>
    public string StreamingEndpointName { get; init; }

    /// <summary>
    /// The input protocol for the <see cref="LiveEvent">LiveEvent's</see> ingest endpoint.
    /// </summary>
    public string StreamingProtocol { get; private set; }

    /// <summary>
    /// A live event can be set to either a basic or standard pass-through (an on-premises live encoder sends a multiple bitrate stream)
    /// or live encoding (an on-premises live encoder sends a single bitrate stream).
    /// </summary>
    public string EncodingType { get; private set; }

    public LivestreamEndpoints RunningEndpoints { get; private set; }
    public LivestreamPlaybackEndpoints PlaybackEndpoints { get; private set; }

    public LivestreamProperties()
    { }

    public static LivestreamProperties From(LivestreamProperties properties)
    {
        return new(properties.Title,
                   properties.StreamingProtocol,
                   properties.EncodingType,
                   properties.LiveStartDate,
                   properties.UserId,
                   properties.LivestreamId,
                   properties.Description,
                   properties.AssetName,
                   properties.LiveEventName,
                   properties.LiveOutputName,
                   properties.DrvStreamingLocatorName,
                   properties.ArchiveStreamingLocatorName,
                   properties.DrvAssetFilterName,
                   properties.StreamingLocatorName,
                   properties.StreamingEndpointName,
                   properties.RecordingDuration,
                   properties.Status,
                   properties.RunningEndpoints,
                   properties.PlaybackEndpoints);
    }

    private LivestreamProperties(string title,
                                   LiveEventInputProtocol streamingProtocol,
                                   LiveEventEncodingType encodingType,
                                   DateTime liveStartDate,
                                   string userId,
                                   string livestreamId,
                                   string description,
                                   string assetName,
                                   string liveEventName,
                                   string liveOutputName,
                                   string drvStreamingLocatorName,
                                   string archiveStreamingLocatorName,
                                   string drvAssetFilterName,
                                   string streamingLocatorName,
                                   string streamingEndpointName,
                                   int recordingDuration,
                                   LivestreamStatus status,
                                   LivestreamEndpoints livestreamEndpoints,
                                   LivestreamPlaybackEndpoints playbackEndpoints)
    {
        UserId = userId;
        LivestreamId = livestreamId;

        Status = status;
        Title = title;
        LiveStartDate = liveStartDate;
        RecordingDuration = recordingDuration;
        LiveEventName = liveEventName;
        Description = string.IsNullOrEmpty(description) ? "No description" : description;
        AssetName = string.IsNullOrEmpty(assetName) ? "archiveAsset" : assetName;
        LiveOutputName = liveOutputName;
        DrvStreamingLocatorName = drvStreamingLocatorName;
        ArchiveStreamingLocatorName = archiveStreamingLocatorName;
        DrvAssetFilterName = drvAssetFilterName;
        StreamingLocatorName = streamingLocatorName;
        StreamingEndpointName = streamingEndpointName;

        RunningEndpoints = livestreamEndpoints;
        PlaybackEndpoints = playbackEndpoints;

        StreamingProtocol = streamingProtocol;
        EncodingType = encodingType;
    }

    /// <param name="description">Livestream's description</param>
    /// <param name="recordingDuration"><inheritdoc cref="RecordingDuration" path="/node()"/></param>
    /// <param name="streamingProtocol"><inheritdoc cref="StreamingProtocol" path="/node()"/></param>
    /// <param name="encodingType"><inheritdoc cref="EncodingType" path="/node()"/></param>

    public LivestreamProperties(string title,
                                LiveEventInputProtocol streamingProtocol,
                                LiveEventEncodingType encodingType,
                                string userId = "",
                                string livestreamId = "",
                                string description = "No description",
                                string assetName = "archiveAsset",
                                string liveOutputName = "output",
                                string drvStreamingLocatorName = "DRVStreamingLocator",
                                string archiveStreamingLocatorName = "fullLocator",
                                string drvAssetFilterName = "filter",
                                string streamingLocatorName = "streamingLocator",
                                string streamingEndpointName = "default",
                                int recordingDuration = 25,
                                LivestreamStatus status = LivestreamStatus.Setup)
    {
        UserId = userId;
        LivestreamId = string.IsNullOrEmpty(livestreamId) ? Guid.NewGuid().ToString() : livestreamId;
        string uniqueness = LivestreamId.Substring(9, 4);

        Status = status;
        Title = title;
        LiveStartDate = DateTime.UtcNow;
        RecordingDuration = recordingDuration;
        LiveEventName = LivestreamId.Substring(0, 32);
        Description = string.IsNullOrEmpty(description) ? "No description" : description;
        AssetName = "archiveAsset-" + uniqueness;
        LiveOutputName = "output-" + uniqueness;
        DrvStreamingLocatorName = "DRVStreamingLocator-" + uniqueness;
        ArchiveStreamingLocatorName = "fullLocator-" + uniqueness;
        DrvAssetFilterName = "filter-" + uniqueness;
        StreamingLocatorName = "streamingLocator" + uniqueness;
        StreamingEndpointName = streamingEndpointName;

        StreamingProtocol = streamingProtocol;
        EncodingType = encodingType;

        RunningEndpoints = new();
        PlaybackEndpoints = new();
    }

    public void LivestreamIsRunning(LivestreamEndpoints accessEndpoints)
    {
        Status = LivestreamStatus.Live;
        LiveStartDate = DateTime.UtcNow;
        RunningEndpoints = accessEndpoints;
    }

    public void LivestreamStopped(LivestreamPlaybackEndpoints playbackEndpoints)
    {
        Status = LivestreamStatus.Saved;
        PlaybackEndpoints = playbackEndpoints;
    }
}