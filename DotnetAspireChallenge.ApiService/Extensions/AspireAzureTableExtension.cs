using Azure.Data.Tables;
using Azure;
using Azure.Storage.Blobs;

namespace DotnetAspireChallenge.ApiService.Extensions
{
    public static class AspireAzureTableExtension
    {
        public static void MapAzureTableStorageEndpoint(this WebApplication app)
        {
            app.MapPost("/create-table", async (TableServiceClient tableServiceClient) =>
            {
                string tableName = "MyTable";
                try
                {
                    TableClient tableClient = tableServiceClient.GetTableClient(tableName);
                    await tableClient.CreateIfNotExistsAsync();

                    return Results.Ok($"Table '{tableName}' created successfully.");

                }
                catch (RequestFailedException e)
                {
                    Console.WriteLine("HTTP error code {0}: {1}", e.Status, e.ErrorCode);
                    Console.WriteLine(e.Message);
                    return Results.Problem($"HTTP error code {e.Status}: {e.Message}");
                }

                return Results.NotFound("Table creation failed or it does not exist.");
            });
        }
    }
}
