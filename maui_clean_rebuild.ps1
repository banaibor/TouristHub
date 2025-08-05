# MAUI Android Clean and Rebuild Script
# This script thoroughly cleans and rebuilds the MAUI project to fix loading issues

Write-Host "?? Starting MAUI Android Clean and Rebuild Process..." -ForegroundColor Green

# Step 1: Clean Solution
Write-Host "?? Cleaning solution..." -ForegroundColor Yellow
try {
    dotnet clean TouristHub.sln --configuration Debug
    dotnet clean TouristHub.sln --configuration Release
    Write-Host "? Solution cleaned successfully" -ForegroundColor Green
} catch {
    Write-Host "? Error cleaning solution: $_" -ForegroundColor Red
}

# Step 2: Remove bin/obj directories manually
Write-Host "??? Removing bin/obj directories..." -ForegroundColor Yellow
try {
    $directories = @(
        ".\MAUI\bin",
        ".\MAUI\obj",
        ".\TouristHub.ApiService\bin",
        ".\TouristHub.ApiService\obj",
        ".\TouristHub.Web\bin",
        ".\TouristHub.Web\obj",
        ".\TouristHub.AppHost\bin",
        ".\TouristHub.AppHost\obj"
    )
    
    foreach ($dir in $directories) {
        if (Test-Path $dir) {
            Remove-Item -Recurse -Force $dir
            Write-Host "  ??? Removed: $dir" -ForegroundColor Gray
        }
    }
    Write-Host "? Directories cleaned successfully" -ForegroundColor Green
} catch {
    Write-Host "? Error removing directories: $_" -ForegroundColor Red
}

# Step 3: Clear NuGet cache
Write-Host "?? Clearing NuGet cache..." -ForegroundColor Yellow
try {
    dotnet nuget locals all --clear
    Write-Host "? NuGet cache cleared" -ForegroundColor Green
} catch {
    Write-Host "? Error clearing NuGet cache: $_" -ForegroundColor Red
}

# Step 4: Restore packages
Write-Host "?? Restoring NuGet packages..." -ForegroundColor Yellow
try {
    dotnet restore TouristHub.sln
    Write-Host "? Packages restored successfully" -ForegroundColor Green
} catch {
    Write-Host "? Error restoring packages: $_" -ForegroundColor Red
}

# Step 5: Build solution
Write-Host "?? Building solution..." -ForegroundColor Yellow
try {
    dotnet build TouristHub.sln --configuration Debug --no-restore
    Write-Host "? Solution built successfully" -ForegroundColor Green
} catch {
    Write-Host "? Error building solution: $_" -ForegroundColor Red
    Write-Host "?? Check the error messages above for details" -ForegroundColor Blue
}

# Step 6: Build MAUI Android specifically
Write-Host "?? Building MAUI Android project..." -ForegroundColor Yellow
try {
    dotnet build .\MAUI\MAUI.csproj -f net9.0-android --configuration Debug
    Write-Host "? MAUI Android built successfully" -ForegroundColor Green
} catch {
    Write-Host "? Error building MAUI Android: $_" -ForegroundColor Red
}

Write-Host "?? Clean and rebuild process completed!" -ForegroundColor Green
Write-Host "?? Next steps:" -ForegroundColor Blue
Write-Host "  1. Deploy to Android emulator" -ForegroundColor Gray
Write-Host "  2. Watch for '[SIMPLE TEST]' messages in debug output" -ForegroundColor Gray
Write-Host "  3. Should see 'Blazor is Working!' instead of loading screen" -ForegroundColor Gray

# Optional: Show current component configuration
Write-Host "?? Current MainPage.xaml configuration:" -ForegroundColor Blue
try {
    $mainPageContent = Get-Content ".\MAUI\MainPage.xaml" -Raw
    if ($mainPageContent -match 'ComponentType="\{x:Type local:Components\.(.+?)\}"') {
        Write-Host "  Root Component: $($matches[1])" -ForegroundColor Gray
    }
} catch {
    Write-Host "  Could not read MainPage.xaml" -ForegroundColor Red
}

Write-Host "? Ready for testing!" -ForegroundColor Green