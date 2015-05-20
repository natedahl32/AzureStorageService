using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureStorageService.Infrastructure
{
    internal class Table : ITable
    {
        #region Declarations

        private readonly CloudTable mCloudTable;
        private readonly string mName;

        #endregion

        #region Constructors

        public Table(CloudTable cloudTable, string name)
        {
            mCloudTable = cloudTable;
            mName = name;
        }

        #endregion

        #region Properties

        public string TableName { get { return mName; } }

        #endregion

        #region Public Methods

        public void AddEntity<T>(T entity) where T : TableEntity
        {
            TableOperation insertOperation = TableOperation.Insert(entity);
            mCloudTable.Execute(insertOperation);
        }

        public T RetrieveEntity<T>(string partitionKey, string rowKey) where T : TableEntity
        {
            TableOperation retrieveOperation = TableOperation.Retrieve<T>(partitionKey, rowKey);
            TableResult result = mCloudTable.Execute(retrieveOperation);
            return result.Result as T;
        }

        public void InsertOrReplaceEntity<T>(T entity) where T : TableEntity
        {
            TableOperation insertOrReplaceOperation = TableOperation.InsertOrReplace(entity);
            mCloudTable.Execute(insertOrReplaceOperation);
        }

        public void DeleteEntity<T>(T entity) where T : TableEntity
        {
            TableOperation deleteOperation = TableOperation.Delete(entity);
            mCloudTable.Execute(deleteOperation);
        }

        #endregion
    }
}
