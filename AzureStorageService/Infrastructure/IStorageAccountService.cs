using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureStorageService.Infrastructure
{
    public interface IStorageAccountService
    {
        #region Properties

        /// <summary>
        /// Gets whether or not the service has been initialized
        /// </summary>
        bool IsInitialized { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Gets a queue by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        IStorageQueue GetQueue(string name);

        /// <summary>
        /// Gets a queue by name, asynchronously
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<IStorageQueue> GetQueueAsync(string name);

        /// <summary>
        /// Gets a blob container by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        IBlobContainer GetBlobContainer(string name);

        /// <summary>
        /// Gets a blob container by name, asynchronously
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<IBlobContainer> GetBlobContainerAsync(string name);

        #endregion
    }
}
