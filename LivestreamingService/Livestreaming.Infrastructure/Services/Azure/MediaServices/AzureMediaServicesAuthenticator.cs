using Livestreaming.Infrastructure.Services.Azure.Credentials;
using Microsoft.Identity.Client;
using Microsoft.Rest;

namespace Livestreaming.Infrastructure.Services.Azure.MediaServices;

public static class AzureMediaServicesAuthenticator
{
    public static async Task<ServiceClientCredentials> GenerateTokenCrendetialsAsync()
    {
        var app = ConfidentialClientApplicationBuilder.Create(AzureServicesCredentials.AAD.ClientId)
                                                      .WithClientSecret(AzureServicesCredentials.AAD.ClientSecret)
                                                      .WithAuthority(AzureCloudInstance.AzurePublic, AzureServicesCredentials.AAD.TenantId)
                                                      .Build();

        var authenticationResult = await app.AcquireTokenForClient(AzureServicesCredentials.AAD.Scopes)
                                            .ExecuteAsync();

        return new TokenCredentials(authenticationResult.AccessToken, tokenType: "Bearer");
    }
}