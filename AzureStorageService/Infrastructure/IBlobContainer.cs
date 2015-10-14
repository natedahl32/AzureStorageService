using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureStorageService.Infrastructure
{
    public interface IBlobContainer
    {
        #region Properties

        /// <summary>
        /// Gets the blob container name
        /// </summary>
        string ContainerName { get; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds or updates a block blob from a stream.
        /// </summary>
        /// <param name="blobName">Case sensitive name of blob</param>
        /// <param name="source">Stream to upload</param>
        void AddOrUpdateBlockBlob(string blobName, Stream source);

        /// <summary>
        /// Adds or updates a block blob from a stream.
        /// </summary>
        /// <param name="blobName">Case sensitive name of blob</param>
        /// <param name="fileInfo">Information about the file to upload</param>
        void AddOrUpdateBlockBlob(string blobName, FileInformation fileInfo);

        /// <summary>
        /// Adds or updates a block blob from a stream.
        /// </summary>
        /// <param name="blobName">Case sensitive name of blob</param>
        /// <param name="directory">Directory to save the blob to in the container</param>
        /// <param name="source">Stream to upload</param>
        void AddOrUpdateBlockBlob(string blobName, string directory, Stream source);

        /// <summary>
        /// Adds or updates a block blob from a stream.
        /// </summary>
        /// <param name="blobName">Case sensitive name of blob</param>
        /// <param name="directory">Directory to save the blob to in the container</param>
        /// <param name="fileInfo">Information about the file to upload</param>
        void AddOrUpdateBlockBlob(string blobName, string directory, FileInformation fileInfo);

        /// <summary>
        /// Adds or updates a block blob from a stream, asynchronously
        /// </summary>
        /// <param name="blobName">Case sensitive name of blob</param>
        /// <param name="source">Stream to upload</param>
        Task AddOrUpdateBlockBlobAsync(string blobName, Stream source);

        /// <summary>
        /// Adds or updates a block blob from a stream, asynchronously
        /// </summary>
        /// <param name="blobName">Case sensitive name of blob</param>
        /// <param name="fileInfo">Information about the file to upload</param>
        Task AddOrUpdateBlockBlobAsync(string blobName, FileInformation fileInfo);

        /// <summary>
        /// Adds or updates a block blob from a stream, asynchronously
        /// </summary>
        /// <param name="blobName">Case sensitive name of blob</param>
        /// <param name="directory">Directory to save the blob to in the container</param>
        /// <param name="source">Stream to upload</param>
        Task AddOrUpdateBlockBlobAsync(string blobName, string directory, Stream source);

        /// <summary>
        /// Adds or updates a block blob from a stream, asynchronously
        /// </summary>
        /// <param name="blobName">Case sensitive name of blob</param>
        /// <param name="directory">Directory to save the blob to in the container</param>
        /// <param name="fileInfo">Information about the file to upload</param>
        Task AddOrUpdateBlockBlobAsync(string blobName, string directory, FileInformation fileInfo);

        /// <summary>
        /// Deletes a block blob from the container
        /// </summary>
        /// <param name="blobName"></param>
        void DeleteBlockBlob(string blobName);

        /// <summary>
        /// Deletes a block blob from the container, asynchronously
        /// </summary>
        /// <param name="blobName"></param>
        void DeleteBlockBlobAsync(string blobName);

        /// <summary>
        /// Retrieves the contents of a blob from the container. Assumes the blob exists in the cloud.
        /// </summary>
        /// <param name="blobName"></param>
        /// <returns>Byte array of file contents</returns>
        byte[] GetBlobContents(string blobName);

        /// <summary>
        /// Retrieves the contents of a blob from the container. Assumes the blob exists in the cloud.
        /// </summary>
        /// <param name="blobName"></param>
        /// <param name="directory"></param>
        /// <returns>Byte array of file contents</returns>
        byte[] GetBlobContents(string blobName, string directory);

        /// <summary>
        /// Retrieves the contents of a blob from the container
        /// </summary>
        /// <param name="blobName">Name of blob</param>
        /// <param name="stream">Stream to write contents to</param>
        /// <returns></returns>
        void GetBlobContents(string blobName, Stream stream);

        /// <summary>
        /// Retrieves the contents of a blob from the container
        /// </summary>
        /// <param name="blobName">Name of blob</param>
        /// <param name="directory">Directory the blob is in</param>
        /// <param name="stream">Stream to write contents to</param>
        /// <returns></returns>
        void GetBlobContents(string blobName, string directory, Stream stream);

        /// <summary>
        /// Retrieves the contents of a blob from the container, asynchronously
        /// </summary>
        /// <param name="blobName"></param>
        /// <returns>Byte array of file contents</returns>
        Task<byte[]> GetBlobContentsAsync(string blobName);

        /// <summary>
        /// Retrieves the contents of a blob from the container, asynchronously
        /// </summary>
        /// <param name="blobName"></param>
        /// <param name="directory"></param>
        /// <returns>Byte array of file contents</returns>
        Task<byte[]> GetBlobContentsAsync(string blobName, string directory);

        /// <summary>
        /// Retrieves the contents of a blob from the container
        /// </summary>
        /// <param name="blobName">Name of blob</param>
        /// <param name="stream">Stream to write contents to</param>
        /// <returns></returns>
        Task GetBlobContentsAsync(string blobName, Stream stream);

        /// <summary>
        /// Retrieves the contents of a blob from the container
        /// </summary>
        /// <param name="blobName">Name of blob</param>
        /// <param name="directory"></param>
        /// <param name="stream">Stream to write contents to</param>
        /// <returns></returns>
        Task GetBlobContentsAsync(string blobName, string directory, Stream stream);

        /// <summary>
        /// Does a check to see if the blob with this name exists on the cloud
        /// </summary>
        /// <param name="blobName"></param>
        /// <returns></returns>
        bool BlobExistsOnCloud(string blobName);

        #endregion
    }
}
