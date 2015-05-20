using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureStorageService.Infrastructure
{
    /// <summary>
    /// Concrete IStorageAccountService implementation. This class should be a 
    /// </summary>
    internal class StorageAccountService : IStorageAccountService
    {
        #region Declarations

        // Dependencies
        private readonly IQueueManager mQueueManager;
        private readonly IBlobContainerManager mBlobContainerManager;
        private readonly ITableManager mTableManager;

        // Storage account object
        private CloudStorageAccount mStorageAccount;

        // Has the storage service been initialized?
        private bool mIsInitialized = false;

        #endregion

        #region Constructors

        public StorageAccountService(IQueueManager queueManager, IBlobContainerManager blobContainerManager, ITableManager tableManager, string connectionString)
        {
            if (queueManager == null) throw new ArgumentNullException("queueManager");
            if (blobContainerManager == null) throw new ArgumentNullException("blobContainerManager");
            if (tableManager == null) throw new ArgumentNullException("tableManager");
            if (string.IsNullOrEmpty(connectionString)) throw new ArgumentNullException("connectionString");

            // Assign managers
            mQueueManager = queueManager;
            mBlobContainerManager = blobContainerManager;
            mTableManager = tableManager;

            // Initialize the service
            this.Initialize(connectionString);
        }

        #endregion

        #region Properties

        public bool IsInitialized { get { return mIsInitialized; } }

        #endregion

        #region Public Methods

        public IStorageQueue GetQueue(string name)
        {
            return mQueueManager.GetQueue(name);
        }

        public async Task<IStorageQueue> GetQueueAsync(string name)
        {
            return await mQueueManager.GetQueueAsync(name);
        }

        public IBlobContainer GetBlobContainer(string name)
        {
            return mBlobContainerManager.GetBlobContainer(name);
        }

        public async Task<IBlobContainer> GetBlobContainerAsync(string name)
        {
            return await mBlobContainerManager.GetBlobContainerAsync(name);
        }

        public ITable GetTable(string name)
        {
            return mTableManager.GetTable(name);
        }

        public async Task<ITable> GetTableAsync(string name) 
        {
            return await mTableManager.GetTableAsync(name);
        }

        #endregion

        #region Private Methods

        private void Initialize(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString)) throw new ArgumentNullException("connectionString");

            // Get storage account reference and make sure it exists
            mStorageAccount = CloudStorageAccount.Parse(connectionString);
            if (mStorageAccount == null)
                throw new ApplicationException("Storage account is null!");

            // Create clients that will be needed here
            mQueueManager.Initialize(mStorageAccount.CreateCloudQueueClient());
            mBlobContainerManager.Initialize(mStorageAccount.CreateCloudBlobClient());
            mTableManager.Initialize(mStorageAccount.CreateCloudTableClient());

            // Set initialized flag
            mIsInitialized = true;
        }

        #endregion
    }
}
