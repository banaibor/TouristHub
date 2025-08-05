# Android MAUI Troubleshooting Script
# Run this script to help diagnose Android MAUI app issues

Write-Host "=== TouristHub Android MAUI Troubleshooting ===" -ForegroundColor Green
Write-Host ""

# Check if adb is available
try {
    $adbVersion = & adb version 2>$null
    Write-Host "? ADB is available: $($adbVersion[0])" -ForegroundColor Green
} catch {
    Write-Host "? ADB not found. Please install Android SDK Platform Tools." -ForegroundColor Red
    exit 1
}

# List connected devices
Write-Host "`n?? Connected Android devices/emulators:" -ForegroundColor Cyan
$devices = & adb devices
$devices | ForEach-Object { 
    if ($_ -match "^\w+\s+device$") {
        Write-Host "  ? $_" -ForegroundColor Green
    } elseif ($_ -match "^\w+\s+offline") {
        Write-Host "  ??  $_" -ForegroundColor Yellow
    }
}

# Check if any device is connected
$connectedDevices = & adb devices | Select-String "device$"
if ($connectedDevices.Count -eq 0) {
    Write-Host "? No Android devices/emulators connected." -ForegroundColor Red
    Write-Host "Please start an Android emulator or connect a device." -ForegroundColor Yellow
    exit 1
}

# Function to run app with detailed logging
function Start-AndroidApp {
    Write-Host "`n?? Building and deploying Android app..." -ForegroundColor Cyan
    
    # Clean and build
    Write-Host "?? Cleaning project..." -ForegroundColor Yellow
    & dotnet clean MAUI\MAUI.csproj -c Debug
    
    Write-Host "?? Building Android project..." -ForegroundColor Yellow
    $buildResult = & dotnet build MAUI\MAUI.csproj -f net9.0-android -c Debug --verbosity minimal
    
    if ($LASTEXITCODE -ne 0) {
        Write-Host "? Build failed!" -ForegroundColor Red
        return
    }
    
    Write-Host "? Build successful!" -ForegroundColor Green
    
    # Deploy and run
    Write-Host "?? Deploying to Android device..." -ForegroundColor Yellow
    & dotnet build MAUI\MAUI.csproj -t:Run -f net9.0-android -c Debug
    
    if ($LASTEXITCODE -eq 0) {
        Write-Host "? App deployed successfully!" -ForegroundColor Green
        Start-LogMonitoring
    } else {
        Write-Host "? Deployment failed!" -ForegroundColor Red
    }
}

# Function to monitor app logs
function Start-LogMonitoring {
    Write-Host "`n?? Monitoring app logs..." -ForegroundColor Cyan
    Write-Host "Press Ctrl+C to stop monitoring" -ForegroundColor Yellow
    Write-Host "----------------------------------------" -ForegroundColor Gray
    
    # Clear existing logs
    & adb logcat -c
    
    # Monitor logs with filtering for our app
    & adb logcat | Select-String -Pattern "(MAUI|TouristHub|Blazor|WebView|chromium)" | ForEach-Object {
        $line = $_.Line
        if ($line -match "ERROR|FATAL") {
            Write-Host $line -ForegroundColor Red
        } elseif ($line -match "WARN") {
            Write-Host $line -ForegroundColor Yellow
        } elseif ($line -match "MAUI|TouristHub") {
            Write-Host $line -ForegroundColor Green
        } else {
            Write-Host $line -ForegroundColor White
        }
    }
}

# Function to check network connectivity
function Test-NetworkConnectivity {
    Write-Host "`n?? Testing network connectivity..." -ForegroundColor Cyan
    
    # Test if API service is running locally
    try {
        $response = Invoke-WebRequest -Uri "http://localhost:5499/health" -TimeoutSec 5 -ErrorAction Stop
        Write-Host "? API service is reachable at localhost:5499" -ForegroundColor Green
    } catch {
        Write-Host "??  API service not reachable at localhost:5499" -ForegroundColor Yellow
        Write-Host "   Make sure TouristHub.ApiService is running" -ForegroundColor Gray
    }
    
    # Test from Android perspective (10.0.2.2)
    Write-Host "?? Testing Android emulator network access..." -ForegroundColor Cyan
    & adb shell "ping -c 1 10.0.2.2" | Out-Null
    if ($LASTEXITCODE -eq 0) {
        Write-Host "? Android emulator can reach host (10.0.2.2)" -ForegroundColor Green
    } else {
        Write-Host "? Android emulator cannot reach host" -ForegroundColor Red
    }
}

# Function to clear app data
function Clear-AppData {
    Write-Host "`n?? Clearing app data..." -ForegroundColor Cyan
    & adb shell pm clear com.companyname.maui
    Write-Host "? App data cleared" -ForegroundColor Green
}

# Main menu
while ($true) {
    Write-Host "`n=== What would you like to do? ===" -ForegroundColor Cyan
    Write-Host "1. ?? Build and run Android app"
    Write-Host "2. ?? Monitor app logs only"
    Write-Host "3. ?? Test network connectivity"
    Write-Host "4. ?? Clear app data and cache"
    Write-Host "5. ?? Restart ADB server"
    Write-Host "6. ? Exit"
    Write-Host ""
    
    $choice = Read-Host "Enter your choice (1-6)"
    
    switch ($choice) {
        "1" { Start-AndroidApp }
        "2" { Start-LogMonitoring }
        "3" { Test-NetworkConnectivity }
        "4" { Clear-AppData }
        "5" { 
            Write-Host "?? Restarting ADB server..." -ForegroundColor Yellow
            & adb kill-server
            & adb start-server
            Write-Host "? ADB server restarted" -ForegroundColor Green
        }
        "6" { 
            Write-Host "?? Goodbye!" -ForegroundColor Green
            exit 0 
        }
        default { 
            Write-Host "? Invalid choice. Please enter 1-6." -ForegroundColor Red 
        }
    }
}