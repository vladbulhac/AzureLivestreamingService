namespace Livestreaming.Infrastructure.Services.Azure.Credentials;
public record AzureResourceManagerCredentials
{
    public Uri Endpoint { get; init; }

    public AzureResourceManagerCredentials() { }
    public AzureResourceManagerCredentials(string uri) => Endpoint = new Uri(uri);
}