using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureStorageService.Infrastructure
{
    public interface ITable
    {
        #region Properties

        /// <summary>
        /// Gets the table name
        /// </summary>
        string TableName { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Adds an entity to the table
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        void AddEntity<T>(T entity) where T : TableEntity;

        /// <summary>
        /// Retrieves a single entity from table storage
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="partitionKey"></param>
        /// <param name="rowKey"></param>
        /// <returns></returns>
        T RetrieveEntity<T>(string partitionKey, string rowKey) where T : TableEntity;

        /// <summary>
        /// Inserts the entity if it does not exist in the table. Overwrites all properties of the entity if already exists.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        void InsertOrReplaceEntity<T>(T entity) where T : TableEntity;

        /// <summary>
        /// Deletes an entity from the table
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        void DeleteEntity<T>(T entity) where T : TableEntity;

        #endregion
    }
}
