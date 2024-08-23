using Aspire.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache");

var messaging = builder.AddKafka("messaging")
                       .WithKafkaUI();


var sql = builder.AddSqlServer("sql")
                 .AddDatabase("sqldata");


var postgres = builder.AddPostgres("postgres")
                      .AddDatabase("pgsqldata");


var apiService = builder.AddProject<Projects.DotnetAspireChallenge_ApiService>("apiservice")
    .WithReference(messaging)
    .WithReference(sql)
    .WithReference(postgres);



builder.AddProject<Projects.DotnetAspireChallenge_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(cache)
    .WithReference(apiService)
    .WithReference(messaging);


builder.Build().Run();
