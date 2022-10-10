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

            var eventHubConnectionString = "Endpoint=sb://kaizenit.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=4+FxOvzDqdv5giZtXa8qCYbKHWtBcvleCfh8urEogXk="; ;
            var eventHubName = "myfirsteventhub";
            var consumerGroupName = "$Default";
            var blobStorageConnectionString = "DefaultEndpointsProtocol=https;AccountName=kaizenitstorage;AccountKey=N/0hcL0SAZVCmPWoTWC/tYDmM7Da8sQ55qN4vI+QDDyUGwecl1Ff9qUFhm/sSQtqOtKBamF/oILe2uoXsIefZA==;EndpointSuffix=core.windows.net";

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
