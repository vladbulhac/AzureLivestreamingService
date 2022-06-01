namespace Livestreaming.Application.DTOs;
public record SetupLivestreamDto
{
    public string UserId { get; init; }
    public string Title { get; init; }
    public string Description { get; init; }
    public int RecordingDuration { get; init; }

    public string StreamingProtocol { get; init; }
    public string EncodingType { get; init; }
}