namespace Livestreaming.Application.DTOs;
public record UserLivestreamsDto
{
    public IEnumerable<LivestreamDetails> livestreams { get; init; }
}