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
            if (string.IsNullOrEmpty(blobName)) throw new ArgumentNullException("blobName");

            // Retrieve reference to a blob
            CloudBlockBlob blockBlob = GetBlockBlobReference(blobName);

            // Upload the stream
            blockBlob.UploadFromStream(source);
        }

        public void AddOrUpdateBlockBlob(string blobName, FileInformation fileInfo)
        {
            if (string.IsNullOrEmpty(blobName)) throw new ArgumentNullException("blobName");

            // Retrieve reference to a blob
            CloudBlockBlob blockBlob = GetBlockBlobReference(blobName);

            // Set content type if we have it
            if (!string.IsNullOrEmpty(fileInfo.ContentType))
                blockBlob.Properties.ContentType = fileInfo.ContentType;

            // Upload the stream
            blockBlob.UploadFromStream(fileInfo.Stream);
        }

        public void AddOrUpdateBlockBlob(string blobName, string directory, Stream source)
        {
            if (string.IsNullOrEmpty(blobName)) throw new ArgumentNullException("blobName");
            if (string.IsNullOrEmpty(directory)) throw new ArgumentNullException("directory");
            // Make sure the directory ends with a slash or we get unexpected results
            if (!directory.EndsWith(@"/") && !directory.EndsWith(@"\"))
                directory += @"/";

            // Retrieve reference to a blob
            CloudBlockBlob blockBlob = GetBlockBlobReference(blobName, directory);

            // Upload the stream
            blockBlob.UploadFromStream(source);
        }

        public void AddOrUpdateBlockBlob(string blobName, string directory, FileInformation fileInfo)
        {
            if (string.IsNullOrEmpty(blobName)) throw new ArgumentNullException("blobName");
            if (string.IsNullOrEmpty(directory)) throw new ArgumentNullException("directory");
            // Make sure the directory ends with a slash or we get unexpected results
            if (!directory.EndsWith(@"/") && !directory.EndsWith(@"\"))
                directory += @"/";

            // Retrieve reference to a blob
            CloudBlockBlob blockBlob = GetBlockBlobReference(blobName, directory);

            // Set content type if we have it
            if (!string.IsNullOrEmpty(fileInfo.ContentType))
                blockBlob.Properties.ContentType = fileInfo.ContentType;

            // Upload the stream
            blockBlob.UploadFromStream(fileInfo.Stream);
        }

        public async Task AddOrUpdateBlockBlobAsync(string blobName, Stream source)
        {
            if (string.IsNullOrEmpty(blobName)) throw new ArgumentNullException("blobName");

            // Retrieve reference to a blob
            CloudBlockBlob blockBlob = GetBlockBlobReference(blobName);

            // Upload the stream
            await blockBlob.UploadFromStreamAsync(source);
        }

        public async Task AddOrUpdateBlockBlobAsync(string blobName, FileInformation fileInfo)
        {
            if (string.IsNullOrEmpty(blobName)) throw new ArgumentNullException("blobName");

            // Retrieve reference to a blob
            CloudBlockBlob blockBlob = GetBlockBlobReference(blobName);

            // Set content type if we have it
            if (!string.IsNullOrEmpty(fileInfo.ContentType))
                blockBlob.Properties.ContentType = fileInfo.ContentType;

            // Upload the stream
            await blockBlob.UploadFromStreamAsync(fileInfo.Stream);
        }

        public async Task AddOrUpdateBlockBlobAsync(string blobName, string directory, Stream source)
        {
            if (string.IsNullOrEmpty(blobName)) throw new ArgumentNullException("blobName");
            if (string.IsNullOrEmpty(directory)) throw new ArgumentNullException("directory");
            // Make sure the directory ends with a slash or we get unexpected results
            if (!directory.EndsWith(@"/") && !directory.EndsWith(@"\"))
                directory += @"/";

            // Retrieve reference to a blob
            CloudBlockBlob blockBlob = GetBlockBlobReference(blobName, directory);

            // Upload the stream
            await blockBlob.UploadFromStreamAsync(source);
        }

        public async Task AddOrUpdateBlockBlobAsync(string blobName, string directory, FileInformation fileInfo)
        {
            if (string.IsNullOrEmpty(blobName)) throw new ArgumentNullException("blobName");
            if (string.IsNullOrEmpty(directory)) throw new ArgumentNullException("directory");
            // Make sure the directory ends with a slash or we get unexpected results
            if (!directory.EndsWith(@"/") && !directory.EndsWith(@"\"))
                directory += @"/";

            // Retrieve reference to a blob
            CloudBlockBlob blockBlob = GetBlockBlobReference(blobName, directory);

            // Set content type if we have it
            if (!string.IsNullOrEmpty(fileInfo.ContentType))
                blockBlob.Properties.ContentType = fileInfo.ContentType;

            // Upload the stream
            await blockBlob.UploadFromStreamAsync(fileInfo.Stream);
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
            return GetBlobContents(blobName, string.Empty);
        }

        public byte[] GetBlobContents(string blobName, string directory)
        {
            if (string.IsNullOrEmpty(blobName)) throw new ArgumentNullException("blobName");
            if (directory == null) throw new ArgumentNullException("directory");

            // Retrieve reference to a blob
            CloudBlockBlob blockBlob = GetBlockBlobReference(blobName, directory);
            blockBlob.FetchAttributes();
            byte[] contents = new byte[blockBlob.Properties.Length];
            blockBlob.DownloadToByteArray(contents, 0);
            return contents;
        }

        public async Task<byte[]> GetBlobContentsAsync(string blobName)
        {
            return await GetBlobContentsAsync(blobName, string.Empty);
        }

        public async Task<byte[]> GetBlobContentsAsync(string blobName, string directory)
        {
            if (string.IsNullOrEmpty(blobName)) throw new ArgumentNullException("blobName");
            if (directory == null) throw new ArgumentNullException("directory");

            // Retrieve reference to a blob
            CloudBlockBlob blockBlob = GetBlockBlobReference(blobName, directory);

            byte[] contents = new byte[blockBlob.Properties.Length];
            await blockBlob.DownloadToByteArrayAsync(contents, 0);
            return contents;
        }

        public void GetBlobContents(string blobName, Stream stream)
        {
            GetBlobContents(blobName, string.Empty, stream);
        }

        public void GetBlobContents(string blobName, string directory, Stream stream)
        {
            if (string.IsNullOrEmpty(blobName)) throw new ArgumentNullException("blobName");
            if (directory == null) throw new ArgumentNullException("directory");

            // Retrieve reference to a blob
            CloudBlockBlob blockBlob = GetBlockBlobReference(blobName, directory);

            blockBlob.DownloadToStream(stream);
        }

        public async Task GetBlobContentsAsync(string blobName, Stream stream)
        {
            await GetBlobContentsAsync(blobName, string.Empty, stream);
        }

        public async Task GetBlobContentsAsync(string blobName, string directory, Stream stream)
        {
            if (string.IsNullOrEmpty(blobName)) throw new ArgumentNullException("blobName");
            if (directory == null) throw new ArgumentNullException("directory");

            // Retrieve reference to a blob
            CloudBlockBlob blockBlob = GetBlockBlobReference(blobName, directory);

            await blockBlob.DownloadToStreamAsync(stream);
        }

        public bool BlobExistsOnCloud(string blobName)
        {
            return GetBlockBlobReference(blobName).Exists();
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

        /// <summary>
        /// Gets a block blob reference from the container
        /// </summary>
        /// <param name="blobName"></param>
        /// <param name="directory"></param>
        /// <returns></returns>
        private CloudBlockBlob GetBlockBlobReference(string blobName, string directory)
        {
            // Url encode the name
            blobName = HttpUtility.UrlEncode(blobName);

            // No case manipulation. Blob names are case sensitive
            // Make sure name is valid
            if (!this.IsValidBlobName(blobName))
                throw new ApplicationException(string.Format("Invalid blob name {0} was supplied. Blob names cannot end with dots or forward slashes; must be between {1} and {2} characters in length.", blobName, MIN_BLOB_NAME_LENGTH, MAX_BLOB_NAME_LENGTH));

            // Retrieve reference to a blob
            var blob = mCloudBlobContainer.GetBlockBlobReference(directory + blobName);
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
