namespace Livestreaming.Application.DTOs;

public record GetLivestreamEndpointsDto
{
    public string LivestreamId { get; init; }
}