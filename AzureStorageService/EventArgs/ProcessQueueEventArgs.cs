using Microsoft.WindowsAzure.Storage.Queue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureStorageService.EventArgs
{
    public class ProcessQueueEventArgs : System.EventArgs
    {
        #region Declarations

        private IList<CloudQueueMessage> mMessages;

        #endregion

        #region Constructors

        internal ProcessQueueEventArgs(IList<CloudQueueMessage> messages)
        {
            this.mMessages = messages;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets messages to be processed
        /// </summary>
        public IEnumerable<CloudQueueMessage> Messages { get { return mMessages; } }

        #endregion
    }
}
