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
    class Program
    {
        static IStorageQueue queue;
        static Timer timer;

        static void Main(string[] args)
        {
            // Setup a timer to add messages on an interval faster than we process them
            timer = new Timer(7000);
            timer.Elapsed += timer_Elapsed;

            // Configure the storage service
            StorageServiceConfigurator.Configure(CloudConfigurationManager.GetSetting("StorageConnectionString"));

            Console.WriteLine("Storage Service configured.");

            CreateGetQueueTest("test");
            var message = AddMessageToQueueTest("This is a message to test");

            queue.MessagesToProcess += queue_MessagesToProcess;

            // This works fine here. Our timer is automaticaly asynchronous so it starts and adds messages when the interval passes
            timer.Start();
            queue.StartMonitor(new QueueMonitorConfiguration(5000, 60000, 5000));
            // This does not work here. It will never be hit because our queue.StartMonitor is run synchronously. See ProgramAsync for a working version
            //timer.Start();

            Console.ReadLine();
        }

        static void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            // Add a few messages each interval
            AddMessageToQueueTest("This is message one of the interval");
            AddMessageToQueueTest("This is message two of the interval");
            AddMessageToQueueTest("This is message three of the interval");
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

        static void CreateGetQueueTest(string queueName)
        {
            queue = StorageService.Instance.GetQueue(queueName);
            Console.WriteLine(string.Format("Retrieved queue '{0}'", queueName));
        }

        static Microsoft.WindowsAzure.Storage.Queue.CloudQueueMessage AddMessageToQueueTest(string message)
        {
            var qMessage = new Microsoft.WindowsAzure.Storage.Queue.CloudQueueMessage(message);
            queue.AddMessage(qMessage);
            Console.WriteLine(string.Format("Added queue message '{0}'", message));
            return qMessage;
        }

        static void DeleteMessageFromQueue(Microsoft.WindowsAzure.Storage.Queue.CloudQueueMessage message)
        {
            queue.DeleteMessage(message);
            Console.WriteLine("Deleted message from queue");
        }
    }
}
