using Aspire.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Identity.Client.Extensions.Msal;

var builder = DistributedApplication.CreateBuilder(args);

// Enable/Disable the components which needs to be tested

#region Day 2 MSSQL

var sql = builder.AddSqlServer("sql")
                 .AddDatabase("sqldata");

#endregion

#region Day 3 PGSQL
var postgres = builder.AddPostgres("postgres")
                      .AddDatabase("pgsqldata");

#endregion

#region Day 4 Oracle

var oracle = builder.AddOracle("oracle")
                    .AddDatabase("oracledb");

#endregion

#region Day 5 Apache Kafka

var messaging = builder.AddKafka("messaging")
                       .WithKafkaUI();

#endregion

#region Day 6 Redis Cache

var cache = builder.AddRedis("cache");

#endregion

#region Day 7 Azure Blog Storage

var storage = builder.AddAzureStorage("storage");
var blobs = storage.AddBlobs("blobs");

if (builder.Environment.IsDevelopment())
{
    storage.RunAsEmulator(c => c.WithImageTag("3.31.0"));
}

#endregion


var apiService = builder.AddProject<Projects.DotnetAspireChallenge_ApiService>("apiservice")
    .WithReference(messaging)
    .WithReference(sql)
    .WithReference(blobs)
    .WithReference(postgres)
    .WithReference(oracle);



builder.AddProject<Projects.DotnetAspireChallenge_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(cache)
    .WithReference(apiService)
    .WithReference(messaging);


builder.Build().Run();
