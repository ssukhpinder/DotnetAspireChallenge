using Confluent.Kafka;
using static Confluent.Kafka.ConfigPropertyNames;

namespace DotnetAspireChallenge.Web;

public class WeatherApiClient(HttpClient httpClient)
{
    public async Task<WeatherForecast[]> GetWeatherAsync(int maxItems = 10, CancellationToken cancellationToken = default)
    {
        List<WeatherForecast>? forecasts = null;

        await foreach (var forecast in httpClient.GetFromJsonAsAsyncEnumerable<WeatherForecast>("/weatherforecast", cancellationToken))
        {
            if (forecasts?.Count >= maxItems)
            {
                break;
            }
            if (forecast is not null)
            {
                forecasts ??= [];
                forecasts.Add(forecast);
            }
        }

        return forecasts?.ToArray() ?? [];
    }
}

public class KafkaConsumeMessageClient(HttpClient httpClient, IConsumer<string, string> _consumer)
{

    public ConsumeResult<string, string>? GetKafkaMessage(CancellationToken cancellationToken = default)
    {
        ConsumeResult<string, string>? deliveryResult = null;
        _consumer.Subscribe("messaging");
        deliveryResult = _consumer.Consume(TimeSpan.FromSeconds(10));

        return deliveryResult;
    }
}

public record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
