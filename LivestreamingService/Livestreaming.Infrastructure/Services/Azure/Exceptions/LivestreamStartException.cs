using System.Net;

namespace Livestreaming.Infrastructure.Services.Azure.Exceptions;

public class LivestreamStartException : HttpRequestException
{
    public LivestreamStartException()
    {
    }

    public LivestreamStartException(string message)
        : base(message)
    {
    }

    public LivestreamStartException(string message, Exception inner)
        : base(message, inner)
    {
    }

    public LivestreamStartException(HttpStatusCode statusCode, string message, Exception inner)
        : base(message, inner, statusCode)
    {
    }
}