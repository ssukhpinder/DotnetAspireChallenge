using Azure;
using Azure.Storage.Blobs;

namespace DotnetAspireChallenge.ApiService.Extensions
{
    public static class AspireAzureBlobExtension
    {
        public static void MapAzureBlobStorageEndpoint(this WebApplication app)
        {
            app.MapPost("/create-images-container", async (BlobServiceClient blobServiceClient) =>
            {
                string containerName = "images-container";

                try
                {
                    BlobContainerClient container = await blobServiceClient.CreateBlobContainerAsync(containerName);

                    if (await container.ExistsAsync())
                    {
                        return Results.Ok(container);
                    }
                }
                catch (RequestFailedException e)
                {
                    Console.WriteLine("HTTP error code {0}: {1}", e.Status, e.ErrorCode);
                    Console.WriteLine(e.Message);
                    return Results.Problem($"HTTP error code {e.Status}: {e.Message}");
                }

                return Results.NotFound("Container creation failed or it does not exist.");
            });
        }
    }
}
