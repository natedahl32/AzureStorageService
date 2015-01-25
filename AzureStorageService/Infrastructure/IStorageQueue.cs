using AzureStorageService.EventArgs;
using Microsoft.WindowsAzure.Storage.Queue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureStorageService.Infrastructure
{
    public interface IStorageQueue
    {
        #region Events

        event EventHandler<ProcessQueueEventArgs> MessagesToProcess;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the queue name
        /// </summary>
        string QueueName { get; }

        /// <summary>
        /// Gets the approximate message count in the queue
        /// </summary>
        int MessageCount { get; }

        /// <summary>
        /// Gets the backoff amount if no messages are in queue
        /// </summary>
        int Backoff { get; }

        /// <summary>
        /// Gets or sets the minimum backoff time allowed.
        /// </summary>
        int MinBackoff { get; set; }

        /// <summary>
        /// Gets or sets the maximum backoff time allowed.
        /// </summary>
        int MaxBackoff { get; set; }

        /// <summary>
        /// Gets or sets the backoff increment used
        /// </summary>
        int BackoffIncrement { get; set; }

        /// <summary>
        /// Gets whether or not the monitor is currently running
        /// </summary>
        bool IsMonitorRunning { get; }

        /// <summary>
        /// Gets or sets the batch size of messages to try and retrieve from the queue. Default is 16. Maximum is 32.
        /// </summary>
        int BatchSize { get; set; }

        /// <summary>
        /// Gets or sets the amount of time a message is hidden in the queue after it is received. Default and maximum is 5 minutes.
        /// </summary>
        TimeSpan HideMessageTime { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Starts monitoring the queue for messages
        /// </summary>
        void StartMonitor();

        /// <summary>
        /// Starts monitoring the queue for messages asynchronously
        /// </summary>
        Task StartMonitorAsync();

        /// <summary>
        /// Starts monitoring the queue for messages
        /// </summary>
        void StartMonitor(QueueMonitorConfiguration config);

        /// <summary>
        /// Starts monitoring the queue for messages asynchronously
        /// </summary>
        Task StartMonitorAsync(QueueMonitorConfiguration config);

        /// <summary>
        /// Add a message to the queue
        /// </summary>
        /// <param name="message"></param>
        /// <param name="timeToLive"></param>
        void AddMessage(CloudQueueMessage message, TimeSpan? timeToLive = null);

        /// <summary>
        /// Add a message to the queue asynchronously
        /// </summary>
        /// <param name="message"></param>
        /// <param name="timeToLive"></param>
        Task AddMessageAsync(CloudQueueMessage message, TimeSpan? timeToLive = null);

        /// <summary>
        /// Updates a message in the queue
        /// </summary>
        /// <param name="message"></param>
        /// <param name="timeoutVisibility"></param>
        /// <param name="updateFields"></param>
        void UpdateMessage(CloudQueueMessage message, TimeSpan timeoutVisibility, MessageUpdateFields updateFields);

        /// <summary>
        /// Updates a message in the queue asynchronously
        /// </summary>
        /// <param name="message"></param>
        /// <param name="timeoutVisibility"></param>
        /// <param name="updateFields"></param>
        /// <returns></returns>
        Task UpdateMessageAsync(CloudQueueMessage message, TimeSpan timeoutVisibility, MessageUpdateFields updateFields);

        /// <summary>
        /// Delete a message from the queue
        /// </summary>
        /// <param name="message"></param>
        void DeleteMessage(CloudQueueMessage message);

        /// <summary>
        /// Delete a message from the queue asynchronously
        /// </summary>
        /// <param name="message"></param>
        Task DeleteMessageAsync(CloudQueueMessage message);

        #endregion
    }
}
