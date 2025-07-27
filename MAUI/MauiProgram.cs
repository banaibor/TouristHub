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

            // Use API_BASE_URL environment variable if set, otherwise fallback to default
            var apiBaseUrl = Environment.GetEnvironmentVariable("API_BASE_URL");
#if ANDROID
            string backendUrl;
            if (!string.IsNullOrEmpty(apiBaseUrl))
            {
                backendUrl = apiBaseUrl
                    .Replace("localhost", "10.0.2.2")
                    .Replace("127.0.0.1", "10.0.2.2");
                if (!backendUrl.EndsWith("/"))
                    backendUrl += "/";
                System.Diagnostics.Debug.WriteLine($"[MAUI ANDROID] Using backendUrl: {backendUrl}");
            }
            else
            {
                backendUrl = "http://10.0.2.2:5499/";
                System.Diagnostics.Debug.WriteLine($"[MAUI ANDROID] Using fallback backendUrl: {backendUrl}");
            }
#else
            var backendUrl = apiBaseUrl ?? "http://localhost:5499/";
#endif
            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(backendUrl) });

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
