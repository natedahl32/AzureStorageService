using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AzureStorageService.Infrastructure
{
    internal class TableManager : ITableManager
    {
        #region Declarations

        private const int MIN_TABLE_NAME_LENGTH = 3;
        private const int MAX_TABLE_NAME_LENGTH = 63;

        private readonly ConcurrentDictionary<string, ITable> mTables = new ConcurrentDictionary<string, ITable>();
        private readonly Regex mTableNameRegex = new Regex(@"^[A-Za-z][A-Za-z0-9]{2,62}$", RegexOptions.Singleline);

        private CloudTableClient mClient;
        private bool mIsInitialized = false;

        #endregion

        #region ITableManager Implementations

        public bool IsInitialized
        {
            get { return mIsInitialized; }
        }

        public void Initialize(CloudTableClient client)
        {
            mClient = client;
            mIsInitialized = true;
        }

        public ITable GetTable(string name)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException("name");

            // Sanitize the string
            name = name.Trim().ToLower();

            // Make sure name is valid
            if (!this.IsValidTableName(name))
                throw new ApplicationException(string.Format("Invalid table name {0} was supplied. Table names must contain only letters and numbers; cannot begin with a numeric character; must be between {1} and {2} characters in length.", name, MIN_TABLE_NAME_LENGTH, MAX_TABLE_NAME_LENGTH));

            // Check if table already exists in memory
            if (this.mTables.ContainsKey(name))
                return this.mTables[name];

            // Doesn't exist in memory, get a reference and try to create it if it doesn't exist
            CloudTable cloudTable = mClient.GetTableReference(name);
            cloudTable.CreateIfNotExists();

            // Create a new queue
            Table table = new Table(cloudTable, name);
            mTables.TryAdd(name, table);
            return table;
        }

        public async Task<ITable> GetTableAsync(string name)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException("name");

            // Sanitize the string
            name = name.Trim().ToLower();

            // Make sure name is valid
            if (!this.IsValidTableName(name))
                throw new ApplicationException(string.Format("Invalid table name {0} was supplied. Table names must contain only letters and numbers; cannot begin with a numeric character; must be between {1} and {2} characters in length.", name, MIN_TABLE_NAME_LENGTH, MAX_TABLE_NAME_LENGTH));

            // Check if table already exists in memory
            if (this.mTables.ContainsKey(name))
                return this.mTables[name];

            // Doesn't exist in memory, get a reference and try to create it if it doesn't exist
            CloudTable cloudTable = mClient.GetTableReference(name);
            await cloudTable.CreateIfNotExistsAsync();

            // Create a new queue
            Table table = new Table(cloudTable, name);
            mTables.TryAdd(name, table);
            return table;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets whether or not the passed table name is valid
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private bool IsValidTableName(string name)
        {
            // Length
            if (name.Length < MIN_TABLE_NAME_LENGTH || name.Length > MAX_TABLE_NAME_LENGTH)
                return false;
            // Check against regex
            if (!mTableNameRegex.IsMatch(name))
                return false;

            return true;
        }

        #endregion
    }
}
