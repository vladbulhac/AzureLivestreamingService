namespace Livestreaming.Application.DTOs;
public record GetLivestreamsDto
{
    public string UserId { get; init; }
    public int Page { get; init; }
}