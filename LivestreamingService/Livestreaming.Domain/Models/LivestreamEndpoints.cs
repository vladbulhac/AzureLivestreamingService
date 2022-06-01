namespace Livestreaming.Domain.Models;
public record LivestreamEndpoints
{
    /// <summary>
    /// Input for an on-premises recording software like OBS.
    /// </summary>
    /// <remarks>
    /// Can take one of the following forms:
    /// <para>With non-static hostname</para>
    /// <list type="bullet">
    ///     <item>
    ///         <term>RTMP</term>
    ///         <description>
    ///             rtmp://&lt;random 128bit hex string&gt;.channel.media.azure.net:1935/live/&lt;auto-generated access token&gt;/&lt;stream name&gt;
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <term>RTMPS</term>
    ///         <description>
    ///             rtmps://&lt;random 128bit hex string&gt;.channel.media.azure.net:2935/live/&lt;auto-generated access token&gt;/&lt;stream name&gt;
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <term>Smooth streaming HTTP</term>
    ///         <description>
    ///             http://&lt;random 128bit hex string&gt;.channel.media.azure.net/&lt;auto-generated access token&gt;/ingest.isml/streams(&lt;stream name&gt;)
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <term>Smooth streaming HTTPS</term>
    ///         <description>
    ///             https://&lt;random 128bit hex string&gt;.channel.media.azure.net/&lt;auto-generated access token&gt;/ingest.isml/streams(&lt;stream name&gt;)
    ///         </description>
    ///     </item>
    /// </list>
    ///
    /// <para>With static hostname</para>
    /// <list type="bullet">
    ///     <item>
    ///         <term>RTMP</term>
    ///         <description>
    ///             rtmp://&lt;live event name&gt;-&lt;ams account name&gt;-&lt;region abbrev name&gt;.channel.media.azure.net:1935/live/&lt;your access token&gt;/&lt;stream name&gt;
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <term>RTMPS</term>
    ///         <description>
    ///             rtmps://&lt;live event name&gt;-&lt;ams account name&gt;-&lt;region abbrev name&gt;.channel.media.azure.net:1936/live/&lt;your access token&gt;/&lt;stream name&gt;
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <term>Smooth streaming HTTP</term>
    ///         <description>
    ///             http://&lt;live event name&gt;-&lt;ams account name&gt;-&lt;region abbrev name&gt;.channel.media.azure.net/&lt;your access token&gt;/ingest.isml/streams(&lt;stream name&gt;)
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <term>Smooth streaming HTTPS</term>
    ///         <description>
    ///             https://&lt;live event name&gt;-&lt;ams account name&gt;-&lt;region abbrev name&gt;.channel.media.azure.net/&lt;your access token&gt;/ingest.isml/streams(&lt;stream name&gt;)
    ///         </description>
    ///     </item>
    /// </list>
    /// </remarks>
    public string IngestUrl { get; init; }

    /// <summary>
    /// Can be used, once the <see cref="LiveEvent"/> starts receiving the contribution feed, to preview and validate that the live stream can be received.
    /// </summary>
    public string PreviewUrl { get; init; }
    public string HlsManifest { get; init; }
    public string DashManifest { get; init; }

    public LivestreamEndpoints()
    {
        IngestUrl = "";
        PreviewUrl = "";
        HlsManifest = "";
        DashManifest = "";
    }
}