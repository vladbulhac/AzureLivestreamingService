namespace Livestreaming.Infrastructure.Services.Azure.Credentials;
public record AzureStorageCredentials
{
    public string AccountName { get; init; }
    public string AccountKey { get; init; }
    public string ContainerName { get; init; }
}