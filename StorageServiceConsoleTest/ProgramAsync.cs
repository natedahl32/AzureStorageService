using AzureStorageService;
using AzureStorageService.Infrastructure;
using Microsoft.WindowsAzure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace StorageServiceConsoleTest
{
    class ProgramAsync
    {
        static IStorageQueue queue;
        static Timer timer;

        static void Main(string[] args)
        {
            // Configure the storage service
            StorageServiceConfigurator.Configure(CloudConfigurationManager.GetSetting("StorageConnectionString"));
            Console.WriteLine("Storage Service configured.");

            Task.Run(async () =>
            {
                // Setup a timer to add messages on an interval faster than we process them
                timer = new Timer(7000);
                timer.Elapsed += timer_Elapsed;

                await CreateGetQueueTest("test");
                var message = await AddMessageToQueueTest("This is a message to test");

                queue.MessagesToProcess += queue_MessagesToProcess;

                // This should now work because we are async
                StartMonitor();
                timer.Start();

                Console.ReadLine();
            }).Wait();
        }

        static async void StartMonitor()
        {
            await queue.StartMonitorAsync(new QueueMonitorConfiguration(5000, 60000, 5000));
        }

        static async void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            // Add a few messages each interval
            await AddMessageToQueueTest("This is message one of the interval");
            await AddMessageToQueueTest("This is message two of the interval");
            await AddMessageToQueueTest("This is message three of the interval");
        }

        static void queue_MessagesToProcess(object sender, AzureStorageService.EventArgs.ProcessQueueEventArgs e)
        {
            foreach (var message in e.Messages)
            {
                Console.WriteLine(string.Format("Processing message '{0}", message.AsString));
                queue.DeleteMessage(message);
                Console.WriteLine(string.Format("Deleted message '{0}", message.AsString));
            }
        }

        static async Task CreateGetQueueTest(string queueName)
        {
            queue = await StorageService.Instance.GetQueueAsync(queueName);
            Console.WriteLine(string.Format("Retrieved queue '{0}'", queueName));
        }

        static async Task<Microsoft.WindowsAzure.Storage.Queue.CloudQueueMessage> AddMessageToQueueTest(string message)
        {
            var qMessage = new Microsoft.WindowsAzure.Storage.Queue.CloudQueueMessage(message);
            await queue.AddMessageAsync(qMessage);
            Console.WriteLine(string.Format("Added queue message '{0}'", message));
            return qMessage;
        }

        static async Task DeleteMessageFromQueue(Microsoft.WindowsAzure.Storage.Queue.CloudQueueMessage message)
        {
            await queue.DeleteMessageAsync(message);
            Console.WriteLine("Deleted message from queue");
        }
    }
}
