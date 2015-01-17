using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace AzureStorageService.Infrastructure
{
    internal class BlobContainer : IBlobContainer
    {
        #region Declarations

        private const int MIN_BLOB_NAME_LENGTH = 1;
        private const int MAX_BLOB_NAME_LENGTH = 1024;

        private readonly CloudBlobContainer mCloudBlobContainer;
        private readonly string mName;

        #endregion

        #region Constructors

        public BlobContainer(CloudBlobContainer blobContainer, string name)
        {
            mCloudBlobContainer = blobContainer;
            mName = name;
        }

        #endregion

        #region Properties

        public string ContainerName { get { return mName; } }

        #endregion

        #region Public Methods

        public void AddOrUpdateBlockBlob(string blobName, Stream source)
        {
            // Retrieve reference to a blob
            CloudBlockBlob blockBlob = GetBlockBlobReference(blobName);

            // Upload the stream
            blockBlob.UploadFromStream(source);
        }

        public async Task AddOrUpdateBlockBlobAsync(string blobName, Stream source)
        {
            // Retrieve reference to a blob
            CloudBlockBlob blockBlob = GetBlockBlobReference(blobName);

            // Upload the stream
            await blockBlob.UploadFromStreamAsync(source);
        }

        public void DeleteBlockBlob(string blobName)
        {
            // Retrieve reference to a blob
            CloudBlockBlob blockBlob = GetBlockBlobReference(blobName);

            // Delete the block blob
            blockBlob.Delete();
        }

        public async void DeleteBlockBlobAsync(string blobName)
        {
            // Retrieve reference to a blob
            CloudBlockBlob blockBlob = GetBlockBlobReference(blobName);

            // Delete the block blob
            await blockBlob.DeleteAsync();
        }

        public byte[] GetBlobContents(string blobName)
        {
            // Retrieve reference to a blob
            CloudBlockBlob blockBlob = GetBlockBlobReference(blobName);
            
            byte[] contents = new byte[blockBlob.Properties.Length];
            blockBlob.DownloadToByteArray(contents, 0);
            return contents;
        }

        public async Task<byte[]> GetBlobContentsAsync(string blobName)
        {
            // Retrieve reference to a blob
            CloudBlockBlob blockBlob = GetBlockBlobReference(blobName);

            byte[] contents = new byte[blockBlob.Properties.Length];
            await blockBlob.DownloadToByteArrayAsync(contents, 0);
            return contents;
        }

        public void GetBlobContents(string blobName, Stream stream)
        {
            // Retrieve reference to a blob
            CloudBlockBlob blockBlob = GetBlockBlobReference(blobName);

            blockBlob.DownloadToStream(stream);
        }

        public async Task GetBlobContentsAsync(string blobName, Stream stream)
        {
            // Retrieve reference to a blob
            CloudBlockBlob blockBlob = GetBlockBlobReference(blobName);

            await blockBlob.DownloadToStreamAsync(stream);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets a block blob reference from the container
        /// </summary>
        /// <param name="blobName"></param>
        /// <returns></returns>
        private CloudBlockBlob GetBlockBlobReference(string blobName)
        {
            // Url encode the name
            blobName = HttpUtility.UrlEncode(blobName);

            // No case manipulation. Blob names are case sensitive
            // Make sure name is valid
            if (!this.IsValidBlobName(blobName))
                throw new ApplicationException(string.Format("Invalid blob name {0} was supplied. Blob names cannot end with dots or forward slashes; must be between {1} and {2} characters in length.", blobName, MIN_BLOB_NAME_LENGTH, MAX_BLOB_NAME_LENGTH));

            // Retrieve reference to a blob
            var blob = mCloudBlobContainer.GetBlockBlobReference(blobName);
            if (blob == null)
                throw new ApplicationException(string.Format("Unable to get block blob reference with name {0}", blobName));
            return blob;
        }

        private bool IsValidBlobName(string name)
        {
            // Cannot end with dot or forward slash
            if (name.EndsWith(".") || name.EndsWith("/"))
                return false;
            // Length
            if (name.Length < MIN_BLOB_NAME_LENGTH || name.Length > MAX_BLOB_NAME_LENGTH)
                return false;

            return true;
        }

        #endregion
    }
}
