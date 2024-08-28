using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Queues;
using Confluent.Kafka;

namespace DotnetAspireChallenge.ApiService.Extensions
{
    public static class AspireAzureQueueExtension
    {
        public static void MapAzureQueueEndpoint(this WebApplication app)
        {
            app.MapGet("/queue-send", async (QueueServiceClient queueServiceClient) =>
            {
                try
                {
                    var queueClient = queueServiceClient.GetQueueClient("test");
                    await queueClient.CreateIfNotExistsAsync();

                    if (await queueClient.ExistsAsync())
                    {
                        await queueClient.SendMessageAsync("Test Message ");
                        return Results.Ok($"Message sent to queue: test");
                    }
                    return Results.NotFound($"Queue not found: test");
                }
                catch (RequestFailedException e)
                {
                    Console.WriteLine("HTTP error code {0}: {1}", e.Status, e.ErrorCode);
                    Console.WriteLine(e.Message);
                    return Results.Problem($"HTTP error code {e.Status}: {e.Message}");
                }
            });


            app.MapGet("/queue-recieve", async (QueueServiceClient queueServiceClient) =>
            {
                try
                {
                    var queueClient = queueServiceClient.GetQueueClient("test");
                    if (await queueClient.ExistsAsync())
                    {
                        var response = await queueClient.ReceiveMessageAsync();
                        if (response?.Value != null)
                        {
                            var message = response.Value;
                            // Delete the message after processing
                            await queueClient.DeleteMessageAsync(message.MessageId, message.PopReceipt);
                            return Results.Ok($"Received message: {message.MessageText}");
                        }
                        return Results.Ok("No messages in the queue.");
                    }
                    return Results.NotFound($"Queue not found: test");
                }
                catch (RequestFailedException e)
                {
                    Console.WriteLine("HTTP error code {0}: {1}", e.Status, e.ErrorCode);
                    Console.WriteLine(e.Message);
                    return Results.Problem($"HTTP error code {e.Status}: {e.Message}");
                }
            });
        }
    }
}
