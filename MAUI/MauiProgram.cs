using Microsoft.Extensions.Logging;
using System.Net.Http;

namespace MAUI
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();

            // Configure HttpClient with proper Android emulator support
            string backendUrl = GetBackendUrl();
            
            // Add detailed logging for debugging
            builder.Services.AddLogging(logging =>
            {
#if DEBUG
                logging.AddDebug();
                logging.SetMinimumLevel(LogLevel.Debug);
#else
                logging.SetMinimumLevel(LogLevel.Information);
#endif
            });

            // Register HttpClient with Android-specific configuration
            builder.Services.AddScoped(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<HttpClient>>();
                logger.LogInformation($"[MAUI] Configuring HttpClient with BaseAddress: {backendUrl}");
                
                // Create HttpClientHandler with Android-specific settings
                var handler = new HttpClientHandler();
                
#if ANDROID
                // For Android, we need to bypass SSL issues in development
                handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
#endif
                
                var httpClient = new HttpClient(handler)
                {
                    BaseAddress = new Uri(backendUrl),
                    Timeout = TimeSpan.FromSeconds(30)
                };
                
                // Add default headers
                httpClient.DefaultRequestHeaders.Add("User-Agent", "TouristHub-MAUI");
                httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
                
                return httpClient;
            });

            // Temporarily disable developer tools to avoid startup conflicts
#if DEBUG && !ANDROID
            builder.Services.AddBlazorWebViewDeveloperTools();
#endif

            return builder.Build();
        }

        private static string GetBackendUrl()
        {
#if ANDROID
            // For Android emulator, use 10.0.2.2 which maps to host's localhost:5499
            var backendUrl = "http://10.0.2.2:5499/";
            System.Diagnostics.Debug.WriteLine($"[MAUI ANDROID] Using Android emulator URL: {backendUrl}");
            return backendUrl;
#else
            // Try to get from environment variable first for other platforms
            var apiBaseUrl = System.Environment.GetEnvironmentVariable("API_BASE_URL");
            var backendUrl = apiBaseUrl ?? "http://localhost:5499/";
            System.Diagnostics.Debug.WriteLine($"[MAUI] Using URL: {backendUrl}");
            return backendUrl;
#endif
        }
    }
}
