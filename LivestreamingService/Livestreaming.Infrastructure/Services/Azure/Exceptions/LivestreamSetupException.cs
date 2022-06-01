using System.Net;

namespace Livestreaming.Infrastructure.Services.Azure.Exceptions;

public class LivestreamSetupException : HttpRequestException
{
    public LivestreamSetupException()
    {
    }

    public LivestreamSetupException(string message)
        : base(message)
    {
    }

    public LivestreamSetupException(string message, Exception inner)
        : base(message, inner)
    {
    }

    public LivestreamSetupException(HttpStatusCode statusCode, string message, Exception inner)
        : base(message, inner, statusCode)
    {
    }
}