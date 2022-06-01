using System.Net;

namespace Livestreaming.Infrastructure.Services.Azure.Exceptions;

public class LivestreamCleanupException : HttpRequestException
{
    public LivestreamCleanupException()
    {
    }

    public LivestreamCleanupException(string message)
        : base(message)
    {
    }

    public LivestreamCleanupException(string message, Exception inner)
        : base(message, inner)
    {
    }

    public LivestreamCleanupException(HttpStatusCode statusCode, string message, Exception inner)
        : base(message, inner, statusCode)
    {
    }
}