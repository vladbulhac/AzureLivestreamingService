namespace Livestreaming.Application.DTOs;

public record StartLivestreamDto
{
    public string UserId { get; init; }
    public string LivestreamId { get; init; }
}