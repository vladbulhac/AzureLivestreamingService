namespace Livestreaming.Infrastructure.Services.Azure.Credentials;
public record AzureMediaServicesCredentials
{
    public string SubscriptionId { get; init; }
    public string ResourceGroup { get; init; }
    public string AccountName { get; init; }
}