using Android.App;
using Android.Content.PM;
using Android.OS;

namespace MAUI
{
    [Activity(
        Theme = "@style/Maui.SplashTheme", 
        MainLauncher = true, 
        LaunchMode = LaunchMode.SingleTop,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        protected override void OnCreate(Bundle? savedInstanceState)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("[MAUI ANDROID] MainActivity.OnCreate starting");

                // Enable WebView debugging in debug builds
#if DEBUG
                Android.Webkit.WebView.SetWebContentsDebuggingEnabled(true);
                System.Diagnostics.Debug.WriteLine("[MAUI ANDROID] WebView debugging enabled");
                
                // Force disable HTTPS for localhost development
                if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
                {
                    System.Diagnostics.Debug.WriteLine("[MAUI ANDROID] Configuring cleartext traffic for development");
                }
#endif

                base.OnCreate(savedInstanceState);
                
                System.Diagnostics.Debug.WriteLine("[MAUI ANDROID] MainActivity.OnCreate completed successfully");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[MAUI ANDROID] MainActivity.OnCreate error: {ex}");
                // Don't rethrow to prevent app crash
            }
        }

        protected override void OnStart()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("[MAUI Android] MainActivity.OnStart");
                base.OnStart();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[MAUI Android] MainActivity.OnStart error: {ex}");
                throw;
            }
        }

        protected override void OnResume()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("[MAUI Android] MainActivity.OnResume");
                base.OnResume();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[MAUI Android] MainActivity.OnResume error: {ex}");
            }
        }

        protected override void OnPause()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("[MAUI Android] MainActivity.OnPause");
                base.OnPause();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[MAUI Android] MainActivity.OnPause error: {ex}");
            }
        }

        protected override void OnDestroy()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("[MAUI Android] MainActivity.OnDestroy");
                base.OnDestroy();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[MAUI Android] MainActivity.OnDestroy error: {ex}");
                // Don't throw on destroy
            }
        }

        public override void OnBackPressed()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("[MAUI Android] MainActivity.OnBackPressed");
                base.OnBackPressed();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[MAUI Android] MainActivity.OnBackPressed error: {ex}");
            }
        }
    }
}
