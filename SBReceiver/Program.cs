using Microsoft.Azure.ServiceBus;
using SBShared.Models;
using System;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace SBReceiver
{
    internal class Program
    {
        private const string connectionString = "";
        private const string queueName = "person-queue";
        private static IQueueClient queueClient;

        private static async Task Main(string[] args)
        {
            queueClient = new QueueClient(connectionString, queueName);

            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                // Wait until message is read successfully before marking complete.
                AutoComplete = false,

                // Only process 1 message at a time to keep demo simple.
                MaxConcurrentCalls = 1,
            };

            queueClient.RegisterMessageHandler(ProcessMessagesAsync, messageHandlerOptions);

            Console.ReadLine();

            await queueClient.CloseAsync();
        }

        private static async Task ProcessMessagesAsync(Message message, CancellationToken token)
        {
            var jsonString = Encoding.UTF8.GetString(message.Body);
            var person = JsonSerializer.Deserialize<PersonModel>(jsonString);

            Console.WriteLine($"Person received: {person.FirstName} {person.LastName}");

            // Because AutoComplete is set to false.
            await queueClient.CompleteAsync(message.SystemProperties.LockToken);
        }

        private static Task ExceptionReceivedHandler(ExceptionReceivedEventArgs arg)
        {
            Console.WriteLine($"Message handler exception: {arg.Exception}");
            return Task.CompletedTask;
        }
    }
}
