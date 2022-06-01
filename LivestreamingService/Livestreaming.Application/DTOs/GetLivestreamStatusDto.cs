namespace Livestreaming.Application.DTOs;
public record GetLivestreamStatusDto
{
    public string UserId { get; init; }
    public string LivestreamId { get; init; }
}