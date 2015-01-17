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
    class ProgramMultipleQueuesAsync
    {
        static IStorageQueue queue1;
        static IStorageQueue queue2;
        static Timer timer1;
        static Timer timer2;

        static void Main(string[] args)
        {
            // Configure the storage service
            StorageServiceConfigurator.Configure(CloudConfigurationManager.GetSetting("StorageConnectionString"));
            Console.WriteLine("Storage Service configured.");

            Task.Run(() => ProcessQeueu1());
            Task.Run(() => ProcessQeueu2());

            while (true) { }
        }

        static async void ProcessQeueu1()
        {
            // Setup a timer to add messages on an interval faster than we process them
            timer1 = new Timer(7000);
            timer1.Elapsed += timer_Elapsed1;

            await CreateGetQueueTest("test");
            var message = await AddMessageToQueueTest("This is a message to test");

            queue1.MessagesToProcess += queue_MessagesToProcess1;

            // This should now work because we are async
            StartMonitorQueue1();
            timer1.Start();
        }

        static async void ProcessQeueu2()
        {
            // Setup a timer to add messages on an interval faster than we process them
            timer2 = new Timer(7000);
            timer2.Elapsed += timer_Elapsed2;

            await CreateGetQueueSecond("second");
            var message = await AddMessageToQueueSecond("I'm a message in the second queue");

            queue2.MessagesToProcess += queue_MessagesToProcess2;

            // This should now work because we are async
            StartMonitorQueue2();
            timer2.Start();
        }

        static async void StartMonitorQueue1()
        {
            await queue1.StartMonitorAsync(new QueueMonitorConfiguration(5000, 60000, 5000));
        }

        static async void StartMonitorQueue2()
        {
            await queue2.StartMonitorAsync(new QueueMonitorConfiguration(5000, 60000, 5000));
        }

        static async Task CreateGetQueueTest(string queueName)
        {
            queue1 = await StorageService.Instance.GetQueueAsync(queueName);
            Console.WriteLine(string.Format("Retrieved queue '{0}'", queueName));
        }

        static async Task CreateGetQueueSecond(string queueName)
        {
            queue2 = await StorageService.Instance.GetQueueAsync(queueName);
            Console.WriteLine(string.Format("Retrieved queue '{0}'", queueName));
        }

        static async Task<Microsoft.WindowsAzure.Storage.Queue.CloudQueueMessage> AddMessageToQueueTest(string message)
        {
            var qMessage = new Microsoft.WindowsAzure.Storage.Queue.CloudQueueMessage(message);
            await queue1.AddMessageAsync(qMessage);
            Console.WriteLine(string.Format("Added queue message '{0}' to queue 'test'", message));
            return qMessage;
        }

        static async Task<Microsoft.WindowsAzure.Storage.Queue.CloudQueueMessage> AddMessageToQueueSecond(string message)
        {
            var qMessage = new Microsoft.WindowsAzure.Storage.Queue.CloudQueueMessage(message);
            await queue2.AddMessageAsync(qMessage);
            Console.WriteLine(string.Format("Added queue message '{0}' to queue 'second'", message));
            return qMessage;
        }



        static async void timer_Elapsed1(object sender, ElapsedEventArgs e)
        {
            // Add a few messages each interval
            await AddMessageToQueueTest("This is message one of the interval");
            await AddMessageToQueueTest("This is message two of the interval");
            await AddMessageToQueueTest("This is message three of the interval");
        }

        static async void timer_Elapsed2(object sender, ElapsedEventArgs e)
        {
            // Add a few messages each interval
            await AddMessageToQueueSecond("This is message one of the interval for the second queue");
            await AddMessageToQueueSecond("This is message two of the interval for the second queue");
            await AddMessageToQueueSecond("This is message three of the interval for the second queue");
        }

        static void queue_MessagesToProcess1(object sender, AzureStorageService.EventArgs.ProcessQueueEventArgs e)
        {
            foreach (var message in e.Messages)
            {
                Console.WriteLine(string.Format("Processing message '{0}", message.AsString));
                queue1.DeleteMessage(message);
                Console.WriteLine(string.Format("Deleted message '{0}", message.AsString));
            }
        }

        static void queue_MessagesToProcess2(object sender, AzureStorageService.EventArgs.ProcessQueueEventArgs e)
        {
            foreach (var message in e.Messages)
            {
                Console.WriteLine(string.Format("Processing message '{0}", message.AsString));
                queue2.DeleteMessage(message);
                Console.WriteLine(string.Format("Deleted message '{0}", message.AsString));
            }
        }

        

        

        static async Task DeleteMessageFromQueue1(Microsoft.WindowsAzure.Storage.Queue.CloudQueueMessage message)
        {
            await queue1.DeleteMessageAsync(message);
            Console.WriteLine("Deleted message from queue");
        }

        static async Task DeleteMessageFromQueue2(Microsoft.WindowsAzure.Storage.Queue.CloudQueueMessage message)
        {
            await queue2.DeleteMessageAsync(message);
            Console.WriteLine("Deleted message from queue");
        }
    }
}
