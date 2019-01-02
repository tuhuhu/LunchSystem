using log4net.Appender;
using log4net.Core;
using log4net.Layout;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Configuration;
using System.IO;
using System.Text;

namespace SystemSetup.UtilityServices
{
    public class RollingAzureBlobAppender : AppenderSkeleton
    {
        private CloudBlobClient _blobClient;
        protected override void Append(LoggingEvent loggingEvent)
        {
            if (null == Layout)
                Layout = new SimpleLayout();
            var sb = new StringBuilder();
            using (var sr = new StringWriter(sb))
            {
                Layout.Format(sr, loggingEvent);
                sr.Flush();
                try
                {
                    const string containerName = "logs"; /* lower casing only */
                    CloudBlobContainer container = _blobClient.GetContainerReference(containerName);
                    container.CreateIfNotExists(null, null);
                    // Creates a unique blob name 
                    var filename = String.Format("Logs-{0}-{1}", loggingEvent.Level.Name, Utility.GetCurrentDateOnly().ToString("MM.dd.yyyy")).ToUpper();
                    var blob = container.GetBlockBlobReference(filename);
                    string blobContent = GetBlobContent(blob);
                    using (var writer = new StreamWriter(blob.OpenWrite()))
                    {
                        writer.Write(blobContent);
                        writer.Write(sb.ToString());
                    }
                }
                catch
                {
                    System.Diagnostics.Trace.Write(sb.ToString(), loggingEvent.Level.Name);
                }
            }
        }

        public override void ActivateOptions()
        {
            base.ActivateOptions();
            //CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
            var connectionString = ConfigurationManager.AppSettings["AzureStorageConnectionString"];
            var storageAccount = CloudStorageAccount.Parse(connectionString);
            _blobClient = storageAccount.CreateCloudBlobClient();
        }


        private string GetBlobContent(CloudBlockBlob blob)
        {
            if (!blob.Exists())
            {
                return String.Empty;
            }
            string blobContent;
            using (var reader = new StreamReader(blob.OpenRead()))
            {
                blobContent = reader.ReadToEnd();
            }
            return blobContent;
        }

    }
    public static class BlobExtensions
    {
        /// <summary> 
        /// Checks if a blob exists 
        /// </summary> 
        /// <see cref="http://blog.smarx.com/posts/testing-existence-of-a-windows-azure-blob"/> 
        public static bool Exists(this CloudBlockBlob blob)
        {
            blob.FetchAttributes();
            return true;
        }
    }
}
