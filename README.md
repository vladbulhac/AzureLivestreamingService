# Livestreaming Application

Built with .NET6 on the backend using Azure Media Services v3 APIs, and Angular on the frontend, from the example provided at [Azure-Samples](https://github.com/Azure-Samples/media-services-v3-dotnet/tree/main/Live/LiveEventWithDVR#readme).

#### Required Environment Variables

```
"environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development",
        "ASPNETCORE_HOSTINGSTARTUPASSEMBLIES": "Microsoft.AspNetCore.SpaProxy",
        "AZURE_ACTIVE_DIRECTORY_ClientId": "YOUR_AAD_CLIENTID",
        "AZURE_ACTIVE_DIRECTORY_ClientSecret": "YOUR_AAD_CLIENTSECRET",
        "AZURE_ACTIVE_DIRECTORY_Audience": "YOUR_AAD_AUD",
        "AZURE_ACTIVE_DIRECTORY_TenantId": "YOUR_AAD_TENANTID",
        "AZURE_MEDIA_SERVICES_AccountName": "YOUR_AMS_ACCNAME",
        "AZURE_MEDIA_SERVICES_ResourceGroup": "YOUR_AMS_RESOURCEGROUP",
        "AZURE_MEDIA_SERVICES_SubscriptionId": "YOUR_AMS_SUBSCRIPTIONID",
        "AZURE_RESOURCE_MANAGER_Endpoint": "YOUR_AMS_ENDPOINT",
        "AZURE_RESOURCE_MANAGER_TOKEN_AUDIENCE": "YOUR_AMS_TOKENAUD",
        "AZURE_STORAGE_AccountKey": "YOUR_AS_ACCKEY",
        "AZURE_STORAGE_AccountName": "YOUR_AS_ACCNAME",
        "AZURE_STORAGE_ContainerName": "YOUR_AS_CONTAINERNAME"
      }
```

_To test locally copy the key-value pairs from above in the Properties/launchSettings.json file_
