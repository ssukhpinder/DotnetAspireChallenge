

using Confluent.Kafka;
using DotnetAspireChallenge.ApiService.Extensions;
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


app.MapForecastEndpoint();
app.MapAspireKafkaEndpoint();
app.MapMssqlAspireEndpoint();

app.MapDefaultEndpoints();

app.Run();




