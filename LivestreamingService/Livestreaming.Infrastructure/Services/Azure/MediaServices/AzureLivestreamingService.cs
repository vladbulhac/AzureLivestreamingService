using Livestreaming.Domain.Models;
using Livestreaming.Infrastructure.Services.Azure.Credentials;
using Livestreaming.Infrastructure.Services.Azure.Exceptions;
using Microsoft.Azure.Management.Media;
using Microsoft.Azure.Management.Media.Models;
using Microsoft.Extensions.Logging;

namespace Livestreaming.Infrastructure.Services.Azure.MediaServices;

public class AzureLivestreamingService : ILivestreamingService
{
    private readonly ILogger<AzureLivestreamingService> logger;
    private readonly IAzureMediaServicesClient streamingConfigurator;
    private readonly LivestreamProperties Properties;

    public LivestreamProperties GetProperties() => LivestreamProperties.From(Properties);

    private AzureLivestreamingService(LivestreamProperties properties, IAzureMediaServicesClient client, ILogger<AzureLivestreamingService> logger)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        streamingConfigurator = client ?? throw new ArgumentNullException(nameof(client));
        Properties = properties ?? throw new ArgumentNullException(nameof(properties));
    }

    /// <summary>
    /// Creates an instance of this class from existing <see cref="StreamProperties"/>.
    /// </summary>
    public static async Task<AzureLivestreamingService> FromAsync(LivestreamProperties properties, ILogger<AzureLivestreamingService> logger)
    {
        var credentials = await AzureMediaServicesAuthenticator.GenerateTokenCrendetialsAsync();
        var streamingClient = new AzureMediaServicesClient(AzureServicesCredentials.ARM.Endpoint, credentials) { SubscriptionId = AzureServicesCredentials.AMS.SubscriptionId };

        return new AzureLivestreamingService(properties, streamingClient, logger);
    }

    /// <summary>
    /// Allocates the following Azure Media Services resources, necessary for the livestream.
    ///<list type="bullet">
    /// <item>
    ///     <term><see cref="LiveEvent"/></term>
    ///     <description>
    ///         Receives the live video feed, from a remote encoder, through an ingest endpoint using either the RTMP or Smooth Streaming input protocol;
    ///         <see href="https://docs.microsoft.com/en-us/azure/media-services/latest/live-event-outputs-concept">Click here for more details.</see>
    ///     </description>
    /// </item>
    /// <item>
    ///     <term><see cref="LiveOutput"/></term>
    ///     <description>
    ///          Archives the stream and makes it available to viewers through the streaming endpoint.
    ///     </description>
    /// </item>
    /// <item>
    ///     <term><see cref="Asset"/></term>
    ///     <description>
    ///         It's used to input media (through upload or live ingest), output media (from a job output) and publish media (for streaming).
    ///         It contains information about the digital files (video,audio,images,thumbnails...) stored in Azure Storage (blob container).
    ///     </description>
    /// </item>
    ///</list>
    /// </summary>
    /// <exception cref="LivestreamSetupException"/>
    public async Task<LivestreamProperties> SetupAsync()
    {
        try
        {
            var mediaService = await streamingConfigurator.Mediaservices.GetAsync(AzureServicesCredentials.AMS.ResourceGroup, AzureServicesCredentials.AMS.AccountName);
            var liveEvent = AzureLiveEventFactory.Create(mediaService.Location,
                                                         Properties.Description,
                                                         Properties.StreamingProtocol,
                                                         Properties.EncodingType);
            await streamingConfigurator.LiveEvents.CreateAsync(AzureServicesCredentials.AMS.ResourceGroup,
                                                               AzureServicesCredentials.AMS.AccountName,
                                                               Properties.LiveEventName,
                                                               liveEvent,
                                                               autoStart: false);

            var asset = await streamingConfigurator.Assets.CreateOrUpdateAsync(AzureServicesCredentials.AMS.ResourceGroup,
                                                                               AzureServicesCredentials.AMS.AccountName,
                                                                               Properties.AssetName,
                                                                               new Asset());

            LiveOutput liveOutput = new(assetName: asset.Name,
                                        manifestName: "output",
                                        archiveWindowLength: TimeSpan.FromHours(Properties.RecordingDuration));
            await streamingConfigurator.LiveOutputs.CreateAsync(AzureServicesCredentials.AMS.ResourceGroup,
                                                                AzureServicesCredentials.AMS.AccountName,
                                                                Properties.LiveEventName,
                                                                Properties.LiveOutputName,
                                                                liveOutput);

            return Properties;
        }
        catch (ErrorResponseException ere)
        {
            logger.LogError(ere, $"[AzureStreamingService][Setup]: Encountered a {0} HTTP status code while creating a new live event, error details => {1}", ere.Response.StatusCode, ere.Message);
            throw new LivestreamSetupException(ere.Response.StatusCode,
                                               ere.Message,
                                               ere.InnerException);
        }
    }

    /// <summary>
    /// Starts the allocated resources to enable inputting the live stream.
    /// Creates additional resources:
    /// <list type="bullet">
    ///     <item>
    ///         <term><see cref="StreamingLocator"/></term>
    ///         <description>
    ///             Used for making the videos in the output <see cref="Asset"/> available to clients for playback.
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <term><see cref="StreamingEndpoint"/></term>
    ///         <description>
    ///             A dynamic (just-in-time) pacakaging, and origin service that delivers the live and on-demand content
    ///             directly to a client player app using one of the streaming media protocols HLS or DASH.
    ///         </description>
    ///     </item>
    /// </list>
    /// </summary>
    /// <returns></returns>
    public async Task<LivestreamEndpoints> StartAsync()
    {
        await streamingConfigurator.LiveEvents.StartAsync(AzureServicesCredentials.AMS.ResourceGroup,
                                                          AzureServicesCredentials.AMS.AccountName,
                                                          Properties.LiveEventName);

        AssetFilter drvAssetFilter = new(presentationTimeRange: new PresentationTimeRange(forceEndTimestamp: false,
                                                                                          presentationWindowDuration: 6000000000L, // represents a 10 minute (600s) sliding window
                                                                                          liveBackoffDuration: 20000000L // represents the latest live position that a client can seek back (2s)
                                                                                          ));
        await streamingConfigurator.AssetFilters.CreateOrUpdateAsync(AzureServicesCredentials.AMS.ResourceGroup,
                                                                     AzureServicesCredentials.AMS.AccountName,
                                                                     Properties.AssetName,
                                                                     Properties.DrvAssetFilterName,
                                                                     drvAssetFilter);

        StreamingLocator streamingLocator = null;
        try
        {
            streamingLocator = await streamingConfigurator.StreamingLocators.CreateAsync(AzureServicesCredentials.AMS.ResourceGroup,
                                                                                         AzureServicesCredentials.AMS.AccountName,
                                                                                         Properties.DrvStreamingLocatorName,
                                                                                         new StreamingLocator
                                                                                         {
                                                                                             AssetName = Properties.AssetName,
                                                                                             StreamingPolicyName = PredefinedStreamingPolicy.ClearStreamingOnly,
                                                                                             Filters = new List<string>() { Properties.DrvAssetFilterName }
                                                                                         });
        }
        catch (ErrorResponseException ere)
        {
            logger.LogError(ere, $"[AzureStreamingService][Start]: Encountered a {0} HTTP status code while creating a new streaming locator, error details => {1}", ere.Response.StatusCode, ere.Message);
            throw new LivestreamSetupException(ere.Response.StatusCode,
                                               ere.Message,
                                               ere.InnerException);
        }
        var streamingEndpoint = await streamingConfigurator.StreamingEndpoints.GetAsync(AzureServicesCredentials.AMS.ResourceGroup,
                                                                                        AzureServicesCredentials.AMS.AccountName,
                                                                                        Properties.StreamingEndpointName);

        if (streamingEndpoint.ResourceState != StreamingEndpointResourceState.Running)
        {
            await streamingConfigurator.StreamingEndpoints.StartAsync(AzureServicesCredentials.AMS.ResourceGroup,
                                                                      AzureServicesCredentials.AMS.AccountName,
                                                                      Properties.StreamingEndpointName);
        }

        var liveOutput = await streamingConfigurator.LiveOutputs.GetAsync(AzureServicesCredentials.AMS.ResourceGroup,
                                                                          AzureServicesCredentials.AMS.AccountName,
                                                                          Properties.LiveEventName,
                                                                          Properties.LiveOutputName);

        var manifests = BuildManifestPaths(streamingEndpoint.HostName, streamingLocator.StreamingLocatorId.ToString(), liveOutput.ManifestName);
        var liveEvent = await streamingConfigurator.LiveEvents.GetAsync(AzureServicesCredentials.AMS.ResourceGroup,
                                                                        AzureServicesCredentials.AMS.AccountName,
                                                                        Properties.LiveEventName);
        var accessEndpoints = new LivestreamEndpoints()
        {
            IngestUrl = liveEvent.Input.Endpoints.First().Url,
            PreviewUrl = $"https://ampdemo.azureedge.net/?url={liveEvent.Preview.Endpoints.First().Url}&heuristicprofile=lowlatency",
            HlsManifest = manifests[0],
            DashManifest = $"https://ampdemo.azureedge.net/?url={manifests[1]}&heuristicprofile=lowlatency"
        };
        Properties.LivestreamIsRunning(accessEndpoints);

        return accessEndpoints;
    }

    /// <summary>
    /// Deletes the allocated <see cref="LiveEvent"/>,<see cref="LiveOutput"/>,<see cref="Asset"/>,<see cref="StreamingLocator"/> resources.
    /// </summary>
    /// <returns></returns>
    public async Task<LivestreamPlaybackEndpoints> StopAsync()
    {
        LivestreamPlaybackEndpoints archivedLivestreamAccess = new();

        if (Properties.Status is LivestreamStatus.Saved) return archivedLivestreamAccess;

        try
        {
            var streamingEndpoint = await streamingConfigurator.StreamingEndpoints.GetAsync(AzureServicesCredentials.AMS.ResourceGroup,
                                                                                            AzureServicesCredentials.AMS.AccountName,
                                                                                            Properties.StreamingEndpointName);
            var liveOutput = await streamingConfigurator.LiveOutputs.GetAsync(AzureServicesCredentials.AMS.ResourceGroup,
                                                                              AzureServicesCredentials.AMS.AccountName,
                                                                              Properties.LiveEventName,
                                                                              Properties.LiveOutputName);

            var archiveLocator = await streamingConfigurator.StreamingLocators.CreateAsync(AzureServicesCredentials.AMS.ResourceGroup,
                                                                                           AzureServicesCredentials.AMS.AccountName,
                                                                                           Properties.ArchiveStreamingLocatorName,
                                                                                           new StreamingLocator
                                                                                           {
                                                                                               AssetName = Properties.AssetName,
                                                                                               StreamingPolicyName = PredefinedStreamingPolicy.ClearStreamingOnly
                                                                                           });

            var manifests = BuildManifestPaths(streamingEndpoint.HostName, archiveLocator.StreamingLocatorId.ToString(), liveOutput.ManifestName);
            archivedLivestreamAccess = new()
            {
                HlsManifest = manifests[0],
                DashManifest = $"https://ampdemo.azureedge.net/?url={manifests[1]}&heuristicprofile=lowlatency",
            };
        }
        catch (ErrorResponseException ere)
        {
            logger.LogError(ere, $"[AzureStreamingService][Stop]: Encountered a {0} HTTP status code while trying to stop the livestream, error details => {1}", ere.Response.StatusCode, ere.Message);
            throw new LivestreamCleanupException(ere.Response.StatusCode,
                                                 ere.Message,
                                                 ere.InnerException);
        }
        finally
        {
            await CleanupLiveEventAndOutputAsync();

            //the following calls remove the asset + archived livestream
            //await CleanupLocatorAndAssetAsync();
            /*await streamingConfigurator.StreamingEndpoints.StopAsync(AzureServicesCredentials.AMS.ResourceGroup,
                                                               AzureServicesCredentials.AMS.AccountName,
                                                               Properties.StreamingEndpointName);*/
        }

        Properties.LivestreamStopped(archivedLivestreamAccess);
        return archivedLivestreamAccess;
    }

    private static List<string> BuildManifestPaths(string hostname, string streamingLocatorId, string manifestName, string scheme = "https")
    {
        const string hlsFormat = "format=m3u8-cmaf";
        const string dashFormat = "format=mpd-time-cmaf";

        List<string> manifests = new();

        var manifestBase = $"{scheme}://{hostname}/{streamingLocatorId}/{manifestName}.ism/manifest";
        var hlsManifest = $"{manifestBase}({hlsFormat})";
        manifests.Add(hlsManifest);

        var dashManifest = $"{manifestBase}({dashFormat})";
        manifests.Add(dashManifest);

        return manifests;
    }

    /// <remarks>
    /// The <see cref="LiveOutput"/> must be deleted first and then the <see cref="LiveEvent"/>.
    /// </remarks>
    private async Task CleanupLiveEventAndOutputAsync()
    {
        try
        {
            var liveEvent = await streamingConfigurator.LiveEvents.GetAsync(AzureServicesCredentials.AMS.ResourceGroup,
                                                                            AzureServicesCredentials.AMS.AccountName,
                                                                            Properties.LiveEventName);

            await streamingConfigurator.LiveOutputs.DeleteAsync(AzureServicesCredentials.AMS.ResourceGroup,
                                                                AzureServicesCredentials.AMS.AccountName,
                                                                Properties.LiveEventName,
                                                                Properties.LiveOutputName);
            if (liveEvent is not null)
            {
                if (liveEvent.ResourceState == LiveEventResourceState.Running)
                {
                    await streamingConfigurator.LiveEvents.StopAsync(AzureServicesCredentials.AMS.ResourceGroup,
                                                                     AzureServicesCredentials.AMS.AccountName,
                                                                     Properties.LiveEventName,
                                                                     removeOutputsOnStop: false);
                }

                await streamingConfigurator.LiveEvents.DeleteAsync(AzureServicesCredentials.AMS.ResourceGroup,
                                                                   AzureServicesCredentials.AMS.AccountName,
                                                                   Properties.LiveEventName);
            }
        }
        catch (ErrorResponseException ere)
        {
            logger.LogError(ere, $"[AzureStreamingService][Stop]: Encountered a {0} HTTP status code while trying to stop the livestream, error details => {1}", ere.Response.StatusCode, ere.Message);
            throw new LivestreamCleanupException(ere.Response.StatusCode,
                                                 ere.Message,
                                                 ere.InnerException);
        }
    }

    private async Task CleanupLocatorAndAssetAsync()
    {
        try
        {
            await streamingConfigurator.StreamingLocators.DeleteAsync(AzureServicesCredentials.AMS.ResourceGroup,
                                                                      AzureServicesCredentials.AMS.AccountName,
                                                                      Properties.StreamingLocatorName);
            await streamingConfigurator.Assets.DeleteAsync(AzureServicesCredentials.AMS.ResourceGroup,
                                                           AzureServicesCredentials.AMS.AccountName,
                                                           Properties.AssetName);
        }
        catch (ErrorResponseException ere)
        {
            logger.LogError(ere, $"[AzureStreamingService][Stop]: Encountered a {0} HTTP status code while trying to stop the livestream, error details => {1}", ere.Response.StatusCode, ere.Message);
            throw new LivestreamCleanupException(ere.Response.StatusCode,
                                                 ere.Message,
                                                 ere.InnerException);
        }
    }

    public Task DeleteAsync()
    {
        throw new NotImplementedException();
    }
}