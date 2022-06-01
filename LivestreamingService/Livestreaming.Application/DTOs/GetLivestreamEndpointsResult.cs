using Livestreaming.Domain.Models;

namespace Livestreaming.Application.DTOs;

public record GetLivestreamEndpointsResult : LivestreamDetails
{
    public LivestreamEndpoints RunningEndpoints { get; init; }
    public LivestreamPlaybackEndpoints PlaybackEndpoints { get; init; }
}