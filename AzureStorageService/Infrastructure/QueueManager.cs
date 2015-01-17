using Microsoft.WindowsAzure.Storage.Queue;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AzureStorageService.Infrastructure
{
    public class QueueManager : IQueueManager
    {
        #region Declarations

        private const int MIN_QUEUE_NAME_LENGTH = 3;
        private const int MAX_QUEUE_NAME_LENGTH = 63;

        private readonly ConcurrentDictionary<string, IStorageQueue> mQueues = new ConcurrentDictionary<string, IStorageQueue>();
        private readonly Regex mQueueNameRegex = new Regex(@"^[a-z\d-]+$", RegexOptions.Singleline);

        private CloudQueueClient mClient;
        private bool mIsInitialized = false;

        #endregion

        #region Properties

        public bool IsInitialized { get { return mIsInitialized;  } }

        #endregion

        #region Public Methods

        public void Initialize(CloudQueueClient client)
        {
            mClient = client;
            mIsInitialized = true;
        }

        public IStorageQueue GetQueue(string name)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException("name");

            // Sanitize the string
            name = name.Trim().ToLower();

            // Make sure name is valid
            if (!this.IsValidQueueName(name))
                throw new ApplicationException(string.Format("Invalid queue name {0} was supplied. Queue names cannot end or being with dashes; must contain only letters, numbers, and dashes; cannot contain two consecutive dashes; must be between {1} and {2} characters in length.", name, MIN_QUEUE_NAME_LENGTH, MAX_QUEUE_NAME_LENGTH));

            // Check if queue already exists in memory
            if (this.mQueues.ContainsKey(name))
                return this.mQueues[name];

            // Doesn't exist in memory, get a reference and try to create it if it doesn't exist
            CloudQueue cloudQueue = mClient.GetQueueReference(name);
            cloudQueue.CreateIfNotExists();

            // Create a new queue
            StorageQueue queue = new StorageQueue(cloudQueue, name);
            mQueues.TryAdd(name, queue);
            return queue;
        }

        public async Task<IStorageQueue> GetQueueAsync(string name)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException("name");

            // Sanitize the string
            name = name.Trim().ToLower();

            // Make sure name is valid
            if (!this.IsValidQueueName(name))
                throw new ApplicationException(string.Format("Invalid queue name {0} was supplied. Queue names cannot end or being with dashes; must contain only letters, numbers, and dashes; cannot contain two consecutive dashes; must be between {1} and {2} characters in length.", name, MIN_QUEUE_NAME_LENGTH, MAX_QUEUE_NAME_LENGTH));

            // Check if queue already exists in memory
            if (this.mQueues.ContainsKey(name))
                return this.mQueues[name];

            // Doesn't exist in memory, get a reference and try to create it if it doesn't exist
            CloudQueue cloudQueue = mClient.GetQueueReference(name);
            await cloudQueue.CreateIfNotExistsAsync();

            // Create a new queue
            StorageQueue queue = new StorageQueue(cloudQueue, name);
            mQueues.TryAdd(name, queue);
            return queue;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets whether or not the passed queue name is valid
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private bool IsValidQueueName(string name)
        {
            // Dash is not permitted at the start or end of the name
            if (name.StartsWith("-") || name.EndsWith("-"))
                return false;
            // Consecutive dashes are not allowed in the name
            if (name.Contains("--"))
                return false;
            // Length
            if (name.Length < MIN_QUEUE_NAME_LENGTH || name.Length > MAX_QUEUE_NAME_LENGTH)
                return false;
            // Check against regex
            if (!mQueueNameRegex.IsMatch(name))
                return false;
            
            return true;
        }

        #endregion
    }
}
