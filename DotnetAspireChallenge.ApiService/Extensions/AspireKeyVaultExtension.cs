using Azure;
using Azure.Security.KeyVault.Secrets;
using Azure.Storage.Blobs;
using Confluent.Kafka;

namespace DotnetAspireChallenge.ApiService.Extensions
{
    public static class AspireKeyVaultExtension
    {

        public static void MapKeyVaultEndpoint(this WebApplication app)
        {
            app.MapGet("/vault", async (SecretClient secretClient) =>
            {

                try
                {
                    // Define the secret name and value
                    string secretName = "mySecret";
                    string secretValue = "This is a secret value";

                    // Set the secret
                    KeyVaultSecret secret = new KeyVaultSecret(secretName, secretValue);

                    await secretClient.SetSecretAsync(secret);
                    return Results.Ok(await secretClient.GetSecretAsync(secretName));

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
