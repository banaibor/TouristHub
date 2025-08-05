using Microsoft.Extensions.Logging;

namespace MAUI
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("[MAUI] MainPage constructor starting");
                
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
                }
                
                System.Diagnostics.Debug.WriteLine("[MAUI] MainPage constructor completed");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[MAUI] MainPage constructor error: {ex}");
                throw;
            }
        }
    }
}
