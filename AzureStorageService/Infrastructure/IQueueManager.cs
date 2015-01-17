using Microsoft.WindowsAzure.Storage.Queue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureStorageService.Infrastructure
{
    internal interface IQueueManager
    {
        #region Properties

        /// <summary>
        /// Gets whether or not the manager has been initialized
        /// </summary>
        bool IsInitialized { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Initialize the queue manager
        /// </summary>
        /// <param name="client"></param>
        void Initialize(CloudQueueClient client);

        /// <summary>
        /// Get a queue by name. Creates the queue if it does not already exist.
        /// </summary>
        /// <param name="name">Case insensitive name of the queue</param>
        /// <returns></returns>
        IStorageQueue GetQueue(string name);

        /// <summary>
        /// Get a queue by name asynchronously. Creates the queue if it does not already exist.
        /// </summary>
        /// <param name="name">Case insensitive name of the queue</param>
        /// <returns></returns>
        Task<IStorageQueue> GetQueueAsync(string name);

        #endregion
    }
}
