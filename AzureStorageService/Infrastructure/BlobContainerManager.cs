using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AzureStorageService.Infrastructure
{
    internal class BlobContainerManager : IBlobContainerManager
    {
        #region Declarations

        private const int MIN_CONTAINER_NAME_LENGTH = 3;
        private const int MAX_CONTAINER_NAME_LENGTH = 63;

        private readonly ConcurrentDictionary<string, IBlobContainer> mBlobContainers = new ConcurrentDictionary<string, IBlobContainer>();
        private readonly Regex mContainerNameRegex = new Regex(@"^[a-z\d-]+$", RegexOptions.Singleline);

        private CloudBlobClient mClient;
        private bool mIsInitialized = false;

        #endregion

        #region Properties

        public bool IsInitialized { get { return mIsInitialized; } }

        #endregion

        #region Methods

        /// <summary>
        /// Initialize the blob manager
        /// </summary>
        /// <param name="client"></param>
        public void Initialize(CloudBlobClient client)
        {
            mClient = client;
            mIsInitialized = true;
        }

        public IBlobContainer GetBlobContainer(string name)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException("name");

            // Sanitize the string
            name = name.Trim().ToLower();

            // Make sure name is valid
            if (!this.IsValidContainerName(name))
                throw new ApplicationException(string.Format("Invalid blob container name {0} was supplied. Blob container names cannot end or being with dashes; must contain only letters, numbers, and dashes; cannot contain two consecutive dashes; must be between {1} and {2} characters in length.", name, MIN_CONTAINER_NAME_LENGTH, MAX_CONTAINER_NAME_LENGTH));

            // Check if blob container already exists in memory
            if (this.mBlobContainers.ContainsKey(name))
                return this.mBlobContainers[name];

            // Doesn't exist in memory, get a reference and try to create it if it doesn't exist
            CloudBlobContainer cloudBlobContainer = mClient.GetContainerReference(name);
            cloudBlobContainer.CreateIfNotExists();

            // Create a new blob container
            BlobContainer blobContainer = new BlobContainer(cloudBlobContainer, name);
            mBlobContainers.TryAdd(name, blobContainer);
            return blobContainer;
        }

        public async Task<IBlobContainer> GetBlobContainerAsync(string name)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException("name");

            // Sanitize the string
            name = name.Trim().ToLower();

            // Make sure name is valid
            if (!this.IsValidContainerName(name))
                throw new ApplicationException(string.Format("Invalid blob container name {0} was supplied. Blob container names cannot end or being with dashes; must contain only letters, numbers, and dashes; cannot contain two consecutive dashes; must be between {1} and {2} characters in length.", name, MIN_CONTAINER_NAME_LENGTH, MAX_CONTAINER_NAME_LENGTH));

            // Check if blob container already exists in memory
            if (this.mBlobContainers.ContainsKey(name))
                return this.mBlobContainers[name];

            // Doesn't exist in memory, get a reference and try to create it if it doesn't exist
            CloudBlobContainer cloudBlobContainer = mClient.GetContainerReference(name);
            await cloudBlobContainer.CreateIfNotExistsAsync();

            // Create a new blob container
            BlobContainer blobContainer = new BlobContainer(cloudBlobContainer, name);
            mBlobContainers.TryAdd(name, blobContainer);
            return blobContainer;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets whether or not the passed container name is valid
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private bool IsValidContainerName(string name)
        {
            // Dash is not permitted at the start or end of the name
            if (name.StartsWith("-") || name.EndsWith("-"))
                return false;
            // Consecutive dashes are not allowed in the name
            if (name.Contains("--"))
                return false;
            // Length
            if (name.Length < MIN_CONTAINER_NAME_LENGTH || name.Length > MAX_CONTAINER_NAME_LENGTH)
                return false;
            // Check against regex
            if (!mContainerNameRegex.IsMatch(name))
                return false;

            return true;
        }

        #endregion
    }
}
