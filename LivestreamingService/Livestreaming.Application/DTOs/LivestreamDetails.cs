namespace Livestreaming.Application.DTOs;

public record LivestreamDetails
{
    public string LivestreamId { get; init; }
    public string Title { get; init; }
    public string Description { get; init; }
    public DateTime Date { get; init; }
    public string Status { get; init; }
}