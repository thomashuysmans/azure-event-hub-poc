using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ServiceBus.Messaging;

namespace EventHubReceiverPoc
{
    internal class EventProcessor : IEventProcessor
    {
        public Task OpenAsync(PartitionContext context)
        {
            return Task.FromResult(true);
        }

        public async Task ProcessEventsAsync(PartitionContext context, IEnumerable<EventData> messages)
        {

            EventData lastMessage = null;
            //ServiceEventSource.Current.EventProcessorProcessing(context, messages);

            try
            {
                foreach (var message in messages)
                {
                    var payload = Encoding.UTF8.GetString(message.GetBytes());
                    
                    if (payload == "Event 74")
                    {
                        Console.WriteLine($"Something with handling {payload}");
                        throw new Exception("Something went wrong.");
                    }
                    
                    Console.WriteLine(payload);
                    lastMessage = message;
                }

                if (lastMessage != null && context != null)
                {
                    await context.CheckpointAsync(lastMessage);
                }

            }
            catch (Exception ex)
            {
                throw;
            }

            await Task.FromResult(true);
        }

        public Task CloseAsync(PartitionContext context, CloseReason reason)
        {
            return Task.FromResult(true);
        }
    }
}
