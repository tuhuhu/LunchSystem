using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Configuration;
using System.IO;

namespace SystemSetup.UtilityServices
{
    public static class AzureBlobUtility
    {
        public static MemoryStream GetBlockBlobStream(string containerName, string blobName)
        {
            var connectionString = ConfigurationManager.AppSettings["AzureStorageConnectionString"];
            var storageAccount = CloudStorageAccount.Parse(connectionString);
            var blobClient = storageAccount.CreateCloudBlobClient();

            // Retrieve reference to a previously created container.
            CloudBlobContainer container = blobClient.GetContainerReference(containerName);

            // Retrieve reference to a blob.
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(blobName);

            var memoryStream = new MemoryStream();

            // Save blob contents to a MemoryStream.
            blockBlob.DownloadToStream(memoryStream);
            memoryStream.Position = 0;
            return memoryStream;
        }

    }
}
