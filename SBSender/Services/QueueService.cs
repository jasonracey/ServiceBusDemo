using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SBSender.Services
{
    public class QueueService : IQueueService
    {
        private readonly IConfiguration config;

        public QueueService(IConfiguration config)
        {
            this.config = config;
        }

        public async Task SendMessageAsync<T>(T messageObject, string queueName)
        {
            var queueClient = new QueueClient(config.GetConnectionString("AzureServiceBus"), queueName);
            var messageJson = JsonSerializer.Serialize(messageObject);
            var message = new Message(Encoding.UTF8.GetBytes(messageJson));
            await queueClient.SendAsync(message);
        }
    }
}
