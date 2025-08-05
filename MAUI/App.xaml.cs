using Microsoft.Extensions.Logging;

namespace MAUI
{
    public partial class App : Application
    {
        private readonly ILogger<App>? _logger;

        public App()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("[MAUI] App constructor starting");
                
                InitializeComponent();
                
                // Get logger if available
                _logger = IPlatformApplication.Current?.Services?.GetService<ILogger<App>>();
                _logger?.LogInformation("MAUI App initialized successfully");
                
                System.Diagnostics.Debug.WriteLine("[MAUI] App constructor completed");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[MAUI] App constructor error: {ex}");
                throw;
            }
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            try
            {
                _logger?.LogInformation("Creating main window");
                System.Diagnostics.Debug.WriteLine("[MAUI] Creating main window");
                
                var mainPage = new MainPage();
                var window = new Window(mainPage) 
                { 
                    Title = "TouristHub - Discover Meghalaya"
                };
                
                _logger?.LogInformation("Main window created successfully");
                System.Diagnostics.Debug.WriteLine("[MAUI] Main window created successfully");
                
                return window;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error creating main window");
                System.Diagnostics.Debug.WriteLine($"[MAUI] Error creating main window: {ex}");
                throw;
            }
        }

        protected override void OnStart()
        {
            try
            {
                _logger?.LogInformation("App starting");
                System.Diagnostics.Debug.WriteLine("[MAUI] App starting");
                base.OnStart();
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error in App.OnStart");
                System.Diagnostics.Debug.WriteLine($"[MAUI] App.OnStart error: {ex}");
                throw;
            }
        }

        protected override void OnSleep()
        {
            try
            {
                _logger?.LogInformation("App going to sleep");
                System.Diagnostics.Debug.WriteLine("[MAUI] App going to sleep");
                base.OnSleep();
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error in App.OnSleep");
                System.Diagnostics.Debug.WriteLine($"[MAUI] App.OnSleep error: {ex}");
                // Don't throw on sleep to avoid app termination
            }
        }

        protected override void OnResume()
        {
            try
            {
                _logger?.LogInformation("App resuming");
                System.Diagnostics.Debug.WriteLine("[MAUI] App resuming");
                base.OnResume();
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error in App.OnResume");
                System.Diagnostics.Debug.WriteLine($"[MAUI] App.OnResume error: {ex}");
                throw;
            }
        }
    }
}
