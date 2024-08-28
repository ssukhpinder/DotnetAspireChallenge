

using Azure.Storage.Blobs;
using Confluent.Kafka;
using DotnetAspireChallenge.ApiService.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client.Extensions.Msal;
using System.ComponentModel.DataAnnotations;

var builder = WebApplication.CreateBuilder(args);

builder.AddKafkaProducer<string, string>("messaging");

builder.AddSqlServerDbContext<MssqlDbContext>("sqldata");

builder.AddNpgsqlDbContext<PgsqlDbContext>("pgsqldata");

builder.AddOracleDatabaseDbContext<OracleDbContext>("oracledb");

builder.AddAzureBlobClient("blobs");

builder.AddAzureQueueClient("queues");

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
app.MapPgsqlAspireEndpoint();
app.MapOracleAspireEndpoint();
app.MapAzureBlobStorageEndpoint();
app.MapAzureQueueEndpoint();

app.MapDefaultEndpoints();

app.Run();