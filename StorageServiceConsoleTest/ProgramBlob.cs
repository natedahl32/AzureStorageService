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
    class ProgramBlob
    {
        static string blobContainerName = "testBlob";
        static string blobName = "SWI Specs";

        static void Main(string[] args)
        {
            // Configure the storage service
            StorageServiceConfigurator.Configure(CloudConfigurationManager.GetSetting("StorageConnectionString"));
            Console.WriteLine("Storage Service configured.");

            var blobContainer = StorageService.Instance.GetBlobContainer(blobContainerName);

            // Write to blob
            using (var reader = File.OpenRead(@"C:\temp\SWI Specifications.docx"))
            {
                blobContainer.AddOrUpdateBlockBlob(blobName, reader);
                Console.WriteLine(string.Format("Created blob {0}", blobName));
            }

            // Retrieve the blob and write the file
            using (var writer = File.OpenWrite(@"C:\temp\NewBlobDoc.docx"))
            {
                blobContainer.GetBlobContents(blobName, writer);
                Console.WriteLine(string.Format("Retrieved blob {0} and wrote to file {1}", blobName, @"C:\temp\NewBlobDoc.docx"));
            }
        }
    }
}
