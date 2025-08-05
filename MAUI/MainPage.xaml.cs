using Microsoft.Extensions.Logging;

namespace MAUI
{
    public partial class MainPage : ContentPage
    {
        private readonly ILogger<MainPage>? _logger;

        public MainPage()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("[MAUI] MainPage constructor starting");
                
                // Get logger if available
                _logger = IPlatformApplication.Current?.Services?.GetService<ILogger<MainPage>>();
                
                _logger?.LogInformation("Initializing MainPage");
                System.Diagnostics.Debug.WriteLine("[MAUI] Initializing MainPage components");
                
                InitializeComponent();
                
                // Configure BlazorWebView for better Android support
                if (blazorWebView != null)
                {
                    System.Diagnostics.Debug.WriteLine("[MAUI] BlazorWebView found, configuring");
                    
                    // Set properties for better reliability
                    blazorWebView.StartPath = "/";
                    
                    System.Diagnostics.Debug.WriteLine($"[MAUI] BlazorWebView configured - HostPage: {blazorWebView.HostPage}, StartPath: {blazorWebView.StartPath}");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("[MAUI] ERROR: BlazorWebView is null!");
                    _logger?.LogError("BlazorWebView is null after InitializeComponent");
                }
                
                _logger?.LogInformation("MainPage initialized successfully");
                System.Diagnostics.Debug.WriteLine("[MAUI] MainPage constructor completed");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error initializing MainPage");
                System.Diagnostics.Debug.WriteLine($"[MAUI] MainPage constructor error: {ex}");
                throw;
            }
        }
    }
}
