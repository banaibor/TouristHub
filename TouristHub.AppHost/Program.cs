var builder = DistributedApplication.CreateBuilder(args);

// Register the apiservice project (default HTTP endpoint is created automatically)
var apiService = builder.AddProject<Projects.TouristHub_ApiService>("apiservice");

// Pass API_BASE_URL to MAUI and Web using the existing "http" endpoint
var mauiApp = builder.AddProject<Projects.MAUI>("mauihybrid")
    .WithEnvironment("API_BASE_URL", apiService.GetEndpoint("http"));

builder.AddProject<Projects.TouristHub_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService)
    .WithReference(mauiApp)
    .WithEnvironment("API_BASE_URL", apiService.GetEndpoint("http"))
    .WaitFor(apiService);

builder.Build().Run();
