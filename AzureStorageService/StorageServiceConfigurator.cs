using AzureStorageService.Infrastructure;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureStorageService
{
    public static class StorageServiceConfigurator
    {
        /// <summary>
        /// Configure the AzureStorageService instance
        /// </summary>
        /// <param name="kernel"></param>
        public static void Configure(string connectionString)
        {
            IKernel kernel = new StandardKernel();

            // Bind dependencies
            kernel.Bind<IStorageQueue>().To<StorageQueue>();
            kernel.Bind<IBlobContainer>().To<BlobContainer>();
            kernel.Bind<IQueueManager>().To<QueueManager>();
            kernel.Bind<ITableManager>().To<TableManager>();
            kernel.Bind<ITable>().To<Table>();
            kernel.Bind<IBlobContainerManager>().To<BlobContainerManager>();
            kernel.Bind<IStorageAccountService>().To<StorageAccountService>()
                  .InSingletonScope()
                  .WithConstructorArgument("connectionString", connectionString);

            // Set the instance of the storage service we will be using
            StorageService.Instance = kernel.Get<IStorageAccountService>();
        }
    }
}
