using Microsoft.Azure.Management.Media.Models;

namespace Livestreaming.Infrastructure.Services.Azure.MediaServices;

public static class AzureLiveEventFactory
{
    /// <summary>
    /// Creates a <see cref="LiveEvent"/> with IP restrictions on the ingest and preview.
    /// </summary>
    /// <param name="location">Geo-location where to allocate this <see cref="LiveEvent"/>.</param>
    /// <param name="description">Livestream's description.</param>
    /// <param name="streamingProtocol">The input protocol for the LiveEvent's ingest endpoint.</param>
    /// <param name="encodingType">A live event can be set to either a basic or standard pass-through (an on-premises live encoder sends a multiple bitrate stream)
    ///                            or live encoding (an on-premises live encoder sends a single bitrate stream).</param>
    /// <param name="allowedIPAddresses">Allowed IP addresses can be specified as either
    ///  <list type="bullet">
    ///      <item>
    ///         <term>a single IP address</term>
    ///         <description>for example '10.0.0.1'</description>
    ///      </item>
    ///      <item>
    ///         <term>an IP range using an IP address and a CIDR subnet mask</term>
    ///         <description>for example, '10.0.0.1/22'</description>
    ///      </item>
    ///      <item>
    ///         <term>an IP range using an IP address and a dotted decimal subnet mask</term>
    ///         <description>for example, '10.0.0.1(255.255.252.0)'</description>
    ///      </item>
    ///  </list>
    ///  </param>
    public static LiveEvent Create(string location,
                                   string description,
                                   LiveEventInputProtocol streamingProtocol,
                                   LiveEventEncodingType encodingType,
                                   ICollection<string> allowedIPAddresses)
    {
        return BuildLiveEvent(location,
                              description,
                              ConfigureAllowedAddresses(allowedIPAddresses),
                              streamingProtocol,
                              encodingType);
    }

    /// <summary>
    /// Creates a <see cref="LiveEvent"/> with NO IP restrictions on the ingest and preview.
    /// </summary>
    /// <param name="location">Geo-location where to allocate this <see cref="LiveEvent"/>. Use MediaServices' location.</param>
    /// <param name="description">Livestream's description.</param>
    /// <param name="streamingProtocol">The input protocol for the LiveEvent's ingest endpoint.</param>
    /// <param name="encodingType">A live event can be set to either a basic or standard pass-through (an on-premises live encoder sends a multiple bitrate stream) or live encoding (an on-premises live encoder sends a single bitrate stream).</param>
    public static LiveEvent Create(string location,
                                   string description,
                                   LiveEventInputProtocol streamingProtocol,
                                   LiveEventEncodingType encodingType)
    {
        return BuildLiveEvent(location,
                              description,
                              new IPRange[1]
                              {
                                  new(name: "AllowAllAddresses",
                                  address: "0.0.0.0",
                                  subnetPrefixLength: 0)
                              },
                              streamingProtocol,
                              encodingType);
    }

    private static LiveEvent BuildLiveEvent(string location,
                                          string description,
                                          IPRange[] allowedIPAddresses,
                                          LiveEventInputProtocol streamingProtocol,
                                          LiveEventEncodingType encodingType)
    {
        var liveEventInputAccess = new LiveEventInputAccessControl()
        {
            Ip = new IPAccessControl(allow: allowedIPAddresses)
        };
        var liveEventPreview = new LiveEventPreview()
        {
            AccessControl = new LiveEventPreviewAccessControl(ip: new IPAccessControl(allow: allowedIPAddresses))
        };
        var streamOptions = new List<StreamOptionsFlag?>(1) { StreamOptionsFlag.LowLatency };

        var input = BuildInput(streamingProtocol, liveEventInputAccess, "acf7b6ef-8a37-425f-b8fc-51c2d6a5a86a");
        var encoding = BuildEncoding(encodingType);

        return new LiveEvent(location: location,
                             description: description,
                             useStaticHostname: true, // a static hostname will make the ingest and preview URL host name the same
                             input: input,
                             encoding: encoding,
                             preview: liveEventPreview,
                             streamOptions: streamOptions);
    }

    private static IPRange[] ConfigureAllowedAddresses(ICollection<string> allowedIPAddresses)
        => allowedIPAddresses.Select((addr, index) => new IPRange(name: "AllowedAddress" + index,
                                                                 address: addr,
                                                                 subnetPrefixLength: 0))
                             .ToArray();

    /// <param name="accessToken">
    ///     A hardcoded value like "acf7b6ef-8a37-425f-b8fc-51c2d6a5a86a" in combination with useStaticHostname:true (from LiveEvent constructor),
    ///     to make sure the ingest URL is static and always the same.
    ///     If the value is null, the service will generate a random GUID value.
    /// </param>
    private static LiveEventInput BuildInput(LiveEventInputProtocol streamingProtocol, LiveEventInputAccessControl eventAccessControl, string accessToken = null)
        => new(streamingProtocol: streamingProtocol,
               accessToken: accessToken,
               accessControl: eventAccessControl,
               keyFrameIntervalDuration: "PT2S");

    private static LiveEventEncoding BuildEncoding(LiveEventEncodingType encodingType) => new(encodingType: encodingType);
}