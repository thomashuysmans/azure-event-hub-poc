using System;
using System.Text;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Consumer;
using Azure.Messaging.EventHubs.Processor;

// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

var ehubNamespaceConnectionString = "Endpoint=sb://kaizenit.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=4+FxOvzDqdv5giZtXa8qCYbKHWtBcvleCfh8urEogXk=";
var eventHubName = "myfirsteventhub";
var blobStorageConnectionString = "DefaultEndpointsProtocol=https;AccountName=kaizenitstorage;AccountKey=N/0hcL0SAZVCmPWoTWC/tYDmM7Da8sQ55qN4vI+QDDyUGwecl1Ff9qUFhm/sSQtqOtKBamF/oILe2uoXsIefZA==;EndpointSuffix=core.windows.net";
var blobContainerName = "eventhubcontainer";

var consumerGroup = EventHubConsumerClient.DefaultConsumerGroupName;
var storageClient = new BlobContainerClient(blobStorageConnectionString, blobContainerName);

var eventProcessorClient = new EventProcessorClient(storageClient, consumerGroup, ehubNamespaceConnectionString,
    eventHubName);


eventProcessorClient.ProcessEventAsync += async eventArgs =>
{
    var payload = Encoding.UTF8.GetString(eventArgs.Data.Body.ToArray());
    //eventArgs.Partition.PartitionId
    if (payload == "Event 74")
    {
        Console.WriteLine($"Something with handling { payload }");
        
        throw new Exception("Something went wrong.");
    }

    // Write the body of the event to the console window
    Console.WriteLine("\tReceived event: {0}", payload );
    
    // Update checkpoint in the blob storage so that the app receives only new events the next time it's run
    await eventArgs.UpdateCheckpointAsync(eventArgs.CancellationToken);
};

eventProcessorClient.ProcessErrorAsync += eventArgs => {
    // Write details about the error to the console window
    Console.WriteLine($"\tPartition '{ eventArgs.PartitionId}': an unhandled exception was encountered. This was not expected to happen.");
    Console.WriteLine(eventArgs.Exception.Message);
    return Task.CompletedTask;
};

do
{
    try
    {
        Console.WriteLine("Start processing events.");
        await eventProcessorClient.StartProcessingAsync();

        await Task.Delay(TimeSpan.FromSeconds(15));

        await eventProcessorClient.StopProcessingAsync();
        Console.WriteLine("Processing Events finished.");
        Console.ReadLine();
    }
    catch (Exception e)
    {
        Console.WriteLine(e);
        throw;
    }
    

} while (Console.ReadKey().KeyChar != 'q');


