using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureStorageService.Infrastructure
{
    internal interface IBlobContainerManager
    {
        #region Properties

        /// <summary>
        /// Gets whether or not the manager has been initialized
        /// </summary>
        bool IsInitialized { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Initialize the blob manager
        /// </summary>
        /// <param name="client"></param>
        void Initialize(CloudBlobClient client);

        /// <summary>
        /// Get a blob container by name. Creates the blob container if it does not already exist.
        /// </summary>
        /// <param name="name">Case insensitive name of the blob container</param>
        /// <returns></returns>
        IBlobContainer GetBlobContainer(string name);

        /// <summary>
        /// Get a blob container by name asynchronously. Creates the blob container if it does not already exist.
        /// </summary>
        /// <param name="name">Case insensitive name of the blob container</param>
        /// <returns></returns>
        Task<IBlobContainer> GetBlobContainerAsync(string name);

        #endregion
    }
}
