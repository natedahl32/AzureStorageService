using AzureStorageService;
using Microsoft.WindowsAzure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageServiceConsoleTest
{
    class ProgramBlobWithContentType
    {
        static string blobContainerName = "testBlob";
        static string blobName = "Tqdlaw.theme.css";

        static void Main(string[] args)
        {
            // Configure the storage service
            StorageServiceConfigurator.Configure(CloudConfigurationManager.GetSetting("StorageConnectionString"));
            Console.WriteLine("Storage Service configured.");

            var blobContainer = StorageService.Instance.GetBlobContainer(blobContainerName);

            // Write to blob
            using (var reader = File.OpenRead(@"C:\temp\Tqdlaw.theme.css"))
            {
                blobContainer.AddOrUpdateBlockBlob(blobName, "frontend/css/", new FileInformation(reader, "text/css"));
                Console.WriteLine(string.Format("Created blob {0}", blobName));
            }

            // Retrieve the blob and write the file
            using (var writer = File.OpenWrite(@"C:\temp\New.Tqdlaw.theme.css"))
            {
                blobContainer.GetBlobContents(blobName, "frontend/css/", writer);
                Console.WriteLine(string.Format("Retrieved blob {0} and wrote to file {1}", blobName, @"C:\temp\New.Tqdlaw.theme.css"));
            }
        }
    }
}
