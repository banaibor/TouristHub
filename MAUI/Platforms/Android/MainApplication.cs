using Android.App;
using Android.Runtime;
using System;

namespace MAUI
{
    [Application]
    public class MainApplication : MauiApplication
    {
        public MainApplication(IntPtr handle, JniHandleOwnership ownership)
            : base(handle, ownership)
        {
            System.Diagnostics.Debug.WriteLine("[MAUI Android] MainApplication constructor");
        }

        protected override MauiApp CreateMauiApp()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("[MAUI Android] Creating MauiApp");
                var app = MauiProgram.CreateMauiApp();
                System.Diagnostics.Debug.WriteLine("[MAUI Android] MauiApp created successfully");
                return app;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[MAUI Android] Error creating MauiApp: {ex}");
                throw;
            }
        }

        public override void OnCreate()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("[MAUI Android] MainApplication.OnCreate");
                base.OnCreate();
                
                // Set up global exception handling for Android
                AndroidEnvironment.UnhandledExceptionRaiser += OnAndroidUnhandledException;
                
                System.Diagnostics.Debug.WriteLine("[MAUI Android] MainApplication.OnCreate completed");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[MAUI Android] MainApplication.OnCreate error: {ex}");
                throw;
            }
        }

        private void OnAndroidUnhandledException(object? sender, RaiseThrowableEventArgs e)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"[MAUI Android] Unhandled Android Exception: {e.Exception}");
            }
            catch
            {
                // Don't throw exceptions in exception handlers
            }
        }
    }
}
