using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureStorageService.Infrastructure
{
    public interface ITableManager
    {
        #region Properties

        /// <summary>
        /// Gets whether or not the manager has been initialized
        /// </summary>
        bool IsInitialized { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Initialize the table manager
        /// </summary>
        /// <param name="client"></param>
        void Initialize(CloudTableClient client);

        /// <summary>
        /// Get a table by name. Creates the table if it does not already exist.
        /// </summary>
        /// <param name="name">Case insensitive name of the table</param>
        /// <returns></returns>
        ITable GetTable(string name);

        /// <summary>
        /// Get a table by name asynchronously. Creates the table if it does not already exist.
        /// </summary>
        /// <param name="name">Case insensitive name of the table</param>
        /// <returns></returns>
        Task<ITable> GetTableAsync(string name);

        #endregion
    }
}
