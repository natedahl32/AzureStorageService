using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureStorageService
{
    public class QueueMonitorConfiguration
    {
        #region Constructors

        public QueueMonitorConfiguration(int minBackoff, int maxBackoff, int backoffIncrement)
        {
            this.MinBackoff = minBackoff;
            this.MaxBackoff = maxBackoff;
            this.BackoffIncrement = backoffIncrement;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the minimum backoff time used for queue monitoring
        /// </summary>
        public int MinBackoff
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the maximum backoff time used for queue monitoring
        /// </summary>
        public int MaxBackoff
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the amount of time the backoff increments when no messages are found while monitoring the queue
        /// </summary>
        public int BackoffIncrement
        {
            get;
            private set;
        }

        #endregion
    }
}
