namespace Livestreaming.Infrastructure.Services.Azure.Credentials;
public record AzureActiveDirectoryCredentials
{
    public string ClientId { get; init; }
    public string ClientSecret { get; init; }
    public string Audience { get; init; }
    public string TenantId { get; init; }
    public string[] Scopes { get; init; }
}