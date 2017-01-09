using System;
using System.Web.Http;
using Microsoft.Azure;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;

namespace FizzBuzzApi.Controllers
{
    public class FizzBuzzController : ApiController
    {
        public void Put(int number)
        {
            WriteToQueue(number);
        }

        private void WriteToQueue(int number)
        {
            var connectionString = CloudConfigurationManager.GetSetting("ServiceBus.ConnectionString");
            var queuePath = "FizzBuzzInput";

            EnsureQueueExists(connectionString, queuePath);

            SendMessage(connectionString, queuePath, number);
        }

        private static void EnsureQueueExists(string connectionString, string queuePath)
        {
            var namespaceManager = NamespaceManager.CreateFromConnectionString(connectionString);

            if (!namespaceManager.QueueExists(queuePath))
            {
                namespaceManager.CreateQueue(queuePath);
//                namespaceManager.CreateTopic(new TopicDescription("") {  DefaultMessageTimeToLive = new TimeSpan(10) });
            }
        }

        private static void SendMessage(string connectionString, string queuePath, int number)
        {
            var message = new BrokeredMessage(number);

            var client = QueueClient.CreateFromConnectionString(connectionString, queuePath);

            client.Send(message);
        }
    }
}
