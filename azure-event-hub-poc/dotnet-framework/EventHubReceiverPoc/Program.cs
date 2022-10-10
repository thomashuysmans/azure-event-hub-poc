using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ServiceBus.Messaging;

namespace EventHubReceiverPoc
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Start the application:");

            var eventHubConnectionString = ""; 
            var eventHubName = "";
            var consumerGroupName = "";
            var blobStorageConnectionString = "";

            var eventProcessorHost = new EventProcessorHost(eventHubPath:eventHubName, 
                consumerGroupName:consumerGroupName, eventHubConnectionString: eventHubConnectionString, storageConnectionString: blobStorageConnectionString);

            eventProcessorHost.RegisterEventProcessorAsync<EventProcessor>(new EventProcessorOptions()
            {
                MaxBatchSize = 32
            });

            
            Console.ReadLine();
        }
    }
}
