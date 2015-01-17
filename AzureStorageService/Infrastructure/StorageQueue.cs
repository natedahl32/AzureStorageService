using AzureStorageService.EventArgs;
using Microsoft.WindowsAzure.Storage.Queue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AzureStorageService.Infrastructure
{
    public class StorageQueue : IStorageQueue
    {
        #region Events

        public event EventHandler<ProcessQueueEventArgs> MessagesToProcess;

        #endregion

        #region Declarations

        private const int MAX_GET_QUEUE = 32;

        private readonly CloudQueue mCloudQueue;

        private string mName;
        private int mBackoff;
        private int mMinBackoff;
        private int mMaxBackoff;
        private int mBackoffIncrement;
        private bool mMonitorRunning = false;

        #endregion

        #region Constructors

        public StorageQueue(CloudQueue cloudQueue, string name)
        {
            mCloudQueue = cloudQueue;
            mName = name;
            mMinBackoff = -1;
            mMaxBackoff = -1;
            mBackoffIncrement = -1;
        }

        #endregion

        #region Properties

        public string QueueName { get { return mName; } }

        public int MessageCount { get { return mCloudQueue.ApproximateMessageCount.HasValue ? mCloudQueue.ApproximateMessageCount.Value : 0; } }

        public int Backoff
        {
            get { return mBackoff; }
            set
            {
                mBackoff = value > mMaxBackoff ? mMaxBackoff : value;
                mBackoff = mBackoff < mMinBackoff ? mMinBackoff : mBackoff;
            }
        }

        public int MinBackoff
        {
            get { return mMinBackoff; }
            set { mMinBackoff = value; }
        }

        public int MaxBackoff
        {
            get { return mMaxBackoff; }
            set { mMaxBackoff = value; }
        }

        public int BackoffIncrement
        {
            get { return mBackoffIncrement; }
            set { mBackoffIncrement = value; }
        }

        public bool IsMonitorRunning { get { return mMonitorRunning; } }

        #endregion

        #region Public Methods

        public void StartMonitor()
        {
            this.StartMonitor(new QueueMonitorConfiguration(mMinBackoff, mMaxBackoff, mBackoffIncrement));
        }

        public void StartMonitor(QueueMonitorConfiguration config)
        {
            if (config.MinBackoff == -1 || config.MaxBackoff == -1 || config.BackoffIncrement == -1)
                throw new InvalidOperationException("You must set a minimum backoff, maximum backoff, and backoff increment to start monitoring a queue");

            // Set our backoff values from the config
            mMinBackoff = config.MinBackoff;
            mMaxBackoff = config.MaxBackoff;
            mBackoffIncrement = config.BackoffIncrement;

            mMonitorRunning = true;
            var noMessageCount = 0;
            while (mMonitorRunning)
            {
                Thread.Sleep(this.Backoff);

                var messages = GetMessages(TimeSpan.FromMinutes(5));
                if (messages.Count > 0)
                {
                    this.Backoff = mMinBackoff;

                    while (messages.Count != 0)
                    {
                        //process messages
                        OnMessagesToProcess(this, new ProcessQueueEventArgs(messages));
                        messages = GetMessages(TimeSpan.FromMinutes(5));
                    }
                    noMessageCount = 0;
                }
                else
                {
                    if (++noMessageCount > 1)
                    {
                        Backoff += mBackoffIncrement;
                        noMessageCount = 0;
                    }
                }
            }

        }

        public async Task StartMonitorAsync()
        {
            await this.StartMonitorAsync(new QueueMonitorConfiguration(mMinBackoff, mMaxBackoff, mBackoffIncrement));
        }

        public async Task StartMonitorAsync(QueueMonitorConfiguration config)
        {
            if (config.MinBackoff == -1 || config.MaxBackoff == -1 || config.BackoffIncrement == -1)
                throw new InvalidOperationException("You must set a minimum backoff, maximum backoff, and backoff increment to start monitoring a queue");

            // Set our backoff values from the config
            mMinBackoff = config.MinBackoff;
            mMaxBackoff = config.MaxBackoff;
            mBackoffIncrement = config.BackoffIncrement;

            mMonitorRunning = true;
            var noMessageCount = 0;
            while (mMonitorRunning)
            {
                await Task.Delay(this.Backoff);

                var messages = await GetMessagesAsync(TimeSpan.FromMinutes(5));
                if (messages.Count > 0)
                {
                    while (messages.Count != 0)
                    {
                        //process messages
                        OnMessagesToProcess(this, new ProcessQueueEventArgs(messages));
                        messages = await GetMessagesAsync(TimeSpan.FromMinutes(5));
                    }
                    noMessageCount = 0;
                }
                else
                {
                    if (++noMessageCount > 1)
                    {
                        Backoff += mBackoffIncrement;
                        noMessageCount = 0;
                    }
                }
            }
        }

        public void AddMessage(CloudQueueMessage message, TimeSpan? timeToLive = null)
        {
            mCloudQueue.AddMessage(message, timeToLive);
        }

        public async Task AddMessageAsync(CloudQueueMessage message, TimeSpan? timeToLive = null)
        {
            await mCloudQueue.AddMessageAsync(message, timeToLive, null, null, null);
        }

        public void UpdateMessage(CloudQueueMessage message, TimeSpan timeoutVisibility, MessageUpdateFields updateFields)
        {
            mCloudQueue.UpdateMessage(message, timeoutVisibility, updateFields);
        }

        public async Task UpdateMessageAsync(CloudQueueMessage message, TimeSpan timeoutVisibility, MessageUpdateFields updateFields)
        {
            await mCloudQueue.UpdateMessageAsync(message, timeoutVisibility, updateFields);
        }

        public void DeleteMessage(CloudQueueMessage message)
        {
            mCloudQueue.DeleteMessage(message);
        }

        public async Task DeleteMessageAsync(CloudQueueMessage message)
        {
            await mCloudQueue.DeleteMessageAsync(message);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets messages from the queue
        /// </summary>
        /// <param name="queueLength">Number of messages to retrieve from queue</param>
        /// <param name="visibilityTimeout"></param>
        /// <returns></returns>
        private List<CloudQueueMessage> GetMessages(TimeSpan visibilityTimeout)
        {
            var messages = mCloudQueue.GetMessages(MAX_GET_QUEUE, visibilityTimeout);
            if (messages != null)
            {
                return messages.ToList();
            }

            return null;
        }

        /// <summary>
        /// Gets messages from the queue asynchronously
        /// </summary>
        /// <param name="queueLength">Number of messages to retrieve from queue</param>
        /// <param name="visibilityTimeout"></param>
        /// <returns></returns>
        private async Task<List<CloudQueueMessage>> GetMessagesAsync(TimeSpan visibilityTimeout)
        {
            var messages = await mCloudQueue.GetMessagesAsync(MAX_GET_QUEUE, visibilityTimeout, null, null);
            if (messages != null)
            {
                return messages.ToList();
            }

            return null;
        }

        #endregion

        #region Private Methods

        private void OnMessagesToProcess(object sender, ProcessQueueEventArgs e)
        {
            EventHandler<ProcessQueueEventArgs> handler = MessagesToProcess;
            if (handler != null)
                handler(sender, e);
        }

        #endregion
    }
}
