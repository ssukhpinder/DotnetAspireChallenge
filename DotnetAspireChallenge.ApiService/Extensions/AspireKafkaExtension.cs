using Confluent.Kafka;

namespace DotnetAspireChallenge.ApiService.Extensions
{
    public static class AspireKafkaExtension
    {
        public static void MapAspireKafkaEndpoint(this WebApplication app)
        {

            app.MapGet("/send", async (IProducer<string, string> services, string key, string value) =>
            {
                try
                {
                    var message = new Message<string, string> { Key = key, Value = value };
                    DeliveryResult<string, string>? result = await services.ProduceAsync("messaging", message);
                    return result;
                }
                catch (Exception ex)
                {

                    throw;
                }

            });
        }
    }
}
