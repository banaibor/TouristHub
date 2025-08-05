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
                logging.AddDebug();
                logging.SetMinimumLevel(LogLevel.Debug);
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
                
                return httpClient;
            });

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }

        private static string GetBackendUrl()
        {
#if ANDROID
            // For Android emulator, always use 10.0.2.2 which maps to host's localhost
            var backendUrl = "http://10.0.2.2:5500/";
            System.Diagnostics.Debug.WriteLine($"[MAUI ANDROID] Using hardcoded Android emulator URL: {backendUrl}");
            return backendUrl;
#else
            // Try to get from environment variable first for other platforms
            var apiBaseUrl = Environment.GetEnvironmentVariable("API_BASE_URL");
            var backendUrl = apiBaseUrl ?? "http://localhost:5500/";
            System.Diagnostics.Debug.WriteLine($"[MAUI] Using URL: {backendUrl}");
            return backendUrl;
#endif
        }
    }
}
