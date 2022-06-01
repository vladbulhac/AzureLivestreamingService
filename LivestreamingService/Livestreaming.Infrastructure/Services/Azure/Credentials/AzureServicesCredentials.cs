namespace Livestreaming.Infrastructure.Services.Azure.Credentials;

public static class AzureServicesCredentials
{
    public static AzureActiveDirectoryCredentials AAD;
    public static AzureMediaServicesCredentials AMS;
    public static AzureResourceManagerCredentials ARM;
    public static AzureStorageCredentials AST;

    static AzureServicesCredentials()
    {
        AAD = new()
        {
            ClientId = Environment.GetEnvironmentVariable("AZURE_ACTIVE_DIRECTORY_ClientId"),
            ClientSecret = Environment.GetEnvironmentVariable("AZURE_ACTIVE_DIRECTORY_ClientSecret"),
            Audience = Environment.GetEnvironmentVariable("AZURE_ACTIVE_DIRECTORY_Audience"),
            Scopes = new string[] { Environment.GetEnvironmentVariable("AZURE_ACTIVE_DIRECTORY_Audience") + "/.default" },
            TenantId = Environment.GetEnvironmentVariable("AZURE_ACTIVE_DIRECTORY_TenantId")
        };
        AMS = new()
        {
            AccountName = Environment.GetEnvironmentVariable("AZURE_MEDIA_SERVICES_AccountName"),
            ResourceGroup = Environment.GetEnvironmentVariable("AZURE_MEDIA_SERVICES_ResourceGroup"),
            SubscriptionId = Environment.GetEnvironmentVariable("AZURE_MEDIA_SERVICES_SubscriptionId"),
        };
        ARM = new() { Endpoint = new Uri(Environment.GetEnvironmentVariable("AZURE_RESOURCE_MANAGER_Endpoint")) };
        AST = new()
        {
            AccountKey = Environment.GetEnvironmentVariable("AZURE_STORAGE_AccountKey"),
            AccountName = Environment.GetEnvironmentVariable("AZURE_STORAGE_AccountName"),
            ContainerName = Environment.GetEnvironmentVariable("AZURE_STORAGE_ContainerName"),
        };
    }
}