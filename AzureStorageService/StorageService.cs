using AzureStorageService.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureStorageService
{
    public static class StorageService
    {
        /// <summary>
        /// Instance of the IStorageAccountService which handles azure storage
        /// </summary>
        public static IStorageAccountService Instance { get; internal set; }
    }
}
