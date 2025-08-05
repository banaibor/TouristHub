# Android File Lock Fix Script
# Resolves the "file is being used by another process" error during Android builds

Write-Host "?? Android File Lock Fix - Starting..." -ForegroundColor Green

# Step 1: Close Visual Studio and related processes
Write-Host "?? Stopping Visual Studio and related processes..." -ForegroundColor Yellow
try {
    $processesToKill = @(
        "devenv",
        "MSBuild",
        "dotnet",
        "VBCSCompiler",
        "ServiceHub.Host.dotnet*",
        "ServiceHub.Host.CLR*",
        "ServiceHub*",
        "Microsoft.Android*",
        "java",
        "adb"
    )
    
    foreach ($processName in $processesToKill) {
        $processes = Get-Process -Name $processName -ErrorAction SilentlyContinue
        if ($processes) {
            foreach ($process in $processes) {
                try {
                    Write-Host "  ?? Stopping: $($process.ProcessName) (PID: $($process.Id))" -ForegroundColor Gray
                    $process.Kill()
                    $process.WaitForExit(5000)
                } catch {
                    Write-Host "    ?? Could not stop $($process.ProcessName): $_" -ForegroundColor Yellow
                }
            }
        }
    }
    
    # Wait a moment for processes to fully terminate
    Start-Sleep -Seconds 3
    Write-Host "? Processes stopped" -ForegroundColor Green
} catch {
    Write-Host "? Error stopping processes: $_" -ForegroundColor Red
}

# Step 2: Force unlock and remove locked directories
Write-Host "?? Force removing locked build artifacts..." -ForegroundColor Yellow
try {
    $directories = @(
        ".\MAUI\bin",
        ".\MAUI\obj",
        ".\TouristHub.ApiService\bin",
        ".\TouristHub.ApiService\obj",
        ".\TouristHub.Web\bin", 
        ".\TouristHub.Web\obj",
        ".\TouristHub.AppHost\bin",
        ".\TouristHub.AppHost\obj",
        ".\TouristHub.ServiceDefaults\bin",
        ".\TouristHub.ServiceDefaults\obj"
    )
    
    foreach ($dir in $directories) {
        if (Test-Path $dir) {
            try {
                # Use cmd /c rmdir for force removal
                cmd /c "rmdir /s /q `"$((Resolve-Path $dir).Path)`""
                Write-Host "  ??? Force removed: $dir" -ForegroundColor Gray
            } catch {
                Write-Host "  ?? Could not remove $dir : $_" -ForegroundColor Yellow
                
                # Try alternative method - take ownership and delete
                try {
                    takeown /f "$dir" /r /d y | Out-Null
                    icacls "$dir" /grant administrators:F /t | Out-Null
                    Remove-Item -Recurse -Force $dir -ErrorAction Stop
                    Write-Host "  ?? Force removed with admin rights: $dir" -ForegroundColor Gray
                } catch {
                    Write-Host "  ? Failed to remove even with admin rights: $dir" -ForegroundColor Red
                }
            }
        }
    }
    Write-Host "? Build artifacts cleaned" -ForegroundColor Green
} catch {
    Write-Host "? Error cleaning build artifacts: $_" -ForegroundColor Red
}

# Step 3: Clear temp directories
Write-Host "?? Clearing temporary directories..." -ForegroundColor Yellow
try {
    $tempDirs = @(
        "$env:TEMP\xamarin*",
        "$env:TEMP\Microsoft*",
        "$env:LOCALAPPDATA\Temp\xamarin*",
        "$env:LOCALAPPDATA\Temp\Microsoft*"
    )
    
    foreach ($tempPattern in $tempDirs) {
        $dirs = Get-ChildItem -Path (Split-Path $tempPattern) -Filter (Split-Path $tempPattern -Leaf) -Directory -ErrorAction SilentlyContinue
        foreach ($dir in $dirs) {
            try {
                Remove-Item -Recurse -Force $dir.FullName
                Write-Host "  ??? Cleared: $($dir.FullName)" -ForegroundColor Gray
            } catch {
                Write-Host "  ?? Could not clear: $($dir.FullName)" -ForegroundColor Yellow
            }
        }
    }
    Write-Host "? Temp directories cleared" -ForegroundColor Green
} catch {
    Write-Host "? Error clearing temp directories: $_" -ForegroundColor Red
}

# Step 4: Clear NuGet caches
Write-Host "?? Clearing NuGet caches..." -ForegroundColor Yellow
try {
    dotnet nuget locals all --clear
    Write-Host "? NuGet caches cleared" -ForegroundColor Green
} catch {
    Write-Host "? Error clearing NuGet caches: $_" -ForegroundColor Red
}

# Step 5: Restart dotnet build server
Write-Host "?? Restarting .NET build server..." -ForegroundColor Yellow
try {
    dotnet build-server shutdown
    Start-Sleep -Seconds 2
    Write-Host "? Build server restarted" -ForegroundColor Green
} catch {
    Write-Host "? Error restarting build server: $_" -ForegroundColor Red
}

# Step 6: Restore and build
Write-Host "?? Restoring packages..." -ForegroundColor Yellow
try {
    dotnet restore TouristHub.sln --force --no-cache
    Write-Host "? Packages restored" -ForegroundColor Green
} catch {
    Write-Host "? Error restoring packages: $_" -ForegroundColor Red
}

Write-Host "?? Building solution..." -ForegroundColor Yellow
try {
    dotnet build TouristHub.sln --configuration Debug --no-restore
    Write-Host "? Solution built successfully" -ForegroundColor Green
} catch {
    Write-Host "? Build failed. Check errors above." -ForegroundColor Red
    Write-Host "?? If errors persist, try running as Administrator" -ForegroundColor Blue
}

Write-Host "?? Android File Lock Fix completed!" -ForegroundColor Green
Write-Host "?? Tips to prevent future locks:" -ForegroundColor Blue
Write-Host "  • Close Visual Studio before cleaning" -ForegroundColor Gray
Write-Host "  • Use 'dotnet build-server shutdown' regularly" -ForegroundColor Gray
Write-Host "  • Restart your Android emulator if issues persist" -ForegroundColor Gray