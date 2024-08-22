using Aspire.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache");

var messaging = builder.AddKafka("messaging")
                       .WithKafkaUI();

var apiService = builder.AddProject<Projects.DotnetAspireChallenge_ApiService>("apiservice").WithReference(messaging);



builder.AddProject<Projects.DotnetAspireChallenge_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(cache)
    .WithReference(apiService)
    .WithReference(messaging);


builder.Build().Run();
