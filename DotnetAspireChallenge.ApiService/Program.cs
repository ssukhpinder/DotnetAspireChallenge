

using Confluent.Kafka;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

var builder = WebApplication.CreateBuilder(args);

builder.AddKafkaProducer<string, string>("messaging");

builder.AddSqlServerDbContext<MssqlDbContext>("sqldata");

// Add service defaults & Aspire components.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddProblemDetails();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
});

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

app.MapGet("/mssql", async (MssqlDbContext mssqlDbContext) =>
{

    await mssqlDbContext.Customers.AddAsync(new Customer()
    {
        Title = "test@gmail.com",
        Description = "sukh"
    });
    int rows = await mssqlDbContext.SaveChangesAsync();
    if (rows > 0)
    {
        return await mssqlDbContext.Customers.FirstOrDefaultAsync();
    }
    else
    {
        return null;
    }
});

app.MapDefaultEndpoints();
if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<MssqlDbContext>();
        context.Database.EnsureCreated();
    }
}

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

internal class MssqlDbContext(DbContextOptions options) : DbContext(options)
{

    public DbSet<Customer> Customers => Set<Customer>();
}

public class Customer
{
    public int Id { get; set; }

    [Required]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string Description { get; set; } = string.Empty;
}
