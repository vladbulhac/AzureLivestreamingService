namespace Livestreaming.Presentation.Requests;

public record StartLivestreamRequest
{
    public string UserId { get; init; }
}