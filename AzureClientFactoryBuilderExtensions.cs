using Azure.Core.Extensions;
using Azure.Data.Tables;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Azure;

namespace LebaraSign
{
    internal static class AzureClientFactoryBuilderExtensions
    {
        public static IAzureClientBuilder<TableServiceClient, TableClientOptions> AddTableServiceClient(this AzureClientFactoryBuilder builder, string serviceUriOrConnectionString, bool preferMsi)
        {
            if (preferMsi && Uri.TryCreate(serviceUriOrConnectionString, UriKind.Absolute, out Uri? serviceUri))
            {
                return builder.AddTableServiceClient(serviceUri);
            }
            else
            {
                return builder.AddTableServiceClient(serviceUriOrConnectionString);
            }
        }

        public static IAzureClientBuilder<BlobServiceClient, BlobClientOptions> AddBlobServiceClient(this AzureClientFactoryBuilder builder, string serviceUriOrConnectionString, bool preferMsi)
        {
            if (preferMsi && Uri.TryCreate(serviceUriOrConnectionString, UriKind.Absolute, out Uri? serviceUri))
            {
                return builder.AddBlobServiceClient(serviceUri);
            }
            else 
            {
                return builder.AddBlobServiceClient(serviceUriOrConnectionString);
            }
        }
    }
}
