using Android.App;
using Android.Content.PM;
using Android.OS;
using System;

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
                System.Diagnostics.Debug.WriteLine("[MAUI Android] MainActivity.OnCreate starting");

#if DEBUG
                // Enable WebView debugging for better troubleshooting
                Android.Webkit.WebView.SetWebContentsDebuggingEnabled(true);
                System.Diagnostics.Debug.WriteLine("[MAUI Android] WebView debugging enabled");
#endif

                base.OnCreate(savedInstanceState);
                System.Diagnostics.Debug.WriteLine("[MAUI Android] MainActivity.OnCreate completed successfully");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[MAUI Android] MainActivity.OnCreate error: {ex}");
                // Don't rethrow to prevent app crash during development
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
            }
        }
    }
}
