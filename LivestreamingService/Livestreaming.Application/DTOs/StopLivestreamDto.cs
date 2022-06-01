namespace Livestreaming.Application.DTOs;
public record StopLivestreamDto
{
    public string UserId { get; init; }
    public string LivestreamId { get; init; }
}