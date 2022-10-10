
using System;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;


// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

var connectionString = "";
var eventHubName = "";
var numOfEvents = 100;

do
{
    EventHubProducerClient producerClient = new EventHubProducerClient(connectionString, eventHubName);

    // Create a batch of events 
    using EventDataBatch eventBatch = await producerClient.CreateBatchAsync();

    for (int i = 1; i <= numOfEvents; i++)
    {
        if (!eventBatch.TryAdd(new EventData(Encoding.UTF8.GetBytes($"Event {i}"))))
        {
            // if it is too large for the batch
            throw new Exception($"Event {i} is too large for the batch and cannot be sent.");
        }
    }

    try
    {
        // Use the producer client to send the batch of events to the event hub
        await producerClient.SendAsync(eventBatch);
        Console.WriteLine($"A batch of {numOfEvents} events has been published.");
    }
    finally
    {
        await producerClient.DisposeAsync();
    }
} while (Console.ReadKey().KeyChar != 'q');



Console.ReadLine();