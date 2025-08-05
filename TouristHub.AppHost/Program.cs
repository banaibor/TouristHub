var builder = DistributedApplication.CreateBuilder(args);

// Register the apiservice project with non-conflicting ports
var apiService = builder.AddProject<Projects.TouristHub_ApiService>("apiservice")
    .WithHttpEndpoint(port: 5500, name: "api-http")  // Changed from 5499 to 5500
    .WithHttpsEndpoint(port: 7574, name: "api-https"); // Changed from 7573 to 7574

// Pass API_BASE_URL to MAUI and Web using the HTTP endpoint
var mauiApp = builder.AddProject<Projects.MAUI>("mauihybrid")
    .WithEnvironment("API_BASE_URL", apiService.GetEndpoint("api-http"));

builder.AddProject<Projects.TouristHub_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(mauiApp)
    .WithEnvironment("API_BASE_URL", apiService.GetEndpoint("api-http"))
    .WaitFor(apiService);

builder.Build().Run();